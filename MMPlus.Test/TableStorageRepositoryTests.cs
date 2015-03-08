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

        private string[] _items =
        {
            "45109;1:0:1:13:0;|H0:item:45109:30:1:0:0:0:0:0:0:0:0:0:0:0:0:7:0:0:0:10000:0|hhomespun sash^p|h",
            "47662;36:0:1:0:0;|H0:item:47662:20:36:0:0:0:0:0:0:0:0:0:0:0:0:3:0:0:0:10000:0|hWhitestrake's Girdle|h"
        };

        private string[] _guilds =
        {
            "Akaveri Imports",
            "Bal-mart",
            "Bleakrock Barter Co",
            "Craftoholics",
            "Dominant Dominion",
            "Dominion Imperial Guard",
            "East Empire Company",
            "Elder Scrolls Exchange",
            "Ethereal Traders Union",
            "Ethereal Traders Union II",
            "Fangs of Ironglaive",
            "Gold Dragons",
            "Iron Bank of Bravos"
        };

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
                    var sale = CreateRandomSale();
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

        private EsoSale CreateRandomSale()
        {
            string randomGuild = _guilds[new Random().Next(_guilds.Length)];
            int randomQuantity = new Random().Next(100) + 1;
            int randomPrice = new Random().Next(10000) + 1;
            int randomTimestamp = new Random().Next(10) + 1425340800;
            string[] randomItemParts = _items[new Random().Next(_items.Length)].Split(';');
            string itemBaseId = randomItemParts[0];
            string itemIndex = randomItemParts[1];
            string itemLink = randomItemParts[2];
            var sale = new EsoSale(false)
            {
                GuildName = randomGuild,
                Buyer = "@" + Guid.NewGuid().ToString("N"),
                Seller = "@" + Guid.NewGuid().ToString("N"),
                ItemBaseId = itemBaseId,
                ItemIndex = itemIndex,
                ItemLink = itemLink,
                Quantity = randomQuantity,
                Price = randomPrice,
                SaleTimestamp = randomTimestamp
            };
            sale.GenerateRowKey();
            return sale;
        }
    }
}