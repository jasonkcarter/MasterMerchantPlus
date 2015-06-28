using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMPlus.Service.Model;
using MMPlus.Service.Services;
using MMPlus.Test.Properties;

namespace MMPlus.Test.Service
{
    [TestClass]
    public class SyncServiceTests
    {
        [TestMethod]
        public void SyncService_Put()
        {
            // Arrange
            var repository = new MemoryStorageRepository();
            MemoryStorageTable<EsoSale> salesTable = repository.GetTable<EsoSale>();
            {
                IEnumerable<EsoSale> existingSales =
                    Resources.SyncService_Put_Arrange.Split(new[] {"\r\n"}, StringSplitOptions.None)
                        .Select(EsoSale.CreateFromSemiDelimited).ToArray();

                foreach (EsoSale existingSale in existingSales)
                {
                    salesTable.InsertOrReplace(existingSale);
                }
            }


            // Act
            var service = new SyncService(repository);
            List<EsoSale> newSales =
                Resources.SyncService_Put_Act.Split(new[] {"\r\n"}, StringSplitOptions.None)
                    .Select(EsoSale.CreateFromSemiDelimited).ToList();
            service.Put("MachineId1", newSales);

            // Assert
            List<EsoSale> expectedSales =
                Resources.SyncService_Put_Act.Split(new[] {"\r\n"}, StringSplitOptions.None)
                    .Select(EsoSale.CreateFromSemiDelimited).ToList();
            foreach (EsoSale expected in expectedSales)
            {
                List<EsoSale> actualList =
                    repository.Find<EsoSale>(expected.PartitionKey, expected.RowKey).ToList();
                Assert.IsNotNull(actualList);
                Assert.AreEqual(1, actualList.Count());
                EsoSale actual = actualList.First();
                if (!expected.Equals(actual))
                {
                    Assert.Fail(
                        "The data for sale with partition key {0} and row key {1} do not match expected values.\r\nExpected: {2}\r\nActual {3}",
                        expected.PartitionKey, expected.RowKey, expected.ToSemiDelimited(), actual.ToSemiDelimited());
                }
            }
            EsoSale unexpectedSale = newSales.Except(expectedSales).FirstOrDefault();
            if (unexpectedSale != null)
            {
                Assert.Fail("Unexpected sale detected: {0}", unexpectedSale.ToSemiDelimited());
            }
        }
    }
}