﻿using PangyaAPI.IFF.Models;
using PangyaAPI.IFF.StructModels;
using PangyaAPI.Utilities.BinaryModels;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Linq;
using PangyaAPI.IFF.Lister;
using PangyaAPI.IFF.Extensions;

namespace PangyaAPI.IFF.Collections
{
    public class CaddieCollection : IFFEntryList<Caddie>
    {
        public UInt32 GetSalary(uint TypeId)
        {
            return GetItem(TypeId).Salary;
        }
    }
}
