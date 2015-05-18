using System;
using System.Runtime.Serialization;
using MMPlus.Shared.Interface;
using MMPlus.Shared.Model;

namespace MMPlus.Service.Model
{
    /// <summary>
    ///     Represents global aggregated Guild Store sales data for a single Elder Scrolls inventory item.
    /// </summary>
    [DataContract]
    public class EsoItemSalesData : EsoItem, IEsoItemSalesData
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
        ///     Backing field for the SalesDate property.
        /// </summary>
        private DateTimeOffset _salesDate;

        /// <summary>
        ///     Gets or sets the date that the sales data pertains to.
        /// </summary>
        public DateTimeOffset SalesDate
        {
            get { return _salesDate; }
            set
            {
                _salesDate = value;

                PartitionKey = value.ToString("yyyy-MM-dd");
            }
        }

        /// <summary>
        ///     Gets or sets the weighted average price of each individual inventory item of this type.
        /// </summary>
        public int SalesPriceAverage { get; set; }

        /// <summary>
        ///     Gets or sets the total sum of all prices for all sales for this item type.
        /// </summary>
        public long SalesPriceSum { get; set; }

        /// <summary>
        ///     Gets or sets the total sum of all quantities for all sales for this item type.
        /// </summary>
        public long SalesQuantitySum { get; set; }

        /// <summary>
        ///     Gets or sets the unique identifier for this item's base, without any special traits or qualities.
        /// </summary>
        public override string BaseId
        {
            get { return _baseId; }
            set
            {
                _baseId = value;
                RefreshRowKey();
            }
        }

        /// <summary>
        ///     Gets or sets a unique index to identify this specific variety of item, which incorporates required level/veteran
        ///     rank, quality, traits, and extra effects numbers.
        /// </summary>
        public override string ItemIndex
        {
            get { return _itemIndex; }
            set
            {
                _itemIndex = value;

                RefreshRowKey();

                // Populate Level, VeteranRank, Quality, Trait, and PotionEffects if this is a valid ItemIndex.
                int level;
                int veteranRank;
                EsoItemQuality quality;
                EsoItemTrait trait;
                int potionEffects;
                var validIndex = TryParseIndex(ItemIndex,
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
        ///     Sets the RowKey property based upon values in the BaseId and ItemIndex properties.
        /// </summary>
        private void RefreshRowKey()
        {
            if (BaseId == null || ItemIndex == null)
            {
                RowKey = null;
            }
            else
            {
                // Store each variety for an item in a separate row in the partition.
                RowKey = BaseId + ":" + ItemIndex;
            }
        }
    }
}