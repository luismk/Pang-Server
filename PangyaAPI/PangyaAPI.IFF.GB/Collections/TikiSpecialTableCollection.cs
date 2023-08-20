using PangyaAPI.IFF.Lister;
using PangyaAPI.IFF.Models;
using PangyaAPI.IFF.StructModels;
using PangyaAPI.Utilities.BinaryModels;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace PangyaAPI.IFF.Collections
{
    public class TikiSpecialTableCollection : IFFEntryList<TikiSpecialTable>
    {
        //Destructor
        ~TikiSpecialTableCollection()
        {
            this.Clear();
        }

    }
}
