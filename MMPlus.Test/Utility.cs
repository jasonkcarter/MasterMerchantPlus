using System;
using MMPlus.Shared.Model;

namespace MMPlus.Test
{
    public static class Utility
    {
        public static readonly string[] Guilds =
        {
            "Akaveri Imports",
            "Bal-mart",
            "Bleakrock Barter Co",
            "Craftoholics",
            "Dominant Dominion",
            "Dominion Imperial Guard",
            "East Empire Company",
            "Elder Scrolls Exchange",
            "Ethereal Traders Union",
            "Ethereal Traders Union II",
            "Fangs of Ironglaive",
            "Gold Dragons",
            "Iron Bank of Bravos"
        };

        public static readonly string[] Items =
        {
            "45109;1:0:1:13:0;|H0:item:45109:30:1:0:0:0:0:0:0:0:0:0:0:0:0:7:0:0:0:10000:0|hhomespun sash^p|h"
        };

        public static EsoSale CreateRandomSale()
        {
            string randomGuild = Guilds[new Random().Next(Guilds.Length)];
            int randomQuantity = new Random().Next(100) + 1;
            int randomPrice = new Random().Next(10000) + 1;
            int randomTimestamp = new Random().Next(10) + 1425340800;
            string[] randomItemParts = Items[new Random().Next(Items.Length)].Split(';');
            string itemBaseId = randomItemParts[0];
            string itemIndex = randomItemParts[1];
            string itemLink = randomItemParts[2];
            var sale = new EsoSale(false)
            {
                GuildName = randomGuild,
                Buyer = "@" + Guid.NewGuid().ToString("N"),
                Seller = "@" + Guid.NewGuid().ToString("N"),
                ItemBaseId = itemBaseId,
                ItemIndex = itemIndex,
                ItemLink = itemLink,
                Quantity = randomQuantity,
                Price = randomPrice,
                SaleTimestamp = randomTimestamp
            };
            sale.GenerateRowKey();
            return sale;
        }
    }
}