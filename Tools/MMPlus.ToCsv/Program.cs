using System;
using System.Diagnostics;
using System.IO;
using MMPlus.Shared.EsoSavedVariables;

namespace MMPlus.ToCsv
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (args == null || args.Length < 1 || args.Length > 2)
            {
                throw new Exception("Usage: MMPlus.ToCsv.exe <outputFileName> <path_to_saved_variables_folder>");
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
            using (FileStream outStream = File.Open(outputFileName, FileMode.Create))
            {
                using (var writer = new StreamWriter(outStream))
                {
                    for (int i = 0; i < 16; i++)
                    {
                        string inFile = string.Format(savedVarPathTemplate, i);
                        Console.WriteLine("Reading sales from {0}...", inFile);
                        var reader = new MMSavedVariableReader(inFile);
                        reader.ProcessEsoGuildStoreSales(
                            sale =>
                            {
                                string line = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9}\n",
                                    sale.TimestampId, sale.SaleTimestamp, sale.ItemBaseId, sale.ItemIndex,
                                    sale.GetItemNameFromLink(), sale.GuildName, sale.Quantity, sale.Price, sale.Buyer,
                                    sale.Seller);
                                // ReSharper disable once AccessToDisposedClosure
                                writer.Write(line);
                            });
                    }
                }
            }

            // Create a sorted version of the file
            string directoryName =
                Path.GetDirectoryName(outputFileName) ?? AppDomain.CurrentDomain.BaseDirectory;
            string sortedFileName = Path.GetFileNameWithoutExtension(outputFileName);
            if (sortedFileName == null)
            {
                sortedFileName = "sorted.csv";
            }
            else
            {
                sortedFileName += "-sorted.csv";
            }
            string sortedFilePath = Path.Combine(directoryName, sortedFileName);
            Console.WriteLine("Sorting");
            Process.Start("sort.exe",
                string.Format("--field-separator=, --ignore-case --output=\"{0}\" \"{1}\"", sortedFilePath,
                    outputFileName));
        }
    }
}