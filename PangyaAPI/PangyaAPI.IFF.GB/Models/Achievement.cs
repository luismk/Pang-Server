using PangyaAPI.IFF.StructModels;
using System.Runtime.InteropServices;
using System.Text;

namespace PangyaAPI.IFF.Models
{
    #region Struct Achievement.iff
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public class Achievement : IFFCommon
    {
        public uint TypeID_Quest_Index { get; set; }
        public uint Achievement_Type { get; set; }
        [field: MarshalAs(UnmanagedType.ByValArray, SizeConst = 129)]
        public byte[] QuestNameInBytes { get; set; }
        public string QuestName { get => Encoding.GetEncoding("Shift_JIS").GetString(QuestNameInBytes).Replace("\0", ""); set => QuestNameInBytes = Encoding.GetEncoding("Shift_JIS").GetBytes(value.PadRight(129, '\0')); }

        [field: MarshalAs(UnmanagedType.ByValArray, SizeConst = 129)]
        public byte[] QuestNameInBytes1 { get; set; }
        public string QuestName1 { get => Encoding.GetEncoding("Shift_JIS").GetString(QuestNameInBytes1).Replace("\0", ""); set => QuestNameInBytes1 = Encoding.GetEncoding("Shift_JIS").GetBytes(value.PadRight(129, '\0')); }

        [field: MarshalAs(UnmanagedType.ByValArray, SizeConst = 129)]
        public byte[] QuestNameInBytes2 { get; set; }
        public string QuestName2 { get => Encoding.GetEncoding("Shift_JIS").GetString(QuestNameInBytes2).Replace("\0", ""); set => QuestNameInBytes2 = Encoding.GetEncoding("Shift_JIS").GetBytes(value.PadRight(129, '\0')); }

        [field: MarshalAs(UnmanagedType.ByValArray, SizeConst = 129)]
        public byte[] QuestNameInBytes3 { get; set; }
        public string QuestName3 { get => Encoding.GetEncoding("Shift_JIS").GetString(QuestNameInBytes3).Replace("\0", ""); set => QuestNameInBytes3 = Encoding.GetEncoding("Shift_JIS").GetBytes(value.PadRight(129, '\0')); }

        [field: MarshalAs(UnmanagedType.ByValArray, SizeConst = 129)]
        public byte[] QuestNameInBytes4 { get; set; }
        public string QuestName4 { get => Encoding.GetEncoding("Shift_JIS").GetString(QuestNameInBytes4).Replace("\0", ""); set => QuestNameInBytes4 = Encoding.GetEncoding("Shift_JIS").GetBytes(value.PadRight(129, '\0')); }

        [field: MarshalAs(UnmanagedType.ByValArray, SizeConst = 129)]
        public byte[] QuestNameInBytes5 { get; set; }
        public string QuestName5 { get => Encoding.GetEncoding("Shift_JIS").GetString(QuestNameInBytes5).Replace("\0", ""); set => QuestNameInBytes5 = Encoding.GetEncoding("Shift_JIS").GetBytes(value.PadRight(129, '\0')); }

        [field: MarshalAs(UnmanagedType.ByValArray, SizeConst = 129)]
        public byte[] QuestNameInBytes6 { get; set; }
        public string QuestName6 { get => Encoding.GetEncoding("Shift_JIS").GetString(QuestNameInBytes6).Replace("\0", ""); set => QuestNameInBytes6 = Encoding.GetEncoding("Shift_JIS").GetBytes(value.PadRight(129, '\0')); }

        [field: MarshalAs(UnmanagedType.ByValArray, SizeConst = 129)]
        public byte[] QuestNameInBytes7 { get; set; }
        public string QuestName7 { get => Encoding.GetEncoding("Shift_JIS").GetString(QuestNameInBytes7).Replace("\0", ""); set => QuestNameInBytes7 = Encoding.GetEncoding("Shift_JIS").GetBytes(value.PadRight(129, '\0')); }

        [field: MarshalAs(UnmanagedType.ByValArray, SizeConst = 129)]
        public byte[] QuestNameInBytes8 { get; set; }
        public string QuestName8 { get => Encoding.GetEncoding("Shift_JIS").GetString(QuestNameInBytes8).Replace("\0", ""); set => QuestNameInBytes8 = Encoding.GetEncoding("Shift_JIS").GetBytes(value.PadRight(129, '\0')); }

        [field: MarshalAs(UnmanagedType.ByValArray, SizeConst = 129)]
        public byte[] QuestNameInBytes9 { get; set; }
        public string QuestName9 { get => Encoding.GetEncoding("Shift_JIS").GetString(QuestNameInBytes9).Replace("\0", ""); set => QuestNameInBytes9 = Encoding.GetEncoding("Shift_JIS").GetBytes(value.PadRight(129, '\0')); }
        public short S_Unknown { get; set; }
        [field: MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public uint[] Quest_TypeID { get; set; }
        public uint T_Unknown { get; set; }
    }
    #endregion
}
