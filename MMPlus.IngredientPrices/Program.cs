using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MMPlus.Shared.EsoSavedVariables;
using MMPlus.Shared.Model;

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

            string outputFileName = args[0];
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


            string savedVarPathTemplate = Path.Combine(inputDirectory, "MM{0:D2}Data.lua");
            var ingredients = new Dictionary<string, EsoItemSalesData>();
            using (FileStream stream = File.OpenRead("provisioning-ingredients.csv"))
            {
                using (var reader = new StreamReader(stream))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] lineParts = line.Split(',');
                        string ingredientName = GetStringValue(lineParts[0]);
                        string ingredientLink = GetStringValue(lineParts[1]);
                        string[] itemLinkParts = ingredientLink.Split(':');
                        if (itemLinkParts.Length < 3)
                        {
                            continue;
                        }
                        string itemBaseId = itemLinkParts[2];
                        ingredients.Add(itemBaseId, new EsoItemSalesData {BaseId = itemBaseId, Name = ingredientName});
                    }
                }
            }

            int earliestSaleTimestamp = -1;
            int latestSaleTimestamp = -1;
            for (int i = 0; i < 16; i++)
            {
                string inFile = string.Format(savedVarPathTemplate, i);
                Console.WriteLine("Reading sales from {0}...", inFile);
                var reader = new MMSavedVariableReader(inFile);
                reader.ProcessEsoGuildStoreSales(
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
                        string[] itemLinkParts = sale.ItemLink.Split(':');
                        if (itemLinkParts.Length < 3)
                        {
                            return;
                        }
                        string itemBaseId = itemLinkParts[2];

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
            Console.WriteLine("Writing prices to {0}...", outputFileName);
            float totalDays = (latestSaleTimestamp - (float) earliestSaleTimestamp)/86400;
            using (FileStream stream = File.Open(outputFileName, FileMode.Create))
            {
                using (var writer = new StreamWriter(stream))
                {
                    string csvLine = string.Format("{0},{1},{2},{3}",
                        GetCsvValue("Ingredient"),
                        GetCsvValue("Average Ea. Price"),
                        GetCsvValue("Average Qty. Sold/Day"),
                        GetCsvValue("Average Gold/Hour"));
                    writer.WriteLine(csvLine);
                    foreach (string ingredientItemBaseId in ingredients.Keys)
                    {
                        EsoItemSalesData ingredientData = ingredients[ingredientItemBaseId];
                        if (ingredientData.SalesQuantitySum == 0)
                        {
                            continue;
                        }
                        ingredientData.SalesPriceAverage =
                            Convert.ToInt32(ingredientData.SalesPriceSum/ingredientData.SalesQuantitySum);
                        float salesQuantityPerDayAverage = ingredientData.SalesQuantitySum/totalDays;
                        float goldPerHourAverage = salesQuantityPerDayAverage*ingredientData.SalesPriceAverage/24;
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