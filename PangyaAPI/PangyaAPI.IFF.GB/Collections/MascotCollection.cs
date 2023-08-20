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
    public class MascotCollection : IFFEntryList<Mascot>
    {
        public UInt32 GetPrice(UInt32 TypeID, uint Day)
        {
            Mascot Mascot = new Mascot();
            if (!LoadItem(TypeID, ref Mascot))
            {
                return 0;
            }
            if (Mascot.Enabled == 1)
            {
                switch (Day)
                {
                    case 1:
                        return Mascot.Price1Day;
                    case 7:
                        return Mascot.Price7Day;
                    case 30:
                        return Mascot.Price30Day;
                }
            }

            if (Mascot.Price1Day == 0 && Mascot.Price7Day == 0 && Mascot.Price30Day == 0)
            {
                return (uint)Mascot.Shop.flag_shop.PriceType;
            }
            return 0;
        }

        public uint GetSalary(uint TypeId, uint Day)
        {
            Mascot Item = new Mascot();
            if (!LoadItem(TypeId, ref Item))
            {
                return 0;
            }
            if (Item.Enabled == 1)
            {
                switch (Day)
                {
                    case 1:
                        return Item.Price1Day;
                    case 7:
                        return Item.Price7Day;
                    case 30:
                        return Item.Price30Day;
                }
            }
            return 0;
        }
    }
}
