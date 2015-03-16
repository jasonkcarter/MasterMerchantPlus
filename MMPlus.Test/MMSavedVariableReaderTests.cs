﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMPlus.Shared.EsoSavedVariables;
using MMPlus.Shared.Model;

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
        ///     Validate that the synchronous GetEsoSales() method returns the expected number of sales when run on
        ///     TestFile00 with a timestamp filter.
        /// </summary>
        [TestMethod]
        public void MMSavedVariableReader_GetEsoGuildStoreSales_GuildTimestampFilter()
        {
            for (int i = 0; i < 16; i++)
            {
                string testFileName = string.Format(TestFilePathFormat, i);
                // Arrange
                var reader = new MMSavedVariableReader(testFileName);
                // Choose a recent enough timestamp that the prefix will have no records with prefixes higher than it.
                // This allows us to do a straight up grep search by timestamp prefix to get the expected count.
                var filter = new EsoSaleFilter
                {
                    GuildName = "Ethereal Traders Union",
                    TimestampMinimum = 1424900000
                };
                reader.Filters = new[] {filter};
                string timestampSubstring = string.Format("[\"timestamp\"] = {0}", filter.TimestampMinimum/100000);
                string guildSubstring = string.Format("[\"guild\"] = \"{0}\"", filter.GuildName);
                int expectedSaleCount = CountLuaTablesWithLines(testFileName, timestampSubstring, guildSubstring);

                // Act
                List<EsoSale> sales = reader.GetEsoSales();

                // Assert
                if (expectedSaleCount != sales.Count)
                {
                    Assert.Fail("Expected {0} sales in {1}, but found {2}", expectedSaleCount, testFileName, sales.Count);
                }
            }
        }

        /// <summary>
        ///     Validate that the synchronous GetEsoSales() method returns the expected number of sales when run on
        ///     TestFile00 with no filters.
        /// </summary>
        [TestMethod]
        public void MMSavedVariableReader_GetEsoGuildStoreSales_NoFilter()
        {
            for (int i = 0; i < 16; i++)
            {
                string testFileName = string.Format(TestFilePathFormat, i);
                // Arrange
                var reader = new MMSavedVariableReader(testFileName);
                int expectedSaleCount = CountLuaTablesWithLines(testFileName);

                // Act
                List<EsoSale> sales = reader.GetEsoSales();

                // Assert
                if (expectedSaleCount != sales.Count)
                {
                    Assert.Fail("Expected {0} sales in {1}, but found {2}", expectedSaleCount, testFileName, sales.Count);
                }
            }
        }

        /// <summary>
        ///     Validate that the synchronous GetEsoSales() method returns the expected number of sales when run on
        ///     TestFile00 with a timestamp filter.
        /// </summary>
        [TestMethod]
        public void MMSavedVariableReader_GetEsoGuildStoreSales_TimestampFilter()
        {
            for (int i = 0; i < 16; i++)
            {
                string testFileName = string.Format(TestFilePathFormat, i);
                // Arrange
                var reader = new MMSavedVariableReader(testFileName);
                // Choose a recent enough timestamp that the prefix will have no records with prefixes higher than it.
                // This allows us to do a straight up grep search by timestamp prefix to get the expected count.
                var filter = new EsoSaleFilter
                {
                    TimestampMinimum = 1424900000
                };
                reader.Filters = new[] {filter};
                string searchString = string.Format("[\"timestamp\"] = {0}", filter.TimestampMinimum/100000);
                int expectedSaleCount = CountLuaTablesWithLines(testFileName, searchString);

                // Act
                List<EsoSale> sales = reader.GetEsoSales();

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
            int count = 0;
            int tableStartIndex = -1;
            int tableEndIndex = -1;
            using (FileStream stream = File.OpenRead(filePath))
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
                            int filterMatchCount = linesToSearch.Count(x => substrings.Any(x.Contains));
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