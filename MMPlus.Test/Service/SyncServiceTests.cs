using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMPlus.Service.Model;
using MMPlus.Service.Services;

namespace MMPlus.Test.Service
{
    [TestClass]
    public class SyncServiceTests
    {
        [TestMethod]
        public void SyncService_Put()
        {
            // Arrange

            var repository = new MemoryStorageRepository();
            MemoryStorageTable<EsoSale> salesTable = repository.GetTable<EsoSale>();
            var existingSales = new []
            {
                new EsoSale(false)
                {
                    Buyer = "@account1",
                    Seller = "@account2",
                    GuildName = "Ethereal Traders Union",
                    ItemBaseId = "45871",
                    ItemIndex = "50:3:1:0:0",
                    ItemIcon = "/esoui/art/icons/enchantment_jewelry_reducefeatcosts.dds",
                    ItemLink = "|H0:item:45871:113:50:0:0:0:0:0:0:0:0:0:0:0:0:7:0:0:0:0:0|hgreater glyph of reduce feat cost^n|h",
                    Quantity = 1,
                    Price = 150,
                    SaleTimestamp = 1423388720,
                    RelativeOrderIndex = 0,
                }.GenerateRowKey(), 
                
                new EsoSale(false)
                {
                    Buyer = "@account1",
                    Seller = "@account2",
                    GuildName = "Ethereal Traders Union",
                    ItemBaseId = "45871",
                    ItemIndex = "50:3:1:0:0",
                    ItemIcon = "/esoui/art/icons/enchantment_jewelry_reducefeatcosts.dds",
                    ItemLink = "|H0:item:45871:113:50:0:0:0:0:0:0:0:0:0:0:0:0:7:0:0:0:0:0|hgreater glyph of reduce feat cost^n|h",
                    Quantity = 1,
                    Price = 150,
                    SaleTimestamp = 1423388725,
                    RelativeOrderIndex = 1,
                }.GenerateRowKey(), 
                
                new EsoSale(false)
                {
                    Buyer = "@account1",
                    Seller = "@account2",
                    GuildName = "Ethereal Traders Union",
                    ItemBaseId = "45871",
                    ItemIndex = "50:3:1:0:0",
                    ItemIcon = "/esoui/art/icons/enchantment_jewelry_reducefeatcosts.dds",
                    ItemLink = "|H0:item:45871:113:50:0:0:0:0:0:0:0:0:0:0:0:0:7:0:0:0:0:0|hgreater glyph of reduce feat cost^n|h",
                    Quantity = 1,
                    Price = 150,
                    SaleTimestamp = 1423388730,
                    RelativeOrderIndex = 2,
                }.GenerateRowKey(), 
            };

            foreach (EsoSale existingSale in existingSales)
            {
                salesTable.InsertOrReplace(existingSale);
            }

            var newSales = new[]
            {
                new EsoSale(false)
                {
                    Buyer = "@account1",
                    Seller = "@account2",
                    GuildName = "Ethereal Traders Union",
                    ItemBaseId = "45871",
                    ItemIndex = "50:3:1:0:0",
                    ItemIcon = "/esoui/art/icons/enchantment_jewelry_reducefeatcosts.dds",
                    ItemLink = "|H0:item:45871:113:50:0:0:0:0:0:0:0:0:0:0:0:0:7:0:0:0:0:0|hgreater glyph of reduce feat cost^n|h",
                    Quantity = 1,
                    Price = 150,
                    SaleTimestamp = 1423388720,
                    RelativeOrderIndex = 0,
                }.GenerateRowKey(), 
                
                new EsoSale(false)
                {
                    Buyer = "@account1",
                    Seller = "@account2",
                    GuildName = "Ethereal Traders Union",
                    ItemBaseId = "45871",
                    ItemIndex = "50:3:1:0:0",
                    ItemIcon = "/esoui/art/icons/enchantment_jewelry_reducefeatcosts.dds",
                    ItemLink = "|H0:item:45871:113:50:0:0:0:0:0:0:0:0:0:0:0:0:7:0:0:0:0:0|hgreater glyph of reduce feat cost^n|h",
                    Quantity = 1,
                    Price = 150,
                    SaleTimestamp = 1423388725,
                    RelativeOrderIndex = 1,
                }.GenerateRowKey(), 

                
                new EsoSale(false)
                {
                    Buyer = "@account1",
                    Seller = "@account2",
                    GuildName = "Ethereal Traders Union",
                    ItemBaseId = "45871",
                    ItemIndex = "50:3:1:0:0",
                    ItemIcon = "/esoui/art/icons/enchantment_jewelry_reducefeatcosts.dds",
                    ItemLink = "|H0:item:45871:113:50:0:0:0:0:0:0:0:0:0:0:0:0:7:0:0:0:0:0|hgreater glyph of reduce feat cost^n|h",
                    Quantity = 1,
                    Price = 150,
                    SaleTimestamp = 1423388725,
                    RelativeOrderIndex = 2,
                }.GenerateRowKey(), 
                
                new EsoSale(false)
                {
                    Buyer = "@account1",
                    Seller = "@account2",
                    GuildName = "Ethereal Traders Union",
                    ItemBaseId = "45871",
                    ItemIndex = "50:3:1:0:0",
                    ItemIcon = "/esoui/art/icons/enchantment_jewelry_reducefeatcosts.dds",
                    ItemLink = "|H0:item:45871:113:50:0:0:0:0:0:0:0:0:0:0:0:0:7:0:0:0:0:0|hgreater glyph of reduce feat cost^n|h",
                    Quantity = 1,
                    Price = 150,
                    SaleTimestamp = 1423388730,
                    RelativeOrderIndex = 2,
                }.GenerateRowKey(), 

                
                new EsoSale(false)
                {
                    Buyer = "@account1",
                    Seller = "@account2",
                    GuildName = "Ethereal Traders Union",
                    ItemBaseId = "45871",
                    ItemIndex = "50:3:1:0:0",
                    ItemIcon = "/esoui/art/icons/enchantment_jewelry_reducefeatcosts.dds",
                    ItemLink = "|H0:item:45871:113:50:0:0:0:0:0:0:0:0:0:0:0:0:7:0:0:0:0:0|hgreater glyph of reduce feat cost^n|h",
                    Quantity = 1,
                    Price = 150,
                    SaleTimestamp = 1423388730,
                    RelativeOrderIndex = 2,
                }.GenerateRowKey(), 

                
                new EsoSale(false)
                {
                    Buyer = "@account1",
                    Seller = "@account2",
                    GuildName = "Ethereal Traders Union",
                    ItemBaseId = "45871",
                    ItemIndex = "50:3:1:0:0",
                    ItemIcon = "/esoui/art/icons/enchantment_jewelry_reducefeatcosts.dds",
                    ItemLink = "|H0:item:45871:113:50:0:0:0:0:0:0:0:0:0:0:0:0:7:0:0:0:0:0|hgreater glyph of reduce feat cost^n|h",
                    Quantity = 1,
                    Price = 151,
                    SaleTimestamp = 1423388800,
                    RelativeOrderIndex = 1,
                }.GenerateRowKey(), 
            };

            // Act
            var service = new SyncService(repository);
            service.Put("MachineId1", newSales);

            // Assert

        }
    }
}
