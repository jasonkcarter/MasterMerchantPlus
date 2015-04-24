using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Antlr4.Runtime;
using MMPlus.Client.Model;

namespace Lua.EsoSavedVariables
{
    // ReSharper disable once InconsistentNaming
    /// <summary>
    ///     Used to read a Master Merchant mule data file and extract sales data, optionally filtered by date.
    /// </summary>
    public class MMSavedVariableReader
    {
        /// <summary>
        ///     Gets a list of Elder Scrolls Online guild store sales items contained within FilePath.
        /// </summary>
        /// <param name="filters">(optional) If set, only returns sales that match the give filters.</param>
        /// <returns>The list of sales retreived.</returns>
        public List<EsoSale> GetEsoGuildStoreSales(Stream stream, params EsoSaleFilter[] filters)
        {
            // Holds the sales discovered in the saved variables file
            var sales = new List<EsoSale>();

            if (!ProcessEsoGuildStoreSales(stream, sales.Add, filters))
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
        public Task<List<EsoSale>> GetEsoGuildStoreSalesAsync(Stream stream, params EsoSaleFilter[] filters)
        {
            return Task<List<EsoSale>>.Factory.StartNew(() => GetEsoGuildStoreSales(stream, filters));
        }

        public bool ProcessEsoGuildStoreSales(Stream stream, Action<EsoSale> onSaleFound,
            params EsoSaleFilter[] filters)
        {
            if (stream == null)
            {
                return false;
            }

            // Set a default filter to not filter anything
            if (filters == null || filters.Length == 0)
            {
                filters = new EsoSaleFilter[1];
                filters[0] = new EsoSaleFilter();
            }

            // Set up our custom Lua listener for extracting sale data
            var listener = new MMLuaListener();

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
                                            || filter.GuildName.Equals(sale.GuildName,
                                                StringComparison.CurrentCultureIgnoreCase))
                    .Any(filter => (filter.TimestampMinimum == null || sale.SaleTimestamp >= filter.TimestampMinimum)
                                   &&
                                   (filter.TimestampMaximum == null || sale.SaleTimestamp <= filter.TimestampMaximum)))
                {
                    onSaleFound(sale);
                }
            };
            parser.AddParseListener(listener);

            // Run the Lua parser on the saved variables file
            parser.exp();

            return true;
        }
    }
}