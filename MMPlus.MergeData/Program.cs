using System;
using System.Collections.Generic;
using System.IO;
using MMPlus.Shared.EsoSavedVariables;
using MMPlus.Shared.Model;

namespace MMPlus.MergeData
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (args == null || args.Length != 5)
            {
                throw new Exception(
                    "Usage: MMPlus.MergeData.exe <accountName> <inputFileName1> <inputFileName2> <outputVariableName> <outputFileName>");
            }

            string accountName = args[0];
            string inputFileName1 = args[1];
            string inputFileName2 = args[2];
            string outputVariableName = args[3];
            string outputFileName = args[4];


            var sortedSales = new SortedSet<EsoSale>(new MMFileSaleComparer());
            Console.WriteLine("Reading sales from {0}...", inputFileName1);
            var reader = new MMSavedVariableReader(inputFileName1);
            reader.ProcessEsoGuildStoreSales(sale => sortedSales.Add(sale));
            Console.WriteLine("Reading sales from {0}...", inputFileName2);
            reader = new MMSavedVariableReader(inputFileName2);
            reader.ProcessEsoGuildStoreSales(sale =>
            {
                if (!sortedSales.Contains(sale))
                {
                    sortedSales.Add(sale);
                }
            });
            Console.WriteLine("Writing combined sales to {0} for account {1}...", outputFileName, accountName);
            using (FileStream stream = File.Open(outputFileName, FileMode.Create))
            {
                using (var writer = new MMSavedVariableWriter(stream))
                {
                    writer.Write(outputVariableName, accountName, sortedSales);
                }
            }
        }
    }
}