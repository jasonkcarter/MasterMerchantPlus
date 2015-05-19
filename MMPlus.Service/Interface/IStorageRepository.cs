using System;
using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage.Table;

namespace MMPlus.Service.Interface
{
    /// <summary>
    ///     A repository provider for Azure Table Storage backends.
    /// </summary>
    public interface IStorageRepository
    {
        /// <summary>
        ///     Removes the entity from the repository.
        /// </summary>
        /// <param name="entity">The entity to delete from the repository.</param>
        bool Delete<T>(T entity) where T : TableEntity, new();

        /// <summary>
        ///     Finds all entities in the repository with non-default properties similar to the given entity.
        /// </summary>
        /// <param name="partitionKey">The partition key to limit results by.</param>
        /// <param name="rowKey">The row key to limit results by.</param>
        /// <param name="timestampMin">The minimum timestamp for entities to include</param>
        /// <param name="timestampMax">The maximum timestamp for entities to include.</param>
        /// <returns>A list of matching entities.</returns>
        IEnumerable<T> Find<T>(string partitionKey, string rowKey = null, DateTimeOffset? timestampMin = null,
            DateTimeOffset? timestampMax = null) where T : TableEntity, new();


        /// <summary>
        ///     Replaces an existing entity or inserts a new entity if it does not exist in the repository. Because this operation
        ///     can insert or update an entity, it is also known as an upsert operation.
        /// </summary>
        /// <param name="entity">The entity to insert or replace in the repository.</param>
        bool InsertOrReplace<T>(T entity) where T : TableEntity, new();

        /// <summary>
        ///     Completely removes the table and all data for a given entity type from Azure Table Storage. Used for cleaning up
        ///     after unit tests.
        /// </summary>
        /// <typeparam name="T">The entity type to remove the table for.</typeparam>
        void RemoveTable<T>() where T : TableEntity, new();
    }
}