using System;
using MMPlus.Shared.Model;

namespace MMPlus.Shared.EsoSavedVariables
{
    public class EsoGuildStoreSaleEventArgs : EventArgs
    {
        public EsoSale Sale { get; set; }
    }
}