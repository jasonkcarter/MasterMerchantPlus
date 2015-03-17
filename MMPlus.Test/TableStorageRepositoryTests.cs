using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMPlus.Shared;
using MMPlus.Shared.Data;
using MMPlus.Shared.Model;

namespace MMPlus.Test
{
    [TestClass]
    public class TableStorageRepositoryTests
    {
        private const string ConnectionString = "UseDevelopmentStorage=true;";


        [TestMethod]
        public void TableStorageRepository_FindByPartition()
        {
            string tablePrefix = "TEST" + Guid.NewGuid().ToString("N");
            var data = new TableStorageRepository(ConnectionString, tablePrefix);
            string tableName = data.GetTable<EsoSale>().Name;
            var salesByPartition = new Dictionary<int, List<EsoSale>>();
            const int saleCount = 100;

            try
            {
                for (int i = 0; i < saleCount; i++)
                {
                    // Arrange
                    var sale = Utility.CreateRandomSale();
                    data.InsertOrReplace(sale);
                    List<EsoSale> partition;
                    if (!salesByPartition.TryGetValue(sale.TimestampId, out partition))
                    {
                        partition = new List<EsoSale>();
                        salesByPartition.Add(sale.TimestampId, partition);
                    }

                    // Act
                    partition.Add(sale);
                }

                foreach (int timestampId in salesByPartition.Keys)
                {
                    // Arrange
                    List<EsoSale> expectedSales = salesByPartition[timestampId];

                    // Act
                    EsoSale[] partitionSales =
                        data.Find<EsoSale>(timestampId.ToString(CultureInfo.InvariantCulture)).ToArray();

                    // Assert
                    EsoSale missingSale = partitionSales.FirstOrDefault(x => expectedSales.All(y => y.RowKey != x.RowKey));
                    if (missingSale != null)
                    {
                        Assert.Fail("Table {0} Partition {1} missing expected entity with row key {2}", tableName,
                            missingSale.PartitionKey,
                            missingSale.RowKey);
                    }
                    EsoSale extraSale = expectedSales.FirstOrDefault(x => partitionSales.All(y => y.RowKey != x.RowKey));
                    if (extraSale != null)
                    {
                        Assert.Fail("Table {0} Partition {1} contains unexpected entity with row key {2}", tableName,
                            extraSale.PartitionKey,
                            extraSale.RowKey);
                    }
                }
            }
            finally
            {
                // Clean up
                data.RemoveTable<EsoSale>();
            }
        }
    }
}