using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage.Table;

namespace MMPlus.Test.Service
{
    public class MemoryStorageTable<T> where T : TableEntity, new()
    {
        private readonly Dictionary<string, MemoryStoragePartition<T>> _partitions =
            new Dictionary<string, MemoryStoragePartition<T>>();

        public MemoryStoragePartition<T> CreatePartition(string partitionKey)
        {
            var partition = new MemoryStoragePartition<T>();
            _partitions.Add(partitionKey, partition);
            return partition;
        }

        /// <summary>
        ///     Removes the entity from the table, if it exists.
        /// </summary>
        /// <param name="entity">The entity to delete from the repository.</param>
        /// <returns>True upon successful delete. False if there was an error or if the entity did not exist in the table.</returns>
        public bool Delete(T entity)
        {
            var partition = GetPartition(entity.PartitionKey);
            if (partition == null)
            {
                return false;
            }
            return partition.Delete(entity);
        }

        public MemoryStoragePartition<T> GetPartition(string partitionKey)
        {
            MemoryStoragePartition<T> partition;
            if (!_partitions.TryGetValue(partitionKey, out partition)) return null;
            return partition;
        }


        /// <summary>
        ///     Replaces an existing entity or inserts a new entity if it does not exist in the table. Because this operation
        ///     can insert or update an entity, it is also known as an upsert operation.
        /// </summary>
        /// <param name="entity">The entity to insert or replace in the table.</param>
        public void InsertOrReplace(T entity)
        {
            var partition = GetPartition(entity.PartitionKey);
            if (partition == null)
            {
                partition = CreatePartition(entity.PartitionKey);
            }
            partition.InsertOrReplace(entity);
        }
    }
}