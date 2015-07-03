using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace MMPlus.Test.Service
{
    public class MemoryStoragePartition<T> where T : TableEntity, new()
    {
        /// <summary>
        ///     The list of entities in the table, indexed by RowKey.
        /// </summary>
        private readonly Dictionary<string, T> _rows = new Dictionary<string, T>();

        /// <summary>
        ///     Removes the entity from the repository.
        /// </summary>
        /// <param name="entity">The entity to delete from the repository.</param>
        /// <returns>True upon successful delete. False if there was an error or if the entity did not exist in the partition.</returns>
        public bool Delete(T entity)
        {
            return _rows.Remove(entity.RowKey);
        }

        /// <summary>
        ///     Finds all entities in the table with non-default properties similar to the given terms.
        /// </summary>
        /// <param name="rowKey">The row key to limit results by.</param>
        /// <param name="timestampMin">The minimum timestamp for entities to include</param>
        /// <param name="timestampMax">The maximum timestamp for entities to include.</param>
        /// <returns>A list of matching entities.</returns>
        public IEnumerable<T> Find(string rowKey = null, DateTimeOffset? timestampMin = null,
            DateTimeOffset? timestampMax = null)
        {
            IEnumerable<T> values;
            if (rowKey != null)
            {
                T row;
                if (!_rows.TryGetValue(rowKey, out row)) return new T[0];
                values = new[] {row};
            }
            else
            {
                values = _rows.Values;
            }
            if (timestampMin != null)
            {
                values = values.Where(x => x.Timestamp >= timestampMin);
            }
            if (timestampMax != null)
            {
                values = values.Where(x => x.Timestamp <= timestampMax);
            }
            return values;
        }

        /// <summary>
        ///     Replaces an existing entity or inserts a new entity if it does not exist in the table. Because this operation
        ///     can insert or update an entity, it is also known as an upsert operation.
        /// </summary>
        /// <param name="entity">The entity to insert or replace in the repository.</param>
        public void InsertOrReplace(T entity)
        {
            if (_rows.ContainsKey(entity.RowKey))
            {
                lock (_rows[entity.RowKey])
                {
                    T existing = _rows[entity.RowKey];
                    if (entity.ETag == "*" || entity.ETag == existing.ETag)
                    {
                        entity.ETag = Guid.NewGuid().ToString("N");
                        _rows[entity.RowKey] = entity;
                        return;
                    }
                }
                throw new StorageException(new RequestResult {HttpStatusCode = 412},
                    "Optimistic concurrency violation – entity has changed since it was retrieved.", new Exception());
            }
            else
            {
                _rows.Add(entity.RowKey, entity);
            }
        }
    }
}