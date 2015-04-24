using System;
using System.IO;
using Lua.EsoSavedVariables;
using MMPlus.Client.Model;
using MMPlus.Shared.Utility;

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

            var accountName = args[0];
            var inputFileName1 = args[1];
            var inputFileName2 = args[2];
            var outputVariableName = args[3];
            var outputFileName = args[4];

            var sortedSales = new SortedCollection<EsoSale>(new MMFileSaleComparer());
            Console.WriteLine("Reading sales from {0}...", inputFileName1);
            var reader = new MMSavedVariableReader();
            using (var stream = File.OpenRead(inputFileName1))
            {
                reader.ProcessEsoGuildStoreSales(stream, sortedSales.Add);
            }
            Console.WriteLine("Reading sales from {0}...", inputFileName2);
            reader = new MMSavedVariableReader();
            using (var stream = File.OpenRead(inputFileName2))
            {
                reader.ProcessEsoGuildStoreSales(
                    stream,
                    sale =>
                    {
                        if (!sortedSales.Contains(sale))
                        {
                            sortedSales.Add(sale);
                        }
                    });
            }
            Console.WriteLine("Writing combined sales to {0} for account {1}...", outputFileName, accountName);
            using (var stream = File.Open(outputFileName, FileMode.Create))
            {
                using (var writer = new MMSavedVariableWriter(stream))
                {
                    writer.Write(outputVariableName, accountName, sortedSales);
                }
            }
        }
    }
}