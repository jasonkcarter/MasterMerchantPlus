namespace MMPlus.Service.Data
{
    /// <summary>
    ///     Supports serializing and deserializing objects to/from semicolon-delimited format.
    /// </summary>
    public interface ISemiDelimited
    {
        /// <summary>
        ///     Populates the properties of this instance with data from a given semicolon-delimited string.
        /// </summary>
        /// <param name="data">The line of tab-delimited string data to load the properties of this instance with.</param>
        void LoadFromSemiDelimited(string data);

        /// <summary>
        ///     Gets a semicolon-delimited string containing the data from this instance's properties.
        /// </summary>
        string ToSemiDelimited();
    }
}