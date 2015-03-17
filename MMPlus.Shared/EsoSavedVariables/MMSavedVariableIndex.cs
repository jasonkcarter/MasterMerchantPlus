using System;
using System.Collections.Generic;
using System.IO;

namespace MMPlus.Shared.EsoSavedVariables
{
    
    // ReSharper disable once InconsistentNaming
    /// <summary>
    /// Used to track the file byte offset positions of all sales within a Master Merchant mule data file.
    /// </summary>
    public class MMSavedVariableIndex : List<MMSavedVariableIndex.Entry>
    {
        /// <summary>
        /// Gets or sets the date and time that the saved variable file this index pertains to was last modified.  Used to determine if the index is stale or not.
        /// </summary>
        public DateTimeOffset? SavedVariablesLastModified { get; set; }

        /// <summary>
        /// Loads the index data from a given file path.
        /// </summary>
        /// <param name="filePath">The path to the file containing index data.</param>
        public void Load(string filePath)
        {
            using (FileStream stream = File.OpenRead(filePath))
            {
                Load(stream);
            }
        }

        /// <summary>
        /// Loads the index data from a given stream of data.
        /// </summary>
        /// <param name="stream">The stream containing index data.</param>
        public void Load(Stream stream)
        {
            // Clear any existing data in this index instance
            Clear();

            // Open the stream for reading
            using (var reader = new StreamReader(stream))
            {
                // Read the first line.
                string line = reader.ReadLine();
                if (line == null)
                {
                    return;
                }
                // The first line contains the last modified date/time of the files the index data pertains to.
                SavedVariablesLastModified = DateTime.Parse(line);

                // Continue reading lines 
                while (true)
                {
                    line = reader.ReadLine();
                    if (line == null)
                    {
                        break;
                    }

                    // Each entry in the index consists of 4 comma-delimited fields
                    string[] lineParts = line.Split(',');

                    // Validate that there are 4 fields, and that the last two are long integers.
                    long length;
                    long saleCount;
                    if (lineParts.Length != 4
                        || !long.TryParse(lineParts[2], out length)
                        || !long.TryParse(lineParts[3], out saleCount))
                    {
                        continue;
                    }

                    // Each entry consists of the item base id, item variation index, 
                    // the byte offset of the item sale data in the data file, and the 
                    // length of the sale data in the file.
                    var entry = new Entry(lineParts[0], lineParts[1], saleCount, length);

                    // Add the entry to this instance
                    Add(entry);
                }
            }
        }

        public void Save(string filePath)
        {
            using (FileStream stream = File.OpenWrite(filePath))
            {
                Save(stream);
            }
        }

        public void Save(Stream stream)
        {
            if (SavedVariablesLastModified == null)
            {
                throw new InvalidOperationException("SavedVariablesLastModified cannot be null during a save operation.");
            }
            using (var writer = new StreamWriter(stream))
            {
                writer.WriteLine(SavedVariablesLastModified.Value.ToString("O"));
                foreach (Entry entry in this)
                {
                    writer.WriteLine("{0},{1},{2},{3}", entry.ItemBaseId, entry.ItemIndex, entry.Length, entry.SaleCount);
                }
            }
        }

        public class Entry
        {
            public string ItemBaseId { get; set; }
            public string ItemIndex { get; set; }
            public long Length { get; set; }
            public long SaleCount { get; set; }

            public Entry(string itemBaseId, string itemIndex, long length, long saleCount)
            {
                ItemBaseId = itemBaseId;
                ItemIndex = itemIndex;
                Length = length;
                SaleCount = saleCount;
            }
        }
    }
}