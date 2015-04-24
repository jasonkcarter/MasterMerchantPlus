using MMPlus.Shared.Model;

namespace MMPlus.Shared.Interface
{
    /// <summary>
    ///     Represents a guild store sale event in Elder Scrolls Online.
    /// </summary>
    public interface IEsoSale
    {
        /// <summary>
        ///     Gets or sets the unique account id of the player that purchased the item.
        /// </summary>
        string Buyer { get; set; }

        /// <summary>
        ///     Gets or sets the name of the guild who's store the item was sold through.
        /// </summary>
        string GuildName { get; set; }

        /// <summary>
        ///     Gets or sets the unique identifier for this item's base, without any special traits or qualities.
        /// </summary>
        string ItemBaseId { get; set; }

        /// <summary>
        ///     Gets or sets the path to the item's icon file.
        /// </summary>
        string ItemIcon { get; set; }

        /// <summary>
        ///     Gets or sets a unique index to identify the specific variety of item purchased, which incorporates required
        ///     level/veteran
        ///     rank, quality, traits, and extra effects numbers.
        /// </summary>
        string ItemIndex { get; set; }

        /// <summary>
        ///     Gets or sets the non-veteran level a player is required to be at in order to use the item purchased.
        /// </summary>
        int ItemLevel { get; set; }

        /// <summary>
        ///     Gets or sets the unique ID that ESO uses to identify the item purchased.
        /// </summary>
        string ItemLink { get; set; }

        /// <summary>
        ///     Gets or sets a number representing any extra effects the item purchased provides, like improved stamina regen or
        ///     restored health.  Just for potions.
        /// </summary>
        int ItemPotionEffects { get; set; }

        /// <summary>
        ///     Gets or sets the item quality, which influences how effective it is.
        /// </summary>
        EsoItemQuality ItemQuality { get; set; }

        /// <summary>
        ///     Gets or sets the the special trait supplied by the item purchased, if any.  Just for armor, weapons, and jewelry.
        /// </summary>
        EsoItemTrait ItemTrait { get; set; }

        /// <summary>
        ///     Gets or sets the veteran rank a player is required to be at in order to use this the item purchased in this sale.
        /// </summary>
        int ItemVeteranRank { get; set; }

        /// <summary>
        ///     Gets or sets the price paid for the entire stack of items, in gold.
        /// </summary>
        int Price { get; set; }

        /// <summary>
        ///     Gets or sets the number of items in the stack that was purchased.
        /// </summary>
        int Quantity { get; set; }

        /// <summary>
        ///     Gets or sets the index used to identify the relative order that sales with the same guild, timestampid, buyer,
        ///     seller, item, quantity, and price occured in, when there are multiples of the same sort of sale in rapid
        ///     succession.
        /// </summary>
        int RelativeOrderIndex { get; set; }

        /// <summary>
        ///     Gets or sets the Unix timestamp (i.e. number of seconds since 1970-01-01 00:00:00) that the sale occured at.
        /// </summary>
        int SaleTimestamp { get; set; }

        /// <summary>
        ///     Gets or sets the unique account id of the player that listed and sold the item.
        /// </summary>
        string Seller { get; set; }

        /// <summary>
        ///     Gets or sets the account name of the player that submits the sale for inclusion in the global aggregate data set.
        /// </summary>
        string Submitter { get; set; }

        /// <summary>
        ///     Gets the value used to group sales that happen around the same time together, since TimestampMinimum and
        ///     SaleTimestamp from
        ///     different
        ///     clients usually don't match.
        /// </summary>
        int TimestampId { get; }

        /// <summary>
        ///     Gets or sets a value indicating whether the sale occurred at a Guild Trader kiosk.
        /// </summary>
        /// <value>
        ///     <c>true</c> if the item was purchased at a Guild Trader kiosk; otherwise, <c>false</c>.
        /// </value>
        bool WasKiosk { get; set; }
    }
}