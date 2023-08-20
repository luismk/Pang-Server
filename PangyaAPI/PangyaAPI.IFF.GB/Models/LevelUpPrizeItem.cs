using PangyaAPI.IFF.StructModels;
using PangyaAPI.IFF.Definitions;
using System;
using System.Runtime.InteropServices;
using System.Text;

namespace PangyaAPI.IFF.Models
{



    #region Struct LevelUpItem.iff
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public class LevelUpPrizeItem
    {
        public byte Active { get; set; }
        [field: MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)]
        byte[] NameInBytes { get; set; }//8 start position
        public string Name { get => Encoding.GetEncoding("Shift_JIS").GetString(NameInBytes); set => NameInBytes = Encoding.GetEncoding("Shift_JIS").GetBytes(value.PadRight(33, '\0')); }

        public ushort Level { get; set; }
        [field: MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public uint[] TypeID;
        [field: MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public uint[] Quantity;
        [field: MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public uint[] Time;
        [field: MarshalAs(UnmanagedType.ByValArray, SizeConst = 132)]
        byte[] DescriptionInBytes { get; set; }//8 start position
        public string Description { get => Encoding.GetEncoding("Shift_JIS").GetString(DescriptionInBytes); set => DescriptionInBytes = Encoding.GetEncoding("Shift_JIS").GetBytes(value.PadRight(132, '\0')); }

    }
    #endregion

}
