using PangyaAPI.IFF.Models;
using PangyaAPI.IFF.StructModels;
using PangyaAPI.Utilities.BinaryModels;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Linq;
using PangyaAPI.IFF.Lister;
using System.Diagnostics;
using PangyaAPI.IFF.Extensions;
using System.Xml.Linq;
using System.Windows.Forms;

namespace PangyaAPI.IFF.Collections
{
    public class PartCollection : IFFEntryList<Part>
    {
        public uint GetRentalPrice(uint TypeId)
        {
            Part item = new Part();
            if (!LoadItem(TypeId, ref item))
            {
                return 0;
            }
            if ((item.Enabled == 1))
            {
                return item.RentPang;
            }
            return 0;
        }
    }
}
