namespace Lua
{
    /// <summary>
    ///     Represents an entry in a Lua table.
    /// </summary>
    public class LuaTableField
    {
        /// <summary>
        ///     Gets or sets a value indicating whether this field's value is a nested table.
        /// </summary>
        /// <value>
        ///     <c>true</c> if the value for this field is a nested Lua table; otherwise, <c>false</c>.
        /// </value>
        public bool IsTable { get; set; }

        /// <summary>
        ///     Gets or sets the field name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets the value of the field, represented as a string. If IsTable is <c>True</c>, this will be <c>null</c>.
        /// </summary>
        public string Value { get; set; }
    }
}