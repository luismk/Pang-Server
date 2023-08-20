using PangyaAPI.IFF.StructModels;
using System;
using System.Runtime.InteropServices;
namespace PangyaAPI.IFF.Models
{

    #region Struct Caddie.iff
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public class Caddie : IFFCommon
    {
        public uint Salary { get; set; }
        [field: MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x27 + 1)]
        public string MPet { get; set; }
        public ushort Power { get; set; }
        public ushort Control { get; set; }
        public ushort Impact { get; set; }
        public ushort Spin { get; set; }
        public ushort Curve { get; set; }
        public ushort Un4 { get; set; }
    }
    #endregion
}
