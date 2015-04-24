using System;
using System.Globalization;
using Humanizer;
using MMPlus.Shared.Interface;
using MMPlus.Shared.Model;

namespace MMPlus.Client.Model
{
    /// <summary>
    ///     Represents a guild store sale event in Elder Scrolls Online.
    /// </summary>
    public class EsoSale : IEsoSale
    {
        /// <summary>
        ///     Backing field for the ItemIndex property.
        /// </summary>
        private string _itemIndex;

        /// <summary>
        ///     Backing field for the SaleTimestamp property.
        /// </summary>
        private int _saleTimestamp;

        /// <summary>
        ///     Gets or sets the unique account id of the player that purchased the item.
        /// </summary>
        public string Buyer { get; set; }

        /// <summary>
        ///     Gets or sets the name of the guild who's store the item was sold through.
        /// </summary>
        public string GuildName { get; set; }

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
        public string ItemIndex
        {
            get { return _itemIndex; }
            set
            {
                _itemIndex = value;

                // Populate Level, VeteranRank, Quality, Trait, and PotionEffects if this is a valid ItemIndex.
                int level;
                int veteranRank;
                EsoItemQuality quality;
                EsoItemTrait trait;
                int potionEffects;
                var validIndex = EsoItem.TryParseIndex(ItemIndex,
                    out level, out veteranRank, out quality, out trait, out potionEffects);
                if (validIndex)
                {
                    ItemLevel = level;
                    ItemVeteranRank = veteranRank;
                    ItemQuality = quality;
                    ItemTrait = trait;
                    ItemPotionEffects = potionEffects;
                }
            }
        }

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
        public int Price { get; set; }

        /// <summary>
        ///     Gets or sets the number of items in the stack that was purchased.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        ///     Gets or sets the index used to identify the relative order that sales with the same guild, timestampid, buyer,
        ///     seller, item, quantity, and price occured in, when there are multiples of the same sort of sale in rapid
        ///     succession.
        /// </summary>
        public int RelativeOrderIndex { get; set; }

        /// <summary>
        ///     Gets or sets the Unix timestamp (i.e. number of seconds since 1970-01-01 00:00:00) that the sale occured at.
        /// </summary>
        public int SaleTimestamp
        {
            get { return _saleTimestamp; }
            set
            {
                _saleTimestamp = value;
                TimestampId = (int) Math.Round(value/1000F);
            }
        }

        /// <summary>
        ///     Gets or sets the unique account id of the player that listed and sold the item.
        /// </summary>
        public string Seller { get; set; }

        /// <summary>
        ///     Gets or sets the account name of the player that submits the sale for inclusion in the global aggregate data set.
        /// </summary>
        public string Submitter { get; set; }

        /// <summary>
        ///     Gets the value used to group sales that happen around the same time together, since TimestampMinimum and
        ///     SaleTimestamp from
        ///     different
        ///     clients usually don't match.
        /// </summary>
        public int TimestampId { get; private set; }

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
            var linkParts = ItemLink.Split(new[] {"|h"}, StringSplitOptions.None);

            // The name portion is the second part
            if (linkParts.Length < 2) return null;
            var name = linkParts[1];

            // ESO sometimes returns item names ending in ^p or ^n. Remove that part.
            var carrotIndex = name.IndexOf('^');
            if (carrotIndex > -1)
            {
                name = name.Substring(0, carrotIndex);
            }

            // Normalize the name into title casing.
            name = name.Titleize();

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
        }

        public string ToSemiDelimited()
        {
            return string.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};{10};{11}",
                GuildName,
                SaleTimestamp,
                ItemBaseId,
                ItemIndex,
                Quantity,
                Price,
                Buyer,
                Seller,
                RelativeOrderIndex,
                WasKiosk,
                ItemLink,
                Submitter);
        }
    }
}