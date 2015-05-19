using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMPlus.Service.Model;
using MMPlus.Service.Services;

namespace MMPlus.Test.Service
{
    [TestClass]
    public class GuildServiceTests
    {
        [TestMethod]
        public void GuildService_Get()
        {
            // Arrange
            var repository = new MemoryStorageRepository();
            MemoryStorageTable<EsoGuild> table = repository.GetTable<EsoGuild>();
            var guildNames = new[]
            {
                "Iron Bank of Braavos",
                "Ethereal Traders Union"
            };
            var expectedGuildNames = new[] {guildNames[0]};
            foreach (string guildName in guildNames)
            {
                table.InsertOrReplace(new EsoGuild {Name = guildName, AllConfirmedThresholdTimestampId = 20000});
            }

            // Act
            var service = new GuildService(repository);
            string[] results = service.Get(expectedGuildNames).Select(x => x.Name).ToArray();

            // Assert
            string[] unmatchedGuildNames = expectedGuildNames.Except(results).ToArray();
            if (unmatchedGuildNames.Any())
            {
                Assert.Fail("Missing the following expected guild names: {0}", string.Join(", ", unmatchedGuildNames));
            }
            string[] unexpectedGuildNames = results.Except(expectedGuildNames).ToArray();
            if (unexpectedGuildNames.Any())
            {
                Assert.Fail("Found the following unexpected guild names: {0}", string.Join(", ", unexpectedGuildNames));
            }
        }
    }
}