using PangyaAPI.IFF.Models;
using PangyaAPI.IFF.StructModels;
using PangyaAPI.Utilities.BinaryModels;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Linq;
using PangyaAPI.IFF.Lister;
namespace PangyaAPI.IFF.Collections
{
    public class CaddieItemCollection : IFFEntryList<CaddieItem>
    {
        public UInt32 GetPrice(UInt32 TypeID, uint Day)
        {
            foreach (var Item in this)
            {
                if (Item.Enabled == 1 && Item.TypeID == TypeID)
                {
                    switch (Day)
                    {
                        case 1:
                            return Item.Price15Day;
                        case 15:
                            return Item.Price15Day;
                        case 30:
                            return Item.Price30Day;
                    }
                }

                if (Item.Price1Day == 0 && Item.Price15Day == 0 && Item.Price30Day == 0 && Item.TypeID == TypeID)
                {
                    return (uint)Item.Shop.flag_shop.PriceType;
                }
            }

            return 0;
        }
    }
}
