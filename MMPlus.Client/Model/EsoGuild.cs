using MMPlus.Shared.Interface;

namespace MMPlus.Client.Model
{
    /// <summary>
    ///     Represents a Guild in the Elder Scrolls Online.
    /// </summary>
    public class EsoGuild : IEsoGuild
    {
        /// <summary>
        ///     Gets or sets the timestamp id of the sale before which all sales are confirmed or marked as duplicates.
        /// </summary>
        public int AllConfirmedThresholdTimestampId { get; set; }

        /// <summary>
        ///     Gets or sets the Guild's human-readable name.
        /// </summary>
        public string Name { get; set; }
    }
}