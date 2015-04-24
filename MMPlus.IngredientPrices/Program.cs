using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Lua.EsoSavedVariables;
using MMPlus.Client.Model;

namespace MMPlus.IngredientPrices
{
    internal class Program
    {
        private static string GetCsvValue(string value)
        {
            if (value == null)
            {
                value = string.Empty;
            }
            if (value.Contains('"'))
            {
                value = value.Replace("\"", "\\\"");
            }
            return string.Format("\"{0}\"", value);
        }

        private static string GetStringValue(string value)
        {
            if (value == null)
            {
                return null;
            }
            if (value.Length < 2
                || value.First() != '"'
                || value.Last() != '"')
            {
                return value;
            }
            return value.Substring(1, value.Length - 2);
        }

        private static void Main(string[] args)
        {
            if (args == null || args.Length < 1 || args.Length > 2)
            {
                throw new Exception(
                    "Usage: MMPlus.IngredientPrices.exe <outputFileName> <path_to_saved_variables_folder>");
            }

            var outputFileName = args[0];
            string inputDirectory;
            if (args.Length < 2)
            {
                inputDirectory = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                    "Elder Scrolls Online",
                    "live",
                    "SavedVariables");
            }
            else
            {
                inputDirectory = args[1];
            }


            var savedVarPathTemplate = Path.Combine(inputDirectory, "MM{0:D2}Data.lua");
            var ingredients = new Dictionary<string, EsoItemSalesData>();
            using (var stream = File.OpenRead("provisioning-ingredients.csv"))
            {
                using (var reader = new StreamReader(stream))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        var lineParts = line.Split(',');
                        var ingredientName = GetStringValue(lineParts[0]);
                        var ingredientLink = GetStringValue(lineParts[1]);
                        var itemLinkParts = ingredientLink.Split(':');
                        if (itemLinkParts.Length < 3)
                        {
                            continue;
                        }
                        var itemBaseId = itemLinkParts[2];
                        ingredients.Add(itemBaseId, new EsoItemSalesData {BaseId = itemBaseId, Name = ingredientName});
                    }
                }
            }

            var earliestSaleTimestamp = -1;
            var latestSaleTimestamp = -1;
            for (var i = 0; i < 16; i++)
            {
                var inFile = string.Format(savedVarPathTemplate, i);
                Console.WriteLine("Reading sales from {0}...", inFile);
                var reader = new MMSavedVariableReader();
                using (var stream = File.OpenRead(inFile))
                {
                    reader.ProcessEsoGuildStoreSales(
                        stream,
                        sale =>
                        {
                            if (sale.SaleTimestamp > latestSaleTimestamp)
                            {
                                latestSaleTimestamp = sale.SaleTimestamp;
                            }
                            if (sale.SaleTimestamp < earliestSaleTimestamp)
                            {
                                earliestSaleTimestamp = sale.SaleTimestamp;
                            }
                            EsoItemSalesData ingredientData;

                            // Remove the name portion of the item link
                            var itemLinkParts = sale.ItemLink.Split(':');
                            if (itemLinkParts.Length < 3)
                            {
                                return;
                            }
                            var itemBaseId = itemLinkParts[2];

                            // Merge data for both carrot types
                            if (itemBaseId == "34324")
                            {
                                itemBaseId = "28600";
                            }

                            // Try to find an ingredient with the same link
                            if (ingredients.TryGetValue(itemBaseId, out ingredientData))
                            {
                                // If one is found, update its price data
                                ingredientData.SalesPriceSum += sale.Price;
                                ingredientData.SalesQuantitySum += sale.Quantity;
                            }
                        });
                }
            }
            Console.WriteLine("Writing prices to {0}...", outputFileName);
            var totalDays = (latestSaleTimestamp - (float) earliestSaleTimestamp)/86400;
            using (var stream = File.Open(outputFileName, FileMode.Create))
            {
                using (var writer = new StreamWriter(stream))
                {
                    var csvLine = string.Format("{0},{1},{2},{3}",
                        GetCsvValue("Ingredient"),
                        GetCsvValue("Average Ea. Price"),
                        GetCsvValue("Average Qty. Sold/Day"),
                        GetCsvValue("Average Gold/Hour"));
                    writer.WriteLine(csvLine);
                    foreach (var ingredientItemBaseId in ingredients.Keys)
                    {
                        var ingredientData = ingredients[ingredientItemBaseId];
                        if (ingredientData.SalesQuantitySum == 0)
                        {
                            continue;
                        }
                        ingredientData.SalesPriceAverage =
                            Convert.ToInt32(ingredientData.SalesPriceSum/ingredientData.SalesQuantitySum);
                        var salesQuantityPerDayAverage = ingredientData.SalesQuantitySum/totalDays;
                        var goldPerHourAverage = salesQuantityPerDayAverage*ingredientData.SalesPriceAverage/24;
                        csvLine = string.Format("{0},{1},{2},{3}",
                            GetCsvValue(ingredientData.Name),
                            GetCsvValue(ingredientData.SalesPriceAverage.ToString("F2")),
                            GetCsvValue(salesQuantityPerDayAverage.ToString("F2")),
                            GetCsvValue(goldPerHourAverage.ToString("F2")));
                        writer.WriteLine(csvLine);
                    }
                }
            }
        }
    }
}