using PangyaAPI.IFF.StructModels;
using PangyaAPI.IFF.Definitions;
using System;
using System.Runtime.InteropServices;
namespace PangyaAPI.IFF.Models
{
    #region Struct HairStyle.iff
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public class HairStyle : IFFCommon
    {
        public byte Color { get; set; }
        public CharTypeByHairColor Character { get; set; }
        public ushort Blank { get; set; }
    }
    #endregion
}
