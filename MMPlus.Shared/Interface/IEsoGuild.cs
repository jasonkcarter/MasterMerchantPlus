namespace MMPlus.Shared.Interface
{
    /// <summary>
    ///     Represents a Guild in the Elder Scrolls Online.
    /// </summary>
    public interface IEsoGuild
    {
        /// <summary>
        ///     Gets or sets the timestamp id of the sale before which all sales are confirmed or marked as duplicates.
        /// </summary>
        int AllConfirmedThresholdTimestampId { get; set; }

        /// <summary>
        ///     Gets or sets the Guild's human-readable name.
        /// </summary>
        string Name { get; set; }
    }
}