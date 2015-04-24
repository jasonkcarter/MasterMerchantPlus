using MMPlus.Shared.Interface;

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

        /// <summary>
        ///     Loads the value from this table field into it's corresponding property in a given Elder Scrolls Online item model,
        ///     if the property exists.
        /// </summary>
        /// <param name="item">Item to load with this field's value.</param>
        public void SetEsoItem(IEsoItem item)
        {
            if (item == null) return;
            switch (Name)
            {
                case "itemIcon":
                    item.ItemIcon = Value;
                    break;
            }
        }

        /// <summary>
        ///     Loads the value from this table field into it's corresponding property in a given Elder Scrolls Online sales model,
        ///     if the property exists.
        /// </summary>
        /// <param name="sale">Sale to load with this field's value.</param>
        public void SetEsoSale(IEsoSale sale)
        {
            if (sale == null) return;
            switch (Name)
            {
                case "buyer":
                    sale.Buyer = Value;
                    break;
                case "guild":
                    sale.GuildName = Value;
                    break;
                case "seller":
                    sale.Seller = Value;
                    break;
                case "itemLink":
                    sale.ItemLink = Value;
                    break;
                case "price":
                    int price;
                    if (int.TryParse(Value, out price))
                    {
                        sale.Price = price;
                    }
                    break;
                case "wasKiosk":
                    bool wasKiosk;
                    if (bool.TryParse(Value, out wasKiosk))
                    {
                        sale.WasKiosk = wasKiosk;
                    }
                    break;
                case "quant":
                    int quantity;
                    if (int.TryParse(Value, out quantity))
                    {
                        sale.Quantity = quantity;
                    }
                    break;
                case "timestamp":
                    int timestamp;
                    if (int.TryParse(Value, out timestamp))
                    {
                        sale.SaleTimestamp = timestamp;
                    }
                    break;
            }
        }
    }
}