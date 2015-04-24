using MMPlus.Shared.Model;

namespace MMPlus.Shared.Interface
{
    /// <summary>
    ///     Represents a specific Elder Scrolls Online item, with all of its special traits, if any.
    /// </summary>
    public interface IEsoItem
    {
        /// <summary>
        ///     Gets or sets the unique identifier for this item's base, without any special traits or qualities.
        /// </summary>
        string BaseId { get; set; }

        /// <summary>
        ///     Gets a value indicating whether this instance is valid.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is valid; otherwise, <c>false</c>.
        /// </value>
        bool IsValid { get; }

        /// <summary>
        ///     Gets or sets the path to this item's icon file.
        /// </summary>
        string ItemIcon { get; set; }

        /// <summary>
        ///     Gets or sets a unique index to identify this specific variety of item, which incorporates required level/veteran
        ///     rank, quality, traits, and extra effects numbers.
        /// </summary>
        string ItemIndex { get; set; }

        /// <summary>
        ///     Gets or sets the non-veteran level a player is required to be at in order to use this item.
        /// </summary>
        int Level { get; set; }

        /// <summary>
        ///     Gets or sets the human-readable, culture-specific name of this item, as it appears in-game.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        ///     Gets or sets a number representing any extra effects this item provides, like improved stamina regen or restored
        ///     health.  Just for potions.
        /// </summary>
        int PotionEffects { get; set; }

        /// <summary>
        ///     Gets or sets the item quality, which influences how effective it is.
        /// </summary>
        EsoItemQuality Quality { get; set; }

        /// <summary>
        ///     Gets or sets the the special trait applied to this item, if any.  Just for armor, weapons, and jewelry.
        /// </summary>
        EsoItemTrait Trait { get; set; }

        /// <summary>
        ///     Gets or sets the veteran rank a player is required to be at in order to use this item.
        /// </summary>
        int VeteranRank { get; set; }
    }
}