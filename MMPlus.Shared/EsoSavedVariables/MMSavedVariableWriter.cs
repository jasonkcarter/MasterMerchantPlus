using System;
using System.Collections.Generic;
using System.IO;
using MMPlus.Shared.Model;

namespace MMPlus.Shared.EsoSavedVariables
{
    // ReSharper disable once InconsistentNaming
    public class MMSavedVariableWriter : IDisposable
    {
        private readonly string _templatesDirectory;
        private readonly StreamWriter _writer;
        private string _itemFooter;
        private string _itemHeader;
        private string _saleFooter;
        private string _saleHeader;
        private string _saleTemplate;

        public MMSavedVariableWriter(Stream stream)
        {
            _writer = new StreamWriter(stream);
            _templatesDirectory = File.Exists(".\\Templates\\MMDataHeader.txt") ? ".\\Templates" : ".";
        }

        private string ItemFooter
        {
            get
            {
                if (_itemFooter == null)
                {
                    _itemFooter = File.ReadAllText(Path.Combine(_templatesDirectory, "MMDataItemFooter.txt"));
                }
                return _itemFooter;
            }
        }

        private string ItemHeader
        {
            get
            {
                if (_itemHeader == null)
                {
                    _itemHeader = File.ReadAllText(Path.Combine(_templatesDirectory, "MMDataItemHeader.txt"));
                }
                return _itemHeader;
            }
        }

        private string SaleFooter
        {
            get
            {
                if (_saleFooter == null)
                {
                    _saleFooter = File.ReadAllText(Path.Combine(_templatesDirectory, "MMDataSaleFooter.txt"));
                }
                return _saleFooter;
            }
        }

        private string SaleHeader
        {
            get
            {
                if (_saleHeader == null)
                {
                    _saleHeader = File.ReadAllText(Path.Combine(_templatesDirectory, "MMDataSaleHeader.txt"));
                }
                return _saleHeader;
            }
        }

        private string SaleTemplate
        {
            get
            {
                if (_saleTemplate == null)
                {
                    _saleTemplate = File.ReadAllText(Path.Combine(_templatesDirectory, "MMDataSale.txt"));
                }
                return _saleTemplate;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Write(string variableName, string accountName, SortedSet<EsoSale> sales)
        {
            WriteHeader(variableName, accountName);

            EsoSale previousSale = null;
            int saleIndex = 0;
            foreach (var sale in sales)
            {
                if (previousSale != null)
                {
                    if (previousSale.ItemIndex != sale.ItemIndex)
                    {
                        WriteSaleFooter(previousSale);
                    }
                    if (previousSale.ItemBaseId != sale.ItemBaseId)
                    {
                        WriteItemFooter();
                    }
                }
                if (previousSale == null || previousSale.ItemBaseId != sale.ItemBaseId)
                {
                    WriteItemHeader(sale);
                }
                if (previousSale == null || previousSale.ItemIndex != sale.ItemIndex)
                {
                    WriteSaleHeader(sale);
                    saleIndex = 1;
                }
                else
                {
                    saleIndex++;
                }
                WriteSale(sale, saleIndex);

                previousSale = sale;
            }
            if (previousSale != null)
            {
                WriteSaleFooter(previousSale);
                WriteItemFooter();
            }
            WriteFooter();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && _writer != null)
            {
                _writer.Close();
            }
        }

        private void WriteFooter()
        {
            string footer = File.ReadAllText(Path.Combine(_templatesDirectory, "MMDataFooter.txt"));
            _writer.Write(footer);
        }

        private void WriteHeader(string variableName, string accountName)
        {
            string format = File.ReadAllText(Path.Combine(_templatesDirectory, "MMDataHeader.txt"));
            string header = string.Format(format, variableName, accountName);
            _writer.Write(header);
        }

        private void WriteItemFooter()
        {
            _writer.Write(ItemFooter);
        }

        private void WriteItemHeader(EsoSale sale)
        {
            string itemHeader = string.Format(ItemHeader, sale.ItemBaseId);
            _writer.Write(itemHeader);
        }

        private void WriteSale(EsoSale sale, int saleIndex)
        {
            string saleText = string.Format(SaleTemplate, saleIndex, sale.Seller, sale.WasKiosk.ToString().ToLower(),
                sale.SaleTimestamp, sale.ItemLink, sale.Buyer, sale.Price, sale.Quantity, sale.GuildName);
            _writer.Write(saleText);
        }

        private void WriteSaleFooter(EsoSale sale)
        {
            string saleFooter = string.Format(SaleFooter, sale.ItemIcon);
            _writer.Write(saleFooter);
        }

        private void WriteSaleHeader(EsoSale sale)
        {
            string saleHeader = string.Format(SaleHeader, sale.ItemIndex);
            _writer.Write(saleHeader);
        }
    }
}