using System;

namespace MMPlus.Shared.Interface
{
    /// <summary>
    ///     Represents global aggregated Guild Store sales data for a single Elder Scrolls inventory item.
    /// </summary>
    public interface IEsoItemSalesData : IEsoItem
    {
        /// <summary>
        ///     Gets or sets the date that the sales data pertains to.
        /// </summary>
        DateTimeOffset SalesDate { get; set; }

        /// <summary>
        ///     Gets or sets the weighted average price of each individual inventory item of this type.
        /// </summary>
        int SalesPriceAverage { get; set; }

        /// <summary>
        ///     Gets or sets the total sum of all prices for all sales for this item type.
        /// </summary>
        long SalesPriceSum { get; set; }

        /// <summary>
        ///     Gets or sets the total sum of all quantities for all sales for this item type.
        /// </summary>
        long SalesQuantitySum { get; set; }
    }
}