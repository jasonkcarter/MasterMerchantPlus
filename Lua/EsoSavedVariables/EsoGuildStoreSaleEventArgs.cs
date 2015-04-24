using System;
using MMPlus.Client.Model;

namespace Lua.EsoSavedVariables
{
    public class EsoGuildStoreSaleEventArgs : EventArgs
    {
        public EsoSale Sale { get; set; }
    }
}