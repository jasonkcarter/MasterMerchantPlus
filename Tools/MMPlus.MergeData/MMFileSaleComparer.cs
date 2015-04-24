using System;
using System.Collections.Generic;
using MMPlus.Client.Model;

namespace MMPlus.MergeData
{
    // ReSharper disable once InconsistentNaming
    /// <summary>
    ///     Used to sort sales by the order they should appear in a Master Merchant data file, i.e. by item base id, item
    ///     index, and then by timestamp.
    /// </summary>
    public class MMFileSaleComparer : IComparer<EsoSale>
    {
        /// <summary>
        ///     Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>
        ///     A signed integer that indicates the relative values of <paramref name="x" /> and <paramref name="y" />, as shown in
        ///     the following table.Value Meaning Less than zero<paramref name="x" /> is less than <paramref name="y" />.Zero
        ///     <paramref name="x" /> equals <paramref name="y" />.Greater than zero<paramref name="x" /> is greater than
        ///     <paramref name="y" />.
        /// </returns>
        public int Compare(EsoSale x, EsoSale y)
        {
            var result = String.Compare(x.ItemBaseId, y.ItemBaseId, StringComparison.InvariantCultureIgnoreCase);
            if (result != 0)
            {
                return result;
            }
            result = String.Compare(x.ItemIndex, y.ItemIndex, StringComparison.InvariantCultureIgnoreCase);
            if (result != 0)
            {
                return result;
            }
            return x.SaleTimestamp.CompareTo(y.SaleTimestamp);
        }
    }
}