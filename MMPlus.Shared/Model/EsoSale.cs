using System;
using System.Globalization;
using System.Text;
using Microsoft.WindowsAzure.Storage.Table;
using MMPlus.Shared.Data;

namespace MMPlus.Shared.Model
{
    /// <summary>
    ///     Represents a guild store sale event in Elder Scrolls Online.
    /// </summary>
    public class EsoSale : TableEntity, ISemiDelimited
    {
        /// <summary>
        ///     Gets or sets a value indicating whether the RowKey property is automatically refreshed whenever its component
        ///     properties are updated.
        /// </summary>
        private bool _autoGenerateRowKey;

        /// <summary>
        ///     Backing field for the Buyer property.
        /// </summary>
        private string _buyer;

        /// <summary>
        ///     Backing field for the GuildName property.
        /// </summary>
        private string _guildName;

        /// <summary>
        ///     Backing field for the ItemBaseId property.
        /// </summary>
        private string _itemBaseId;

        /// <summary>
        ///     Backing field for the ItemIndex property.
        /// </summary>
        private string _itemIndex;

        /// <summary>
        ///     Backing field for the Price property.
        /// </summary>
        private int _price;

        /// <summary>
        ///     Backing field for the Quantity property.
        /// </summary>
        private int _quantity;

        /// <summary>
        ///     Backing field for the RelativeOrderIndex property.
        /// </summary>
        private int _relativeOrderIndex;

        /// <summary>
        ///     Backing field for the SaleTimestamp property.
        /// </summary>
        private int _saleTimestamp;

        /// <summary>
        ///     Backing field for the Seller property.
        /// </summary>
        private string _seller;

        /// <summary>
        ///     Backing field for the TimestampId property.
        /// </summary>
        private int _timestampId;

        /// <summary>
        ///     Backing field for the WasKiosk property.
        /// </summary>
        private bool _wasKiosk;

        public EsoSale() : this(true)
        {
        }

        public EsoSale(bool autoGenerateRowKey)
        {
            _autoGenerateRowKey = autoGenerateRowKey;
        }


        /// <summary>
        ///     Gets or sets the unique account id of the player that purchased the item.
        /// </summary>
        public string Buyer
        {
            get { return _buyer; }
            set
            {
                _buyer = value;
                if (_autoGenerateRowKey)
                {
                    GenerateRowKey();
                }
            }
        }

        /// <summary>
        ///     Gets or sets the name of the guild who's store the item was sold through.
        /// </summary>
        public string GuildName
        {
            get { return _guildName; }
            set
            {
                _guildName = value;
                if (_autoGenerateRowKey)
                {
                    GenerateRowKey();
                }
            }
        }

        /// <summary>
        ///     Gets or sets the unique identifier for this item's base, without any special traits or qualities.
        /// </summary>
        public string ItemBaseId
        {
            get { return _itemBaseId; }
            set
            {
                _itemBaseId = value;
                if (_autoGenerateRowKey)
                {
                    GenerateRowKey();
                }
            }
        }

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
                bool validIndex = EsoItem.TryParseIndex(ItemIndex,
                    out level, out veteranRank, out quality, out trait, out potionEffects);
                if (validIndex)
                {
                    ItemLevel = level;
                    ItemVeteranRank = veteranRank;
                    ItemQuality = quality;
                    ItemTrait = trait;
                    ItemPotionEffects = potionEffects;
                }

                if (_autoGenerateRowKey)
                {
                    GenerateRowKey();
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
        public int Price
        {
            get { return _price; }
            set
            {
                _price = value;
                if (_autoGenerateRowKey)
                {
                    GenerateRowKey();
                }
            }
        }

        /// <summary>
        ///     Gets or sets the number of items in the stack that was purchased.
        /// </summary>
        public int Quantity
        {
            get { return _quantity; }
            set
            {
                _quantity = value;
                if (_autoGenerateRowKey)
                {
                    GenerateRowKey();
                }
            }
        }

        /// <summary>
        ///     Gets or sets the index used to identify the relative order that sales with the same guild, timestampid, buyer,
        ///     seller, item, quantity, and price occured in, when there are multiples of the same sort of sale in rapid
        ///     succession.
        /// </summary>
        public int RelativeOrderIndex
        {
            get { return _relativeOrderIndex; }
            set
            {
                _relativeOrderIndex = value;
                if (_autoGenerateRowKey)
                {
                    GenerateRowKey();
                }
            }
        }

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
        public string Seller
        {
            get { return _seller; }
            set
            {
                _seller = value;
                if (_autoGenerateRowKey)
                {
                    GenerateRowKey();
                }
            }
        }

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
        public int TimestampId
        {
            get { return _timestampId; }
            private set
            {
                _timestampId = value;
                PartitionKey = value.ToString(CultureInfo.InvariantCulture);
            }
        }


        /// <summary>
        ///     Gets or sets a value indicating whether the sale occurred at a Guild Trader kiosk.
        /// </summary>
        /// <value>
        ///     <c>true</c> if the item was purchased at a Guild Trader kiosk; otherwise, <c>false</c>.
        /// </value>
        public bool WasKiosk
        {
            get { return _wasKiosk; }
            set
            {
                _wasKiosk = value;
                if (_autoGenerateRowKey)
                {
                    GenerateRowKey();
                }
            }
        }

        public virtual void GenerateRowKey()
        {
            RowKey = string.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9}",
                PrepareForKey(GuildName),
                SaleTimestamp,
                PrepareForKey(ItemBaseId),
                PrepareForKey(ItemIndex),
                Quantity,
                Price,
                PrepareForKey(Buyer),
                PrepareForKey(Seller),
                RelativeOrderIndex,
                WasKiosk);
        }

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

        public void LoadFromSemiDelimited(string data)
        {
            string[] parts = data.Split(';');
            if (parts.Length != 12)
            {
                throw new InvalidOperationException(string.Format("Expected 12 fields in serialized input. Actual {0}",
                    parts.Length));
            }
            bool autoGenerateRowKey = _autoGenerateRowKey;
            _autoGenerateRowKey = false;
            GuildName = parts[0];
            SaleTimestamp = int.Parse(parts[1]);
            ItemBaseId = parts[2];
            ItemIndex = parts[3];
            Quantity = int.Parse(parts[4]);
            Price = int.Parse(parts[5]);
            Buyer = parts[6];
            Seller = parts[7];
            RelativeOrderIndex = int.Parse(parts[8]);
            WasKiosk = bool.Parse(parts[9]);
            ItemLink = parts[10];
            Submitter = parts[11];
            GenerateRowKey();
            _autoGenerateRowKey = autoGenerateRowKey;
        }

        /// <summary>
        ///     Sets all item-related properties with values contained in a given item instance.
        /// </summary>
        /// <param name="item">The item containing the property data to copy.</param>
        public void Set(EsoItem item)
        {
            bool autoPopulateRowKey = _autoGenerateRowKey;
            _autoGenerateRowKey = false;
            ItemBaseId = item.BaseId;
            ItemIndex = item.ItemIndex;
            ItemIcon = item.ItemIcon;
            GenerateRowKey();
            _autoGenerateRowKey = autoPopulateRowKey;
        }

        /// <summary>
        ///     Loads the value from a given Lua table field into it's corresponding property, if one exists.
        /// </summary>
        public void Set(string fieldKey, string fieldValue)
        {
            switch (fieldKey)
            {
                case "buyer":
                    Buyer = fieldValue;
                    break;
                case "guild":
                    GuildName = fieldValue;
                    break;
                case "seller":
                    Seller = fieldValue;
                    break;
                case "itemLink":
                    ItemLink = fieldValue;
                    break;
                case "price":
                    int price;
                    if (int.TryParse(fieldValue, out price))
                    {
                        Price = price;
                    }
                    break;
                case "wasKiosk":
                    bool wasKiosk;
                    if (bool.TryParse(fieldValue, out wasKiosk))
                    {
                        WasKiosk = wasKiosk;
                    }
                    break;
                case "quant":
                    int quantity;
                    if (int.TryParse(fieldValue, out quantity))
                    {
                        Quantity = quantity;
                    }
                    break;
                case "timestamp":
                    int timestamp;
                    if (int.TryParse(fieldValue, out timestamp))
                    {
                        SaleTimestamp = timestamp;
                    }
                    break;
            }
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

        /// <summary>
        ///     Removes any special characters not allowed in Azure table storage as keys.
        ///     See https://msdn.microsoft.com/library/azure/dd179338.aspx for details.
        /// </summary>
        /// <param name="input">The string to prepare for use as an Azure Table Storage key value.</param>
        private static string PrepareForKey(string input)
        {
            if (input == null)
            {
                return null;
            }
            var output = new StringBuilder();
            foreach (char c in input)
            {
                // Ignore control characters
                if (char.IsControl(c))
                {
                    continue;
                }
                // See Characters Disallowed in Key Fields
                // at https://msdn.microsoft.com/library/azure/dd179338.aspx 
                switch (c)
                {
                    case '/':
                    case '\\':
                    case '#':
                    case '?':
                        continue;
                    default:
                        output.Append(c);
                        break;
                }
            }
            return output.ToString();
        }
    }
}