﻿using PangyaAPI.IFF.Definitions;
using PangyaAPI.IFF.StructModels;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

namespace PangyaAPI.IFF.Models
{
    #region Struct Part.iff
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public class Part : IFFCommon
    {
        [field: MarshalAs(UnmanagedType.ByValTStr, SizeConst = 40)]
        public string MPet { get; set; }
        public PartType type_item { get; set; }// o tipo do item, 0, 2 normal, 8 e 9 UCC, 5 acho que é base ou commom Item
        public uint PosMask { get; set; }
        public uint HideMask { get; set; }
        [field: MarshalAs(UnmanagedType.ByValTStr, SizeConst = 40)]
        public string Texture1 { get; set; }
        [field: MarshalAs(UnmanagedType.ByValTStr, SizeConst = 40)]
        public string Texture2 { get; set; }
        [field: MarshalAs(UnmanagedType.ByValTStr, SizeConst = 40)]
        public string Texture3 { get; set; }
        [field: MarshalAs(UnmanagedType.ByValTStr, SizeConst = 40)]
        public string Texture4 { get; set; }
        [field: MarshalAs(UnmanagedType.ByValTStr, SizeConst = 40)]
        public string Texture5 { get; set; }
        [field: MarshalAs(UnmanagedType.ByValTStr, SizeConst = 40)]
        public string Texture6 { get; set; }
        public ushort Power { get; set; }
        public ushort Control { get; set; }
        public ushort Impact { get; set; }
        public ushort Spin { get; set; }
        public ushort Curve { get; set; }
        public ushort PowerSlot { get; set; }
        public ushort ControlSlot { get; set; }
        public ushort ImpactSlot { get; set; }
        public ushort SpinSlot { get; set; }
        public ushort CurveSlot { get; set; }
        [field: MarshalAs(UnmanagedType.ByValArray, SizeConst = 40)]
        byte[] _EquippableWith { get; set; }
        public string EquippableWith
        {
            get => Encoding.GetEncoding("Shift_JIS").GetString(_EquippableWith).Replace("\0", "");
            set => _EquippableWith = Encoding.GetEncoding("Shift_JIS").GetBytes(value.PadRight(64, '\0'));
        }
        public uint SubPart1 { get; set; }
        public uint SubPart2 { get; set; }
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public class CardSlot
        {
            public ushort Slot_Char { get; set; }//Bonus Char Slot
            public ushort Slot_Caddie { get; set; }//Bonus Card Slot
        }
        [field: MarshalAs(UnmanagedType.Struct)]
        public CardSlot _CardSlot { get; set; }
        public uint Points { get; set; }//mastery points?
        public uint RentPang { get; set; }
        public uint Un1 { get; set; }
        public uint EquipmentCategory { get => Convert.ToUInt32(type_item); set => type_item = (PartType)value; }


        public uint newTypeid(uint num, uint serial)
        {
            return Extensions.IFFHandleExtension.GenerateNewTypeID(getPersonagem(), num, 2, EquipmentCategory, serial);
        }
        public uint newTypeid(uint CharacterType, uint Pos, uint Category, uint serial)
        {
            return Extensions.IFFHandleExtension.GenerateNewTypeID(CharacterType, Pos, 2, EquipmentCategory, serial);
        }
    }
    #endregion
}
