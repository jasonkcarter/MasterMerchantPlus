using System;
using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage.Table;
using MMPlus.Service.Interface;

namespace MMPlus.Test.Service
{
    public class MemoryStorageRepository : IStorageRepository
    {
        private readonly Dictionary<Type, object> _tables = new Dictionary<Type, object>();

        /// <summary>
        ///     Removes the entity from the repository.
        /// </summary>
        /// <param name="entity">The entity to delete from the repository.</param>
        public bool Delete<T>(T entity) where T : TableEntity, new()
        {
            MemoryStorageTable<T> storageTable = GetTable<T>();
            if (storageTable == null)
            {
                return false;
            }
            MemoryStoragePartition<T> partition = storageTable.GetPartition(entity.PartitionKey);
            if (partition == null)
            {
                return false;
            }
            return partition.Delete(entity);
        }

        /// <summary>
        ///     Finds all entities in the repository with non-default properties similar to the given entity.
        /// </summary>
        /// <param name="partitionKey">The partition key to limit results by.</param>
        /// <param name="rowKey">The row key to limit results by.</param>
        /// <param name="timestampMin">The minimum timestamp for entities to include</param>
        /// <param name="timestampMax">The maximum timestamp for entities to include.</param>
        /// <returns>A list of matching entities.</returns>
        public IEnumerable<T> Find<T>(string partitionKey = null, string rowKey = null, DateTimeOffset? timestampMin = null,
            DateTimeOffset? timestampMax = null) where T : TableEntity, new()
        {
            MemoryStorageTable<T> storageTable = GetTable<T>();
            if (storageTable == null)
            {
                return new T[0];
            }

            if (partitionKey == null)
            {
                List<T> entityList = new List<T>();
                foreach (MemoryStoragePartition<T> partition in storageTable.GetPartitions())
                {
                    if (partition == null)
                    {
                        continue;
                    }
                    IEnumerable<T> results = partition.Find(rowKey, timestampMin, timestampMax);
                    entityList.AddRange(results);
                }
                return entityList;
            }
            MemoryStoragePartition<T> singlePartition = storageTable.GetPartition(partitionKey);
            if (singlePartition == null)
            {
                return new T[0];
            }
            return singlePartition.Find(rowKey, timestampMin, timestampMax);
        }

        /// <summary>
        ///     Gets a table reference to the Azure Table Storage table responsible for holding entities of type <c>T</c>
        /// </summary>
        /// <returns>A reference to the table that holds entities of type <c>T</c>.</returns>
        public MemoryStorageTable<T> GetTable<T>(bool createIfNotExists = true) where T : TableEntity, new()
        {
            Type type = typeof (T);
            object tableObject;
            if (_tables.TryGetValue(type, out tableObject))
            {
                return tableObject as MemoryStorageTable<T>;
            }
            if (!createIfNotExists) return null;
            var table = new MemoryStorageTable<T>();
            _tables.Add(type, table);
            return table;
        }

        /// <summary>
        ///     Replaces an existing entity or inserts a new entity if it does not exist in the repository. Because this operation
        ///     can insert or update an entity, it is also known as an upsert operation.
        /// </summary>
        /// <param name="entity">The entity to insert or replace in the repository.</param>
        public bool InsertOrReplace<T>(T entity) where T : TableEntity, new()
        {
            MemoryStorageTable<T> storageTable = GetTable<T>();
            var partition = storageTable.GetPartition(entity.PartitionKey);
            if (partition == null)
            {
                partition = storageTable.CreatePartition(entity.PartitionKey);
            }
            partition.InsertOrReplace(entity);
            return true;
        }

        /// <summary>
        ///     Completely removes the table and all data for a given entity type from Azure Table Storage. Used for cleaning up
        ///     after unit tests.
        /// </summary>
        /// <typeparam name="T">The entity type to remove the table for.</typeparam>
        public void RemoveTable<T>() where T : TableEntity, new()
        {
            Type type = typeof (T);
            _tables.Remove(type);
        }
    }
}