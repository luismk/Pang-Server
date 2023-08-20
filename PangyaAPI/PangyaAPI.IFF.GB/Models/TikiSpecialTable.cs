using PangyaAPI.IFF.StructModels;
using System.Runtime.InteropServices;
namespace PangyaAPI.IFF.Models
{
    /// <summary>
    /// Is Struct file TikiSpecialTable.iff
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public class TikiSpecialTable
    {
        public uint Enable { get; set; }
        public byte TypeID { get; set; }
        [field: MarshalAs(UnmanagedType.ByValTStr, SizeConst = 35)]
        public string Name { get; set; }
        public uint Qty { get; set; }
        [field: MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public uint[] TypeID_Item { get; set; }
    }
}
