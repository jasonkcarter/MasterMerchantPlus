using System.Runtime.Serialization;
using Microsoft.WindowsAzure.Storage.Table;
using MMPlus.Shared.Interface;

namespace MMPlus.Service.Model
{
    /// <summary>
    ///     Represents a Guild in the Elder Scrolls Online.
    /// </summary>
    [DataContract]
    public class EsoGuild : TableEntity, IEsoGuild
    {
        /// <summary>
        ///     Backing field for the Name property.
        /// </summary>
        private string _name;

        /// <summary>
        ///     Initializes a new instance of the <see cref="EsoGuild" /> class.
        /// </summary>
        public EsoGuild()
        {
            PartitionKey = string.Empty;
        }

        /// <summary>
        ///     Gets or sets the timestamp id of the sale before which all sales are confirmed or marked as duplicates.
        /// </summary>
        public int AllConfirmedThresholdTimestampId { get; set; }

        /// <summary>
        ///     Gets or sets the Guild's human-readable name.
        /// </summary>
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;

                // Use the guild name as the primary key.
                RowKey = value;
            }
        }
    }
}