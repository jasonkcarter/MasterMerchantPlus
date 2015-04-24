using System;
using System.Collections.Generic;
using Humanizer;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace MMPlus.Service.Data
{
    /// <summary>
    ///     A repository provider for Azure Table Storage backends.
    /// </summary>
    public class TableStorageRepository
    {
        /// <summary>
        ///     The connection string for the Azure Table Storage account.
        /// </summary>
        private readonly string _connectionString;

        private readonly string _tablePrefix;
        private readonly Dictionary<Type, CloudTable> _tables = new Dictionary<Type, CloudTable>();

        /// <summary>
        ///     Initializes a new instance of the <see cref="TableStorageRepository" /> class.
        /// </summary>
        /// <param name="connectionString">The connection string for the Azure Table Storage account.</param>
        public TableStorageRepository(string connectionString, string tablePrefix = "")
        {
            _connectionString = connectionString;
            _tablePrefix = tablePrefix;
        }

        /// <summary>
        ///     Removes the entity from the repository.
        /// </summary>
        /// <param name="entity">The entity to delete from the repository.</param>
        public bool Delete<T>(T entity) where T : TableEntity
        {
            CloudTable table = GetTable<T>();
            var deleteOperation = TableOperation.Delete(entity);
            TableResult result = table.Execute(deleteOperation);
            return result.HttpStatusCode == 204;
        }

        /// <summary>
        ///     Finds all entities in the repository with non-default properties similar to the given entity.
        /// </summary>
        /// <param name="partitionKey">The partition key to limit results by.</param>
        /// <param name="rowKey">The row key to limit results by.</param>
        /// <param name="timestampMin">The minimum timestamp for entities to include</param>
        /// <param name="timestampMax">The maximum timestamp for entities to include.</param>
        /// <returns>A list of matching entities.</returns>
        public IEnumerable<T> Find<T>(string partitionKey, string rowKey = null, DateTimeOffset? timestampMin = null,
            DateTimeOffset? timestampMax = null) where T : TableEntity, new()
        {
            CloudTable table = GetTable<T>();
            string filter = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey);
            if (!string.IsNullOrEmpty(rowKey))
            {
                filter = TableQuery.CombineFilters(filter,
                    TableOperators.And,
                    TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, rowKey));
            }
            if (timestampMin != null)
            {
                filter = TableQuery.CombineFilters(filter,
                    TableOperators.And,
                    TableQuery.GenerateFilterConditionForDate("Timestamp", QueryComparisons.GreaterThanOrEqual,
                        timestampMin.Value));
            }
            if (timestampMax != null)
            {
                filter = TableQuery.CombineFilters(filter,
                    TableOperators.And,
                    TableQuery.GenerateFilterConditionForDate("Timestamp", QueryComparisons.LessThanOrEqual,
                        timestampMax.Value));
            }
            var query = new TableQuery<T>().Where(filter);
            return table.ExecuteQuery(query);
        }

        /// <summary>
        ///     Gets a table reference to the Azure Table Storage table responsible for holding entities of type <c>T</c>
        /// </summary>
        /// <returns>A reference to the table that holds entities of type <c>T</c>.</returns>
        public CloudTable GetTable<T>(bool createIfNotExists = true) where T : TableEntity
        {
            CloudTable table;
            Type type = typeof (T);
            if (_tables.TryGetValue(type, out table))
            {
                return table;
            }
            var account = CloudStorageAccount.Parse(_connectionString);
            CloudTableClient client = account.CreateCloudTableClient();
            string tableName = _tablePrefix + type.Name.Pluralize();
            table = client.GetTableReference(tableName);
            if (createIfNotExists)
            {
                table.CreateIfNotExists();
            }
            _tables.Add(type, table);
            return table;
        }

        /// <summary>
        ///     Replaces an existing entity or inserts a new entity if it does not exist in the repository. Because this operation
        ///     can insert or update an entity, it is also known as an upsert operation.
        /// </summary>
        /// <param name="entity">The entity to insert or replace in the repository.</param>
        public bool InsertOrReplace<T>(T entity) where T : TableEntity
        {
            CloudTable table = GetTable<T>();
            var upsertOperation = TableOperation.InsertOrReplace(entity);
            TableResult result = table.Execute(upsertOperation);
            return result.HttpStatusCode == 204;
        }

        /// <summary>
        ///     Completely removes the table and all data for a given entity type from Azure Table Storage. Used for cleaning up
        ///     after unit tests.
        /// </summary>
        /// <typeparam name="T">The entity type to remove the table for.</typeparam>
        public void RemoveTable<T>() where T : TableEntity
        {
            var table = GetTable<T>(false);
            table.DeleteIfExists();
            _tables.Remove(typeof (T));
        }
    }
}