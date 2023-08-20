﻿using PangyaAPI.IFF.StructModels;
using System.Runtime.InteropServices;
namespace PangyaAPI.IFF.Models
{


    #region Struct QuestItem.iff
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public class QuestItem : IFFCommon
    {
        public uint Unknown { get; set; }
        public uint Quest_Type { get; set; }
        public uint Quest_Counter { get; set; }
        [field: MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public uint[] Quest_TypeID { get; set; }
        [field: MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public uint[] Reward_Item_TypeID { get; set; }
        [field: MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public uint[] Reward_Item_Qtnd { get; set; }
        [field: MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public uint[] Reward_Item_Time { get; set; }
        public uint Unknown1 { get; set; }

    }
    #endregion    
}
