using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMPlus.Service.Data;
using MMPlus.Service.Model;

namespace MMPlus.Test
{
    [TestClass]
    public class TableStorageRepositoryTests
    {
        private const string ConnectionString = "UseDevelopmentStorage=true;";

        private readonly string[] _guilds =
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

        private readonly string[] _items =
        {
            "45109;1:0:1:13:0;|H0:item:45109:30:1:0:0:0:0:0:0:0:0:0:0:0:0:7:0:0:0:10000:0|hhomespun sash^p|h",
            "47662;36:0:1:0:0;|H0:item:47662:20:36:0:0:0:0:0:0:0:0:0:0:0:0:3:0:0:0:10000:0|hWhitestrake's Girdle|h"
        };

        [TestMethod]
        public void TableStorageRepository_FindByPartition()
        {
            var tablePrefix = "TEST" + Guid.NewGuid().ToString("N");
            var data = new TableStorageRepository(ConnectionString, tablePrefix);
            var tableName = data.GetTable<EsoSale>().Name;
            var salesByPartition = new Dictionary<int, List<EsoSale>>();
            const int saleCount = 100;

            try
            {
                for (var i = 0; i < saleCount; i++)
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

                foreach (var timestampId in salesByPartition.Keys)
                {
                    // Arrange
                    var expectedSales = salesByPartition[timestampId];

                    // Act
                    var partitionSales =
                        data.Find<EsoSale>(timestampId.ToString(CultureInfo.InvariantCulture)).ToArray();

                    // Assert
                    var missingSale = partitionSales.FirstOrDefault(x => expectedSales.All(y => y.RowKey != x.RowKey));
                    if (missingSale != null)
                    {
                        Assert.Fail("Table {0} Partition {1} missing expected entity with row key {2}", tableName,
                            missingSale.PartitionKey,
                            missingSale.RowKey);
                    }
                    var extraSale = expectedSales.FirstOrDefault(x => partitionSales.All(y => y.RowKey != x.RowKey));
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
            var randomGuild = _guilds[new Random().Next(_guilds.Length)];
            var randomQuantity = new Random().Next(100) + 1;
            var randomPrice = new Random().Next(10000) + 1;
            var randomTimestamp = new Random().Next(10) + 1425340800;
            var randomItemParts = _items[new Random().Next(_items.Length)].Split(';');
            var itemBaseId = randomItemParts[0];
            var itemIndex = randomItemParts[1];
            var itemLink = randomItemParts[2];
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