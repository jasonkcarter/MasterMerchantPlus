using System;
using System.Globalization;
using Lua;
using Microsoft.WindowsAzure.Storage.Table;

namespace MMPlus.Shared.Model
{
    /// <summary>
    ///     Represents a guild store sale event in Elder Scrolls Online.
    /// </summary>
    public class EsoGuildStoreSale : TableEntity
    {
        /// <summary>
        ///     Backing field for the TimestampInt property.
        /// </summary>
        private int _timestampInt;

        /// <summary>
        ///     Gets or sets the unique account id of the player that purchased the item.
        /// </summary>
        public string Buyer { get; set; }

        /// <summary>
        ///     Gets or sets the name of the guild who's store the item was sold through.
        /// </summary>
        public string Guild { get; set; }

        /// <summary>
        ///     Gets or sets the unique identifier for this item's base, without any special traits or qualities.
        /// </summary>
        public string ItemBaseId { get; set; }

        /// <summary>
        ///     Gets or sets the path to the item's icon file.
        /// </summary>
        public string ItemIcon { get; set; }

        /// <summary>
        ///     Gets or sets a unique index to identify the specific variety of item purchased, which incorporates required
        ///     level/veteran
        ///     rank, quality, traits, and extra effects numbers.
        /// </summary>
        public string ItemIndex { get; set; }

        /// <summary>
        ///     Gets or sets the non-veteran level a player is required to be at in order to use the item purchased.
        /// </summary>
        public int ItemLevel { get; set; }

        /// <summary>
        ///     Gets or sets the unique ID that ESO uses to identify the item purchased.
        /// </summary>
        public string ItemLink { get; set; }

        /// <summary>
        ///     Gets or sets a number representing any extra effects the item purchased provides, like improved stamina regen or
        ///     restored health.  Just for potions.
        /// </summary>
        public int ItemPotionEffects { get; set; }

        /// <summary>
        ///     Gets or sets the item quality, which influences how effective it is.
        /// </summary>
        public EsoItemQuality ItemQuality { get; set; }

        /// <summary>
        ///     Gets or sets the the special trait supplied by the item purchased, if any.  Just for armor, weapons, and jewelry.
        /// </summary>
        public EsoItemTrait ItemTrait { get; set; }

        /// <summary>
        ///     Gets or sets the veteran rank a player is required to be at in order to use this the item purchased in this sale.
        /// </summary>
        public int ItemVeteranRank { get; set; }

        /// <summary>
        ///     Gets or sets the price paid for the entire stack of items, in gold.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        ///     Gets or sets the number of items in the stack that was purchased.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        ///     Gets or sets the unique account id of the player that listed and sold the item.
        /// </summary>
        public string Seller { get; set; }

        /// <summary>
        ///     Gets or sets the Unix timestamp (i.e. number of seconds since 1970-01-01 00:00:00) that the sale occured at.
        /// </summary>
        public int TimestampInt
        {
            get { return _timestampInt; }
            set
            {
                _timestampInt = value;
                var timestampDateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(value);
                Timestamp = new DateTimeOffset(timestampDateTime);
            }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether the sale occurred at a Guild Trader kiosk.
        /// </summary>
        /// <value>
        ///     <c>true</c> if the item was purchased at a Guild Trader kiosk; otherwise, <c>false</c>.
        /// </value>
        public bool WasKiosk { get; set; }

        /// <summary>
        ///     Extract's the human-readable, culture-specific name of the item sold from the ItemLink property.
        /// </summary>
        /// <returns>The name of the item in the sale, if it exists in the ItemLink property, in title-casing.</returns>
        public string GetItemNameFromLink()
        {
            // No item link exists
            if (string.IsNullOrEmpty(ItemLink)) return null;

            // Separate the link into the details portion and the name portion
            string[] linkParts = ItemLink.Split(new[] {"|h"}, StringSplitOptions.None);

            // The name portion is the second part
            if (linkParts.Length < 2) return null;
            string name = linkParts[1];

            // ESO sometimes returns item names ending in ^p or ^n. Remove that part.
            int carrotIndex = name.IndexOf('^');
            if (carrotIndex > -1)
            {
                name = name.Substring(0, carrotIndex);
            }

            // Normalize the name into the local culture's title casing
            TextInfo cultureTextInfo = CultureInfo.CurrentUICulture.TextInfo;
            name = cultureTextInfo.ToTitleCase(name);

            return name;
        }

        /// <summary>
        ///     Sets all item-related properties with values contained in a given item instance.
        /// </summary>
        /// <param name="item">The item containing the property data to copy.</param>
        public void Set(EsoItem item)
        {
            ItemBaseId = item.BaseId;
            ItemIndex = item.ItemIndex;
            ItemIcon = item.ItemIcon;
            ItemLevel = item.Level;
            ItemPotionEffects = item.PotionEffects;
            ItemQuality = item.Quality;
            ItemTrait = item.Trait;
            ItemVeteranRank = item.VeteranRank;
        }

        /// <summary>
        ///     Loads the value from a given Lua table field into it's corresponding property, if one exists.
        /// </summary>
        /// <param name="field">The Lua table field containing the property and value to set.</param>
        public void Set(LuaTableField field)
        {
            if (field == null) return;
            switch (field.Name)
            {
                case "buyer":
                    Buyer = field.Value;
                    break;
                case "guild":
                    Guild = field.Value;
                    break;
                case "seller":
                    Seller = field.Value;
                    break;
                case "itemLink":
                    ItemLink = field.Value;
                    break;
                case "price":
                    decimal price;
                    if (decimal.TryParse(field.Value, out price))
                    {
                        Price = price;
                    }
                    break;
                case "wasKiosk":
                    bool wasKiosk;
                    if (bool.TryParse(field.Value, out wasKiosk))
                    {
                        WasKiosk = wasKiosk;
                    }
                    break;
                case "quant":
                    int quantity;
                    if (int.TryParse(field.Value, out quantity))
                    {
                        Quantity = quantity;
                    }
                    break;
                case "timestamp":
                    int timestamp;
                    if (int.TryParse(field.Value, out timestamp))
                    {
                        TimestampInt = timestamp;
                    }
                    break;
            }
        }
    }
}