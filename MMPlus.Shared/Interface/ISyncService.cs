using System.Collections.Generic;

namespace MMPlus.Shared.Interface
{
    /// <summary>
    ///     Supports server-side operations related to submitting and receiving guild sales data to and from a shared community data store.
    /// </summary>
    public interface ISyncService
    {
        /// <summary>
        ///     Retrieves aggregate sales data for a given timestmap range for a given machine id.  If the service has not
        ///     completed a Put operation for the given machine id yet, the list will be empty.
        /// </summary>
        /// <param name="machineId">The unique identifier of the machine submitting data.</param>
        /// <param name="getTimestampStart">The Unix timestamp of the first aggregate sales data to retreive.</param>
        /// <param name="getTimestampEnd">The Unix timestamp of the last aggregate sales data to retreive.</param>
        /// <returns>
        ///     A collection of aggregate sales data from all other guilds on the server for the requested time span.
        /// </returns>
        IEnumerable<IEsoItemSalesData> Get(string machineId, int getTimestampStart, int getTimestampEnd);

        /// <summary>
        ///     Submits a collection of sales data to the community server for inclusion in the aggregate data.  Required to be
        ///     called for a given machine id before the Get operation will return any data.
        /// </summary>
        /// <param name="machineId">The unique identifier of the machine requesting data.</param>
        /// <param name="sales">The sales data to submit.</param>
        void Put(string machineId, IEnumerable<IEsoSale> sales);
    }
}