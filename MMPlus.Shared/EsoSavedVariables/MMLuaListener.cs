using System;
using Lua;
using MMPlus.Shared.Model;

namespace MMPlus.Shared.EsoSavedVariables
{
    /// <summary>
    ///     Used to report sales nodes in the Lua parse tree for a Master Merchant data file
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public class MMLuaListener : LuaBaseListener
    {
        /// <summary>
        ///     The unique id for the base item (without special traits) defined in the current scope.
        /// </summary>
        private string _currentItemBaseId;

        /// <summary>
        ///     The current state of the parse tree walker's position in terms of the data represented.
        /// </summary>
        private MMSavedVariableScope _currentScope;

        /// <summary>
        ///     Occurs when a sale node is found in the Master Merchant data file.
        /// </summary>
        public event EventHandler<EsoGuildStoreSaleEventArgs> SaleFound;

        /// <summary>
        ///     Gets the instance of the item currently being populated with data from the parse tree.
        /// </summary>
        public EsoItem CurrentItem { get; private set; }

        /// <summary>
        ///     Gets the instance of the sale currently being populated with data from the parse tree.
        /// </summary>
        public EsoSale CurrentSale { get; private set; }

        /// <summary>
        /// Gets the name of the account associated with the current parse tree scope.
        /// </summary>
        public string CurrentAccountName { get; private set; }

        /// <summary>
        ///     Enter a parse tree produced by <see cref="LuaParser.tableconstructor" />.
        ///     <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void EnterTableconstructor(LuaParser.TableconstructorContext context)
        {
            var field = GetParentField(context);

            _currentScope++;
            switch (_currentScope)
            {
                case MMSavedVariableScope.AccountData:
                    if (field != null)
                    {
                        CurrentAccountName = field.Name;
                    }
                    break;

                case MMSavedVariableScope.EsoItemBase:

                    // Save the name of the base item scope field as the item id for all items within the scope.
                    if (field != null)
                    {
                        _currentItemBaseId = field.Name;
                    }
                    break;

                case MMSavedVariableScope.EsoItem:

                    // Instantiate new item variety
                    if (string.IsNullOrEmpty(_currentItemBaseId)
                        || field == null
                        || string.IsNullOrEmpty(field.Name))
                    {
                        CurrentItem = null;
                    }
                    else
                    {
                        CurrentItem = new EsoItem
                        {
                            BaseId = _currentItemBaseId,
                            ItemIndex = field.Name
                        };
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
                            //Submitter = CurrentAccountName
                        };
                        CurrentSale.Set(CurrentItem);
                    }
                    break;
            }
        }

        /// <summary>
        ///     Exit a parse tree produced by <see cref="LuaParser.field" />.
        ///     <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitField(LuaParser.FieldContext context)
        {
            switch (_currentScope)
            {
                case MMSavedVariableScope.EsoItem:
                    if (CurrentItem != null)
                    {
                        LuaTableField itemField = GetField(context);
                        if (itemField != null)
                        {
                            CurrentItem.Set(itemField);
                        }
                    }
                    break;
                case MMSavedVariableScope.EsoGuildStoreSale:
                    if (CurrentSale != null)
                    {
                        LuaTableField saleField = GetField(context);
                        if (saleField != null)
                        {
                            CurrentSale.Set(saleField);
                        }
                    }
                    break;
            }
        }

        /// <summary>
        ///     Exit a parse tree produced by <see cref="LuaParser.tableconstructor" />.
        ///     <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public override void ExitTableconstructor(LuaParser.TableconstructorContext context)
        {
            switch (_currentScope)
            {
                case MMSavedVariableScope.EsoGuildStoreSale:

                    // Exiting a sale scope
                    if (CurrentSale != null)
                    {
                        // Extract the item's name from the sale's ItemLink property, since it's not stored in the item scope as a field.
                        if (CurrentItem != null && string.IsNullOrEmpty(CurrentItem.Name))
                        {
                            CurrentItem.Name = CurrentSale.GetItemNameFromLink();
                        }

                        // Report the sale.
                        if (SaleFound != null)
                        {
                            SaleFound(this, new EsoGuildStoreSaleEventArgs {Sale = CurrentSale});
                        }
                    }
                    break;
            }
            _currentScope--;
        }
    }
}