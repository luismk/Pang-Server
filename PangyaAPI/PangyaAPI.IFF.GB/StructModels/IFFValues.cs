using PangyaAPI.IFF.Definitions;
using System;
using System.Runtime.InteropServices;
using System.Collections;

namespace PangyaAPI.IFF.StructModels
{
    [Serializable, StructLayout(LayoutKind.Sequential)]
    public class IFFValues
    {
        public uint CharacterType;
        public ushort Character_Raw;
        public ushort Group;
        public ushort Type;
        public ushort Pos;
        public ushort Serial;
    }
    [Serializable, StructLayout(LayoutKind.Sequential)]
    public class _stFlagShop
    {
        public bool IsTypeCash;//is_cash
        public bool IsReserve;//can_send_mail_and_personal_shop(vende no psq e envia no email)
        public bool IsDuplication;//can_dup
        public bool IsSpecial;
        public bool IsNew;//block_mail_and_personal_shop
        public bool IsHot;//is_saleable
        public bool IsGift;//is_giftable
        public bool IsDisplay;//Shop availability set to 'available'
        public bool IsHide
        {
            get
            {
                return IsReserve == false
                   & IsDuplication == false & IsSpecial == false & IsNew == false
                   & IsHot == false & IsGift == false & IsDisplay == false;
            }
            set
            {
                var v = value;
                if (v)
                {
                    IsReserve = false;
                    IsDuplication = false;
                    IsSpecial = false;
                    IsNew = false;
                    IsHot = false;
                    IsGift = false;
                    IsDisplay = false;
                }
                else
                {
                    IsReserve = false;
                    IsDuplication = false;
                    IsSpecial = false;
                    IsNew = false;
                    IsHot = false;
                    IsGift = false;
                    IsDisplay = false;
                }
            }
        }
        public bool IsNormal
        {
            get
            {
                return (IsReserve == true
                   && IsDuplication == false && IsSpecial == false && IsNew == false
                   && IsHot == false && IsGift == false && IsDisplay == false);
            }
        }
        public short value;
        public _stFlagShop()
        {
            BitArray bits = new BitArray(BitConverter.GetBytes(0));
            bits = PadToFullByte(bits);



            //IsTypeCash = bits.Get(0);//
            //IsReserve = bits.Get(1);
            //IsDuplication = bits.Get(2);
            //IsSpecial = bits.Get(3);//special
            //IsNew = bits.Get(4);//new?
            //IsHot = bits.Get(5); // Pang(Only Purchase; CP Gift and Purchase) (is_saleable e is_giftable vira flag de só Purchase CP ou Pang)
            //IsGift = bits.Get(6); // CP só pode ser presenteado (is_saleable e is_giftable vira flag de só Purchase CP ou Pang)
            //IsDisplay = bits.Get(7);    // Apenas Display no shop
        }
        public _stFlagShop(short PriceType)
        {
            value = PriceType;
            BitArray bits = new BitArray(BitConverter.GetBytes(PriceType));
            bits = PadToFullByte(bits);
            //IsTypeCash = (value & 0x1) == 0x1;
            //IsGift = ((value & 0x2) == 0x2);
            //IsSpecial = ((value & 0x4) == 0x4);
            //IsNew = ((value & 0x8) == 0x8);
            //IsHot = ((value & 0x10) == 0x10);

            IsTypeCash = bits.Get(0);//
            IsReserve = bits.Get(1);
            IsDuplication = bits.Get(2);
            IsSpecial = bits.Get(3);//special
            IsNew = bits.Get(4);//new?
            IsHot = bits.Get(5); // Pang(Only Purchase; CP Gift and Purchase) (is_saleable e is_giftable vira flag de só Purchase CP ou Pang)
            IsGift = bits.Get(6); // CP só pode ser presenteado (is_saleable e is_giftable vira flag de só Purchase CP ou Pang)
            IsDisplay = bits.Get(7);    // Apenas Display no shop	          
        }

        public _stFlagShop setFlag()
        {

            BitArray bits = new BitArray(8, false);

            if (IsTypeCash)
            {
                bits.Set(0, true);
            }
            if (IsReserve)
            {
                bits.Set(1, true);
            }
            if (IsDuplication)
            {
                bits.Set(2, true);
            }
            if (IsSpecial)
            {
                bits.Set(3, true);
            }
            if (IsNew)
            {
                bits.Set(4, true);
            }
            if (IsHot)
            {
                bits.Set(5, true);
            }
            if (IsGift)
            {
                bits.Set(6, true);
            }
            if (IsDisplay)
            {
                bits.Set(7, true);
            }
            if (IsHide)
            {
                for (int i = 0; i < 8; i++)
                {
                    bits.Set(i, false);
                }
            }
            value = ConvertToByte(bits);
            return new _stFlagShop(value);
        }
        public bool SetTypeCash(bool CpOrPang)
        {
            BitArray bits = new BitArray(1, false);
            IsTypeCash = CpOrPang;
            bits.Set(0, CpOrPang);
            return CpOrPang;
        }

        public byte GetTypeCash()
        {
            return Convert.ToByte(IsTypeCash == true ? 1 : 0);
        }

        BitArray PadToFullByte(BitArray bits)
        {
            BitArray array = new BitArray(8, false);
            if (bits.Count > 0)
            {
                for (int i = 0; i < bits.Count; i++)
                {
                    if ((bits.Count > 8) && (i < 8))
                    {
                        array.Set(i, bits[i]);
                    }
                }
            }
            return array;
        }

        byte ConvertToByte(BitArray bits)
        {
            byte[] array = new byte[1];
            bits.CopyTo(array, 0);
            return array[0];
        }
    }
    public class TestFlags
    {
        /// <summary>
        /// true = cash
        /// </summary>
        public bool IsTypeCash;//is_cash
        /// <summary>
        /// //can_send_mail_and_personal_shop(vende no psq e envia no email)
        /// </summary>
        public bool IsReserve;
        /// <summary>
        /// //can_dup
        /// </summary>
        public bool IsDuplication;
        public bool IsSpecial;
        /// <summary>
        /// //block_mail_and_personal_shop
        /// </summary>
        public bool IsNew;
        /// <summary>
        /// //is_saleable
        /// </summary>
        public bool IsHot;
        /// <summary>
        /// is_giftable
        /// </summary>
        public bool IsGift;
        /// <summary>
        ///Shop availability set to 'available'
        /// </summary>
        public bool IsDisplay;
        public byte _Flag { get; set; }
    }
    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 1)]
    public class StLevel
    {
        private ItemLevelEnum _level;//ler somente esse ;D

        public bool GoodLevel(byte _stlevel)
        {
            if (is_max && _stlevel <= level)
                return true;
            else if (!is_max && _stlevel >= level)
                return true;

            return false;
        }

        public byte level
        {
            get
            {
                return (byte)_level;
            }
            set
            {
                _level =(ItemLevelEnum)value;
            }
        }
        public bool is_max
        {
            get {
                bool _is_max = false;
                BitArray bits = new BitArray(BitConverter.GetBytes(level));
                bits = PadToFullByte(bits);
                if (bits.Get(7))
                {
                    _is_max = true;
                    bits.Set(7, false);
                }
                else
                {
                    _is_max = false;
                }
                return _is_max;
            }
        }

        BitArray PadToFullByte(BitArray bits)
        {
            BitArray array = new BitArray(8, false);
            if (bits.Count > 0)
            {
                for (int i = 0; i < bits.Count; i++)
                {
                    if ((bits.Count > 8) && (i < 8))
                    {
                        array.Set(i, bits[i]);
                    }
                }
            }
            return array;
        }

        public static explicit operator int(StLevel v)
        {
          return  v.level;
        }
    }
}
