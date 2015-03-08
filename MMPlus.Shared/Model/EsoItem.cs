using System;
using System.Linq;
using Lua;
using Microsoft.WindowsAzure.Storage.Table;

namespace MMPlus.Shared.Model
{
    /// <summary>
    ///     Represents a specific Elder Scrolls Online item, with all of its special traits, if any.
    /// </summary>
    public class EsoItem : TableEntity
    {
        /// <summary>
        ///     Backing field for the BaseId property.
        /// </summary>
        private string _baseId;

        /// <summary>
        ///     Backing field for the ItemIndex property.
        /// </summary>
        private string _itemIndex;

        /// <summary>
        ///     Gets or sets the unique identifier for this item's base, without any special traits or qualities.
        /// </summary>
        public string BaseId
        {
            get { return _baseId; }
            set
            {
                _baseId = value;

                // Store each base item in its own partition.
                PartitionKey = value;
            }
        }

        /// <summary>
        ///     Gets a value indicating whether this instance is valid.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is valid; otherwise, <c>false</c>.
        /// </value>
        public bool IsValid { get; private set; }

        /// <summary>
        ///     Gets or sets the path to this item's icon file.
        /// </summary>
        public string ItemIcon { get; set; }

        /// <summary>
        ///     Gets or sets a unique index to identify this specific variety of item, which incorporates required level/veteran
        ///     rank, quality, traits, and extra effects numbers.
        /// </summary>
        public string ItemIndex
        {
            get { return _itemIndex; }
            set
            {
                _itemIndex = value;

                // Store each variety for an item in a separate row in the partition.
                RowKey = value;

                // Populate Level, VeteranRank, Quality, Trait, and PotionEffects if this is a valid ItemIndex.
                int level;
                int veteranRank;
                EsoItemQuality quality;
                EsoItemTrait trait;
                int potionEffects;
                bool validIndex = TryParseIndex(ItemIndex,
                    out level, out veteranRank, out quality, out trait, out potionEffects);
                if (validIndex)
                {
                    Level = level;
                    VeteranRank = veteranRank;
                    Quality = quality;
                    Trait = trait;
                    PotionEffects = potionEffects;
                }

                IsValid = validIndex && !string.IsNullOrEmpty(BaseId);
            }
        }

        /// <summary>
        ///     Gets or sets the non-veteran level a player is required to be at in order to use this item.
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        ///     Gets or sets the human-readable, culture-specific name of this item, as it appears in-game.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets a number representing any extra effects this item provides, like improved stamina regen or restored
        ///     health.  Just for potions.
        /// </summary>
        public int PotionEffects { get; set; }

        /// <summary>
        ///     Gets or sets the item quality, which influences how effective it is.
        /// </summary>
        public EsoItemQuality Quality { get; set; }

        /// <summary>
        ///     Gets or sets the the special trait applied to this item, if any.  Just for armor, weapons, and jewelry.
        /// </summary>
        public EsoItemTrait Trait { get; set; }

        /// <summary>
        ///     Gets or sets the veteran rank a player is required to be at in order to use this item.
        /// </summary>
        public int VeteranRank { get; set; }

        /// <summary>
        ///     Populates the Level, VeteranRank, Quality, Trait, and PotionEffects properties from their corresponding values in
        ///     the ItemIndex property.
        /// </summary>
        /// <returns><c>true</c> if ItemIndex is able to be successfully parsed; otherwise <c>false</c>.</returns>
        public static bool TryParseIndex(string itemIndex, out int level, out int veteranRank,
            out EsoItemQuality quality, out EsoItemTrait trait, out int potionEffects)
        {
            // Confirm that the ItemIndex property look like 5 values separated by colons.
            string[] indexParts = null;

            // Try to parse each value of the index into an integer, and then validate
            // the quality and trait.
            var values = new int[5];
            if (string.IsNullOrEmpty(itemIndex)
                || (indexParts = itemIndex.Split(':')).Length != 5
                || indexParts.Where((t, i) => !int.TryParse(t, out values[i])).Any()
                || !Enum.IsDefined(typeof (EsoItemQuality), values[3])
                || !Enum.IsDefined(typeof (EsoItemTrait), values[4]))
            {
                level = -1;
                veteranRank = -1;
                quality = EsoItemQuality.Trash;
                trait = EsoItemTrait.None;
                potionEffects = -1;
                return false;
            }

            // Set properties
            level = values[0];
            veteranRank = values[1];
            quality = (EsoItemQuality) values[2];
            trait = (EsoItemTrait) values[3];
            potionEffects = values[4];

            return true;
        }

        /// <summary>
        ///     Loads the value from a given Lua table field into it's corresponding property, if one exists.
        /// </summary>
        /// <param name="field">The Lua table field containing the property and value to set.</param>
        internal void Set(LuaTableField field)
        {
            if (field == null) return;
            switch (field.Name)
            {
                case "itemIcon":
                    ItemIcon = field.Value;
                    break;
            }
        }
    }
}