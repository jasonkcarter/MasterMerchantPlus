namespace Lua.EsoSavedVariables
{
    /// <summary>
    ///     Used to filter out guild store sales by name and/or date/time.
    /// </summary>
    public class EsoSaleFilter
    {
        /// <summary>
        ///     Gets or sets the name of the guild that the filter applies to.
        /// </summary>
        public string GuildName { get; set; }

        /// <summary>
        ///     Gets or sets the Unix timestamp (i.e. seconds since 1970-01-01 00:00:00 UTC) that sales for the given Guild should
        ///     be older than.
        /// </summary>
        public int? TimestampMaximum { get; set; }

        /// <summary>
        ///     Gets or sets the Unix timestamp (i.e. seconds since 1970-01-01 00:00:00 UTC) that sales for the given Guild should
        ///     be newer than.
        /// </summary>
        public int? TimestampMinimum { get; set; }
    }
}