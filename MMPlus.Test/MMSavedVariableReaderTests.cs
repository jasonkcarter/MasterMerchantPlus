using System.Collections.Generic;
using System.IO;
using System.Linq;
using Lua.EsoSavedVariables;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMPlus.Client.Model;

namespace MMPlus.Test
{
    /// <summary>
    ///     Unit tests on the MMSavedVariableReader class.
    /// </summary>
    [TestClass]
    [DeploymentItem("MM00Data.lua")]
    [DeploymentItem("MM01Data.lua")]
    [DeploymentItem("MM02Data.lua")]
    [DeploymentItem("MM03Data.lua")]
    [DeploymentItem("MM04Data.lua")]
    [DeploymentItem("MM05Data.lua")]
    [DeploymentItem("MM06Data.lua")]
    [DeploymentItem("MM07Data.lua")]
    [DeploymentItem("MM08Data.lua")]
    [DeploymentItem("MM09Data.lua")]
    [DeploymentItem("MM10Data.lua")]
    [DeploymentItem("MM11Data.lua")]
    [DeploymentItem("MM12Data.lua")]
    [DeploymentItem("MM13Data.lua")]
    [DeploymentItem("MM14Data.lua")]
    [DeploymentItem("MM15Data.lua")]
    // ReSharper disable once InconsistentNaming
    public class MMSavedVariableReaderTests
    {
        /// <summary>
        ///     Name of the zip archive containing all the test Master Merchant saved variable sales data.
        /// </summary>
        public const string SavedVariablesArchive = "SavedVariables.zip";

        /// <summary>
        ///     Name of the file containing test Master Merchant saved variable sales data.
        /// </summary>
        public const string TestFilePathFormat = "MM{0:D2}Data.lua";

        /// <summary>
        ///     Validate that the synchronous GetEsoGuildStoreSales() method returns the expected number of sales when run on
        ///     TestFile00 with a timestamp filter.
        /// </summary>
        [TestMethod]
        public void MMSavedVariableReader_GetEsoGuildStoreSales_GuildTimestampFilter()
        {
            for (var i = 0; i < 16; i++)
            {
                var testFileName = string.Format(TestFilePathFormat, i);
                // Arrange
                var reader = new MMSavedVariableReader();
                // Choose a recent enough timestamp that the prefix will have no records with prefixes higher than it.
                // This allows us to do a straight up grep search by timestamp prefix to get the expected count.
                var filter = new EsoSaleFilter
                {
                    GuildName = "Ethereal Traders Union",
                    TimestampMinimum = 1424900000
                };
                var timestampSubstring = string.Format("[\"timestamp\"] = {0}", filter.TimestampMinimum/100000);
                var guildSubstring = string.Format("[\"guild\"] = \"{0}\"", filter.GuildName);
                var expectedSaleCount = CountLuaTablesWithLines(testFileName, timestampSubstring, guildSubstring);

                // Act
                // TODO: Performance tune. Possibly ditch full Lua parsing. 
                // TODO: Full data file scan is fine to take a long time, but filtered scans should be faster than this currently performs.
                List<EsoSale> sales;
                using (var stream = File.OpenRead(testFileName))
                {
                    sales = reader.GetEsoGuildStoreSales(stream, filter);
                }

                // Assert
                Assert.IsNotNull(sales);
                if (expectedSaleCount != sales.Count)
                {
                    Assert.Fail("Expected {0} sales in {1}, but found {2}", expectedSaleCount, testFileName, sales.Count);
                }
            }
        }

        /// <summary>
        ///     Validate that the synchronous GetEsoGuildStoreSales() method returns the expected number of sales when run on
        ///     TestFile00 with no filters.
        /// </summary>
        [TestMethod]
        public void MMSavedVariableReader_GetEsoGuildStoreSales_NoFilter()
        {
            for (var i = 0; i < 16; i++)
            {
                var testFileName = string.Format(TestFilePathFormat, i);
                // Arrange
                var reader = new MMSavedVariableReader();
                var expectedSaleCount = CountLuaTablesWithLines(testFileName);

                // Act
                List<EsoSale> sales;
                using (var stream = File.OpenRead(testFileName))
                {
                    sales = reader.GetEsoGuildStoreSales(stream);
                }

                // Assert
                Assert.IsNotNull(sales);
                if (expectedSaleCount != sales.Count)
                {
                    Assert.Fail("Expected {0} sales in {1}, but found {2}", expectedSaleCount, testFileName, sales.Count);
                }
            }
        }

        /// <summary>
        ///     Validate that the synchronous GetEsoGuildStoreSales() method returns the expected number of sales when run on
        ///     TestFile00 with a timestamp filter.
        /// </summary>
        [TestMethod]
        public void MMSavedVariableReader_GetEsoGuildStoreSales_TimestampFilter()
        {
            for (var i = 0; i < 16; i++)
            {
                var testFileName = string.Format(TestFilePathFormat, i);
                // Arrange
                var reader = new MMSavedVariableReader();
                // Choose a recent enough timestamp that the prefix will have no records with prefixes higher than it.
                // This allows us to do a straight up grep search by timestamp prefix to get the expected count.
                var filter = new EsoSaleFilter
                {
                    TimestampMinimum = 1424900000
                };
                var searchString = string.Format("[\"timestamp\"] = {0}", filter.TimestampMinimum/100000);
                var expectedSaleCount = CountLuaTablesWithLines(testFileName, searchString);

                // Act
                List<EsoSale> sales;
                using (var stream = File.OpenRead(testFileName))
                {
                    sales = reader.GetEsoGuildStoreSales(stream, filter);
                }

                // Assert
                if (expectedSaleCount != sales.Count)
                {
                    Assert.Fail("Expected {0} sales in {1}, but found {2}", expectedSaleCount, testFileName, sales.Count);
                }
            }
        }

        /// <summary>
        ///     Counts the number of lines containing a particular substring in a given file.
        /// </summary>
        /// <param name="filePath">The fully-qualified file path of the file to search.</param>
        /// <param name="substrings">The substrings to match lines to.</param>
        /// <returns>The number of lines within the file that contain the substring.</returns>
        private static int CountLuaTablesWithLines(string filePath, params string[] substrings)
        {
            var lineBuffer = new List<string>(11);
            var count = 0;
            var tableStartIndex = -1;
            var tableEndIndex = -1;
            using (var stream = File.OpenRead(filePath))
            {
                using (var reader = new StreamReader(stream))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (lineBuffer.Count == 11)
                        {
                            lineBuffer.RemoveAt(0);
                        }
                        lineBuffer.Add(line);

                        if (line.IndexOf('{') > -1)
                        {
                            tableStartIndex = lineBuffer.Count - 1;
                        }
                        else
                        {
                            if (tableStartIndex > -1)
                            {
                                tableStartIndex--;
                            }

                            if (line.IndexOf('}') > -1)
                            {
                                tableEndIndex = lineBuffer.Count - 1;
                            }
                            else if (tableEndIndex > -1)
                            {
                                tableEndIndex--;
                            }
                        }

                        if (tableStartIndex < 0 || (tableEndIndex - 2) < tableStartIndex)
                        {
                            continue;
                        }


                        if (substrings.Length == 0)
                        {
                            count++;
                        }
                        else
                        {
                            var linesToSearch = lineBuffer.GetRange(tableStartIndex + 1,
                                tableEndIndex - tableStartIndex - 2);
                            var filterMatchCount = linesToSearch.Count(x => substrings.Any(x.Contains));
                            if (filterMatchCount == substrings.Length)
                            {
                                count++;
                            }
                        }
                        tableStartIndex = -1;
                        tableEndIndex = -1;
                    }
                }
            }
            return count;
        }
    }
}