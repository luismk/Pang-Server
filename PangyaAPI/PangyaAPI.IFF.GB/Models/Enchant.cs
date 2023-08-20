﻿using PangyaAPI.IFF.StructModels;
using System;
using System.Runtime.InteropServices;
namespace PangyaAPI.IFF.Models
{
    /// <summary>
    /// Is Struct file Enchant.iff
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public class Enchant
    {
        public uint Enable { get; set; }
        public uint TypeID { get; set; }
        public long Pang { get; set; }
    }
}
