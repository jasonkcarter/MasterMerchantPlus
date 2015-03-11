using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Antlr4.Runtime;
using Lua;
using MMPlus.Shared.Model;

namespace MMPlus.Shared.EsoSavedVariables
{
    // ReSharper disable once InconsistentNaming
    /// <summary>
    ///     Used to read a Master Merchant mule data file and extract sales data, optionally filtered by date.
    /// </summary>
    public class MMSavedVariableReader
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="MMSavedVariableReader" /> class.
        /// </summary>
        /// <param name="filePath">The fully-qualified file system path of the Master Merchant saved variables file to read.</param>
        public MMSavedVariableReader(string filePath = null)
        {
            FilePath = filePath;
        }

        /// <summary>
        ///     The fully-qualified file system path of the Master Merchant saved variables file to read.
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        ///     Gets a list of Elder Scrolls Online guild store sales items contained within FilePath.
        /// </summary>
        /// <param name="filters">(optional) If set, only returns sales that match the give filters.</param>
        /// <returns>The list of sales retreived.</returns>
        public List<EsoGuildStoreSale> GetEsoGuildStoreSales(params EsoGuildStoreSaleFilter[] filters)
        {
            // Holds the sales discovered in the saved variables file
            var sales = new List<EsoGuildStoreSale>();

            if (!ProcessEsoGuildStoreSales(sales.Add, filters))
            {
                return null;
            }

            return sales;
        }

        /// <summary>
        ///     Asynchronously gets a list of Elder Scrolls Online guild store sales items contained within FilePath.
        /// </summary>
        /// <param name="filters">(optional) If set, only returns sales that match the give filters.</param>
        /// <returns>An asynchronous task containing the list of sales retreived.</returns>
        public Task<List<EsoGuildStoreSale>> GetEsoGuildStoreSalesAsync(params EsoGuildStoreSaleFilter[] filters)
        {
            return Task<List<EsoGuildStoreSale>>.Factory.StartNew(() => GetEsoGuildStoreSales(filters));
        }

        public bool ProcessEsoGuildStoreSales(Action<EsoGuildStoreSale> onSaleFound,
            params EsoGuildStoreSaleFilter[] filters)
        {
            if (FilePath == null)
            {
                return false;
            }

            // Set a default filter to not filter anything
            if (filters == null || filters.Length == 0)
            {
                filters = new EsoGuildStoreSaleFilter[1];
                filters[0] = new EsoGuildStoreSaleFilter();
            }

            // Set up our custom Lua listener for extracting sale data
            var listener = new MMLuaListener();

            // Open the saved variables file for reading
            using (var stream = File.OpenRead(FilePath))
            {
                // Lua parser stuff
                var charStream = new AntlrInputStream(stream);
                var lexer = new LuaLexer(charStream);
                var tokenStream = new CommonTokenStream(lexer);
                var parser = new LuaParser(tokenStream);
                listener.SaleFound += (sender, saleFoundArgs) =>
                {
                    // Make sure the sale matches at least one of the filters, then add it to the list
                    var sale = saleFoundArgs.Sale;
                    if (filters.Where(filter => string.IsNullOrEmpty(filter.GuildName)
                                                || filter.GuildName.Equals(sale.Guild,
                                                    StringComparison.CurrentCultureIgnoreCase))
                        .Any(filter => (filter.TimestampMinimum == null || sale.TimestampInt >= filter.TimestampMinimum)
                                       &&
                                       (filter.TimestampMaximum == null || sale.TimestampInt <= filter.TimestampMaximum)))
                    {
                        onSaleFound(sale);
                    }
                };
                parser.AddParseListener(listener);

                // Run the Lua parser on the saved variables file
                parser.exp();
            }

            return true;
        }
    }
}