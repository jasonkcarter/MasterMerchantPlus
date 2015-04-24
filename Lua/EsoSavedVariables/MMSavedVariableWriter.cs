using System;
using System.IO;
using Lua.Properties;
using MMPlus.Client.Model;
using MMPlus.Shared.Utility;

namespace Lua.EsoSavedVariables
{
    // ReSharper disable once InconsistentNaming
    public class MMSavedVariableWriter : IDisposable
    {
        private readonly StreamWriter _writer;

        public MMSavedVariableWriter(Stream stream)
        {
            _writer = new StreamWriter(stream);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Write(string variableName, string accountName, SortedCollection<EsoSale> sales)
        {
            WriteHeader(variableName, accountName);

            EsoSale previousSale = null;
            var saleIndex = 0;
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
                _writer.Dispose();
            }
        }

        private void WriteFooter()
        {
            var footer = Resources.MMDataFooter;
            _writer.Write(footer);
        }

        private void WriteHeader(string variableName, string accountName)
        {
            var format = Resources.MMDataHeader;
            var header = string.Format(format, variableName, accountName);
            _writer.Write(header);
        }

        private void WriteItemFooter()
        {
            _writer.Write(Resources.MMDataItemFooter);
        }

        private void WriteItemHeader(EsoSale sale)
        {
            var itemHeader = string.Format(Resources.MMDataItemHeader, sale.ItemBaseId);
            _writer.Write(itemHeader);
        }

        private void WriteSale(EsoSale sale, int saleIndex)
        {
            var saleText = string.Format(Resources.MMDataSale, saleIndex, sale.Seller,
                sale.WasKiosk.ToString().ToLower(),
                sale.SaleTimestamp, sale.ItemLink, sale.Buyer, sale.Price, sale.Quantity, sale.GuildName);
            _writer.Write(saleText);
        }

        private void WriteSaleFooter(EsoSale sale)
        {
            var saleFooter = string.Format(Resources.MMDataSaleFooter, sale.ItemIcon);
            _writer.Write(saleFooter);
        }

        private void WriteSaleHeader(EsoSale sale)
        {
            var saleHeader = string.Format(Resources.MMDataSaleHeader, sale.ItemIndex);
            _writer.Write(saleHeader);
        }
    }
}