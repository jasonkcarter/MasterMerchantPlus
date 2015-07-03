﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using MMPlus.Service.Interface;
using MMPlus.Service.Model;
using MMPlus.Shared.Interface;

namespace MMPlus.Service.Services
{
    [ServiceContract(Namespace = "https://carterjk.com/mmplus/2015/05")]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class SyncService : ISyncService
    {
        /// <summary>
        ///     The storage data store for all Sales tables.
        /// </summary>
        private readonly IStorageRepository _repository;

        /// <summary>
        ///     Initializes a new instance of the <see cref="SyncService" /> class.
        /// </summary>
        /// <param name="repository">The storage data store for all Sales tables.</param>
        public SyncService(IStorageRepository repository)
        {
            _repository = repository;
        }

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
        [OperationContract(AsyncPattern = true)]
        public IEnumerable<IEsoItemSalesData> Get(string machineId, int getTimestampStart, int getTimestampEnd)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Submits a collection of sales data to the community server for inclusion in the aggregate data.  Required to be
        ///     called for a given machine id before the Get operation will return any data.
        /// </summary>
        /// <param name="machineId">The unique identifier of the machine requesting data.</param>
        /// <param name="sales">The sales data to submit.</param>
        [OperationContract(IsOneWay = true, AsyncPattern = true)]
        public void Put(string machineId, IEnumerable<IEsoSale> sales)
        {
            foreach (IEsoSale sale in sales)
            {
                int relativeOrderIndex = sale.RelativeOrderIndex;
                string partitionKey = EsoSale.GeneratePartitionKey(sale);
                string rowKey = EsoSale.GenerateRowKey(sale);
                EsoSale existing =
                    _repository.Find<EsoSale>(partitionKey, rowKey)
                        .FirstOrDefault();
                if (existing != null)
                {
                    if (existing.SubmitterCount < 3 && !existing.Submitter.Contains(machineId))
                    {
                        existing.SubmitterCount++;
                        existing.Submitter += "; " + machineId;
                        _repository.InsertOrReplace(existing);
                    }
                }
                else
                {
                    EsoSale newSale = EsoSale.Create(sale);
                    newSale.Submitter = machineId;
                    newSale.SubmitterCount = 1;
                    _repository.InsertOrReplace(newSale);
                }
            }
        }
    }
}