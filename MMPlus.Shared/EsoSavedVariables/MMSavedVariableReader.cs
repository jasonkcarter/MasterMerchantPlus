using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MMPlus.Shared.Model;

namespace MMPlus.Shared.EsoSavedVariables
{
    // ReSharper disable once InconsistentNaming
    /// <summary>
    ///     Used to read a Master Merchant mule data file and extract sales data, optionally filtered by date.
    /// </summary>
    public class MMSavedVariableReader
    {
        private const char TokenFieldKeyConstructor = '[';
        private const char TokenFieldKeyExit = ']';
        private const char TokenFieldOperator = '=';
        private const char TokenFieldSeparator = ',';
        private const char TokenNormalString = '"';
        private const char TokenTableConstructor = '{';
        private const char TokenTableExit = '}';
        private readonly List<EsoSale> _currentItemSales = new List<EsoSale>();
        private string _currentFieldKey;
        private string _currentFieldValue;

        /// <summary>
        ///     The unique id for the base item (without special traits) defined in the current scope.
        /// </summary>
        private string _currentItemBaseId;

        /// <summary>
        ///     The current state of the parse tree walker's position in terms of the data represented.
        /// </summary>
        private MMSavedVariableScope _currentScope;

        private TokenType _currentTokenType = TokenType.None;
        private StringBuilder _currentTokenValue = new StringBuilder();

        /// <summary>
        ///     Initializes a new instance of the <see cref="MMSavedVariableReader" /> class.
        /// </summary>
        /// <param name="filePath">The fully-qualified file system path of the Master Merchant saved variables file to read.</param>
        public MMSavedVariableReader(string filePath = null)
        {
            FilePath = filePath;
        }

        /// <summary>
        ///     Gets the name of the account associated with the current parse tree scope.
        /// </summary>
        public string CurrentAccountName { get; private set; }

        /// <summary>
        ///     Gets the instance of the item currently being populated with data from the parse tree.
        /// </summary>
        public EsoItem CurrentItem { get; private set; }

        /// <summary>
        ///     Gets the instance of the sale currently being populated with data from the parse tree.
        /// </summary>
        public EsoSale CurrentSale { get; private set; }

        /// <summary>
        ///     The fully-qualified file system path of the Master Merchant saved variables file to read.
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        ///     Gets or sets the criteria used by member functions to determine which sale records from the data file to
        ///     return/process.
        /// </summary>
        public EsoSaleFilter[] Filters { get; set; }


        public void EnterTableconstructor()
        {
            _currentScope++;
            switch (_currentScope)
            {
                case MMSavedVariableScope.AccountData:
                    CurrentAccountName = _currentFieldKey;
                    break;

                case MMSavedVariableScope.EsoItemBase:

                    // Save the name of the base item scope field as the item id for all items within the scope.
                    _currentItemBaseId = _currentFieldKey;
                    break;

                case MMSavedVariableScope.EsoItem:

                    // Instantiate new item variety
                    if (string.IsNullOrEmpty(_currentItemBaseId)
                        || string.IsNullOrEmpty(_currentFieldKey))
                    {
                        CurrentItem = null;
                    }
                    else
                    {
                        CurrentItem = new EsoItem
                        {
                            BaseId = _currentItemBaseId,
                            ItemIndex = _currentFieldKey
                        };
                        _currentItemSales.Clear();
                    }
                    break;
                case MMSavedVariableScope.EsoGuildStoreSale:

                    if (CurrentItem == null)
                    {
                        CurrentSale = null;
                    }
                    else
                    {
                        CurrentSale = new EsoSale
                        {
                            Submitter = CurrentAccountName
                        };
                    }
                    break;
            }
        }

        public void ExitField()
        {
            switch (_currentScope)
            {
                case MMSavedVariableScope.EsoItem:
                    if (CurrentItem != null)
                    {
                        CurrentItem.Set(_currentFieldKey, _currentFieldValue);
                    }
                    break;
                case MMSavedVariableScope.EsoGuildStoreSale:
                    if (CurrentSale != null)
                    {
                        CurrentSale.Set(_currentFieldKey, _currentFieldValue);
                    }
                    break;
            }
        }

        public void ExitTableconstructor(Action<EsoSale> onSaleFound)
        {
            switch (_currentScope)
            {
                case MMSavedVariableScope.EsoGuildStoreSale:
                    // Exiting a sale scope
                    if (CurrentSale != null)
                    {
                        _currentItemSales.Add(CurrentSale);
                    }
                    break;
                case MMSavedVariableScope.EsoItem:
                    if (CurrentItem != null && _currentItemSales.Count > 0)
                    {
                        // Extract the item's name from the last sale's ItemLink property, since it's not stored in the item scope as a field.
                        if (string.IsNullOrEmpty(CurrentItem.Name))
                        {
                            EsoSale lastSale = _currentItemSales.Last();
                            CurrentItem.Name = lastSale.GetItemNameFromLink();
                        }

                        // Report the sale.
                        if (onSaleFound != null)
                        {
                            foreach (var sale in _currentItemSales)
                            {
                                // Make sure the sale matches at least one of the filters, then add it to the list
                                if (Filters.Where(filter => string.IsNullOrEmpty(filter.GuildName)
                                                            || filter.GuildName.Equals(sale.GuildName,
                                                                StringComparison.CurrentCultureIgnoreCase))
                                    .Any(
                                        filter =>
                                            (filter.TimestampMinimum == null ||
                                             sale.SaleTimestamp >= filter.TimestampMinimum)
                                            &&
                                            (filter.TimestampMaximum == null ||
                                             sale.SaleTimestamp <= filter.TimestampMaximum)))
                                {
                                    sale.Set(CurrentItem);
                                    onSaleFound( sale );
                                }
                            }
                        }
                    }
                    break;
            }
            _currentScope--;
        }

        /// <summary>
        ///     Asynchronously gets a list of Elder Scrolls Online guild store sales items contained within FilePath.
        /// </summary>
        /// <returns>An asynchronous task containing the list of sales retreived.</returns>
        public Task<List<EsoSale>> GetEsoGuildStoreSalesAsync()
        {
            return Task<List<EsoSale>>.Factory.StartNew(GetEsoSales);
        }

        /// <summary>
        ///     Gets a list of Elder Scrolls Online guild store sales items contained within FilePath.
        /// </summary>
        /// <returns>The list of sales retreived.</returns>
        public List<EsoSale> GetEsoSales()
        {
            // Holds the sales discovered in the saved variables file
            var sales = new List<EsoSale>();

            if (!ProcessEsoGuildStoreSales(sales.Add))
            {
                return null;
            }

            return sales;
        }

        public bool ProcessEsoGuildStoreSales(Action<EsoSale> onSaleFound)
        {
            if (FilePath == null)
            {
                return false;
            }

            // Set a default filter to not filter anything
            if (Filters == null || Filters.Length == 0)
            {
                Filters = new EsoSaleFilter[1];
                Filters[0] = new EsoSaleFilter();
            }

            // Open the saved variables file for reading
            using (var stream = File.OpenRead(FilePath))
            {
                using (var streamReader = new StreamReader(stream))
                {
                    var buffer = new char[65536];
                    int bytesRead;
                    while ((bytesRead = streamReader.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        for (int i = 0; i < bytesRead; i++)
                        {
                            char c = buffer[i];
                            switch (_currentTokenType)
                            {
                                    // Skip the variable declaration and go straight to the value.
                                case TokenType.None:
                                    if (c == TokenFieldOperator)
                                    {
                                        _currentTokenType = TokenType.FieldValue;
                                    }
                                    break;
                                case TokenType.TableConstructor:
                                    if (c == TokenFieldKeyConstructor)
                                    {
                                        _currentTokenType = TokenType.FieldKey;
                                    }
                                    else if (c == TokenTableExit)
                                    {
                                        _currentTokenType = TokenType.TableConstructor;
                                        ExitTableconstructor(onSaleFound);
                                    }
                                    break;
                                case TokenType.FieldKey:

                                    // Extract the key value for the current field
                                    if (c == TokenFieldKeyExit)
                                    {
                                        _currentTokenType = TokenType.FieldOperator;
                                        _currentFieldKey = GetString(_currentTokenValue.ToString());
                                        _currentTokenValue = new StringBuilder();
                                    }
                                    else
                                    {
                                        _currentTokenValue.Append(c);
                                    }
                                    break;
                                case TokenType.FieldOperator:
                                    if (c == TokenFieldOperator)
                                    {
                                        _currentTokenType = TokenType.FieldValue;
                                    }
                                    break;
                                case TokenType.FieldValue:
                                    if (_currentTokenValue.Length == 0)
                                    {
                                        if (c == TokenTableConstructor)
                                        {
                                            _currentTokenType = TokenType.TableConstructor;
                                            EnterTableconstructor();
                                        }
                                        else if (!char.IsWhiteSpace(c))
                                        {
                                            _currentTokenValue.Append(c);
                                        }
                                        break;
                                    }

                                    if (c == TokenTableExit)
                                    {
                                        _currentFieldValue = _currentTokenValue.ToString();
                                        _currentTokenValue = new StringBuilder();
                                        ExitField();
                                        _currentTokenType = TokenType.TableConstructor;
                                        ExitTableconstructor(onSaleFound);
                                        break;
                                    }
                                    if (_currentTokenValue[0] == TokenNormalString)
                                    {
                                        _currentTokenValue.Append(c);
                                        if (c == TokenNormalString)
                                        {
                                            _currentFieldValue = GetString(_currentTokenValue.ToString());
                                            _currentTokenValue = new StringBuilder();
                                            _currentTokenType = TokenType.TableConstructor;
                                            ExitField();
                                        }
                                        break;
                                    }

                                    if (char.IsWhiteSpace(c) || c == TokenFieldSeparator)
                                    {
                                        _currentFieldValue = _currentTokenValue.ToString();
                                        _currentTokenValue = new StringBuilder();
                                        _currentTokenType = TokenType.TableConstructor;
                                        ExitField();
                                        break;
                                    }

                                    _currentTokenValue.Append(c);
                                    break;
                            }
                        }
                    }
                }
            }

            return true;
        }

        /// <summary>
        ///     Gets the given expression as a string value.
        /// </summary>
        /// <returns>The string value of the expression.</returns>
        private static string GetString(string text)
        {
            if (text.Length > 1
                && text.First() == TokenNormalString
                && text.Last() == TokenNormalString)
            {
                text = text.Substring(1, text.Length - 2);
            }
            return text;
        }

        private enum TokenType
        {
            None,
            FieldKey,
            FieldOperator,
            FieldValue,
            TableConstructor
        }
    }
}