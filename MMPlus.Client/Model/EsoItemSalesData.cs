using System;
using MMPlus.Shared.Interface;

namespace MMPlus.Client.Model
{
    /// <summary>
    ///     Represents global aggregated Guild Store sales data for a single Elder Scrolls inventory item.
    /// </summary>
    public class EsoItemSalesData : EsoItem, IEsoItemSalesData
    {
        /// <summary>
        ///     Gets or sets the date that the sales data pertains to.
        /// </summary>
        public DateTimeOffset SalesDate { get; set; }

        /// <summary>
        ///     Gets or sets the weighted average price of each individual inventory item of this type.
        /// </summary>
        public int SalesPriceAverage { get; set; }

        /// <summary>
        ///     Gets or sets the total sum of all prices for all sales for this item type.
        /// </summary>
        public long SalesPriceSum { get; set; }

        /// <summary>
        ///     Gets or sets the total sum of all quantities for all sales for this item type.
        /// </summary>
        public long SalesQuantitySum { get; set; }
    }
}