using PangyaAPI.Utilities.BinaryModels;
using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using static PangyaAPI.IFF.Extensions.IFFHandleExtension;

namespace PangyaAPI.IFF.StructModels
{
    /// <summary>
    /// Ref:
    ///<see href="https://github.com/Acrisio-Filho/SuperSS-Dev/blob/master/Server%20Lib/Projeto%20IOCP/TYPE/data_iff.h"/>
    ///<code></code>
    /// add news fields:
    /// <see href="https://github.com/retreev/PangLib/tree/master/PangLib.IFF/"/>
    ///<code></code>
    /// update in 30/06/2023 - 10:40 AM by LuisMK
    ///<code></code>
    /// Common data structure found at the head of many IFF datasets
    ///<code></code>
    /// Tamanho 168 bytes
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public partial class IFFCommon : ICloneable //size(168 bytes)
    {
        //------------------- IFF BASIC ----------------------------\\
        public uint Enabled { get; set; }//0 start position
        public uint TypeID { get; set; }//4 start position
        [field: MarshalAs(UnmanagedType.ByValArray, SizeConst = 40)]
        public byte[] NameInBytes { get; set; }//8 start position
        
        [field: MarshalAs(UnmanagedType.Struct, SizeConst = 1)]
        public StLevel Level { get; set; }//72 start position
        [field: MarshalAs(UnmanagedType.ByValTStr, SizeConst = 43)]
        public string Icon { get; set; }//73 start position
        //--------------------------end--------------------------------\\

        //------------------ SHOP DADOS ---------------------------------\\
        [field: MarshalAs(UnmanagedType.Struct, SizeConst = 16)]
        public IFFShopData Shop { get; set; } = new IFFShopData();
        //-------------------  END  ------------------------------\\
        [field: MarshalAs(UnmanagedType.Struct, SizeConst = 24)]
        public IFFTikiShopData tiki { get; set; } = new IFFTikiShopData();
        //-------------------- TIME IFF--------------\\
        [field: MarshalAs(UnmanagedType.Struct, SizeConst = 36)]
        public IFFDate date { get; set; } = new IFFDate();
        public string Name//correcão para não causar conflito ao escrever
        {
            get=> Encoding.GetEncoding("Windows-1252").GetString(NameInBytes).Replace("\0","");            
            set=> NameInBytes = Encoding.GetEncoding("Windows-1252").GetBytes(value.PadRight(40, '\0'));
        }
        /// <summary>
        /// voce pode carregar qualquer iff(que contem o Base)
        /// </summary>
        /// <param name="reader">binario de leitura</param>
        /// <param name="LenghtStr">tamanho do string name</param>
        public void Load(ref PangyaBinaryReader reader, uint LenghtStr)
        {
            //------------------- IFF BASIC ----------------------------\\
            Enabled = reader.ReadUInt32();
            TypeID = reader.ReadUInt32();
            Name = reader.ReadPStr(LenghtStr);
            Level.level = reader.ReadByte(); //49 start position
            Icon = reader.ReadPStr(43); //89 start position
            //--------------------------end--------------------------------\\
            //------------------ SHOP DADOS ---------------------------------\\
            Shop = (IFFShopData)reader.Read(new IFFShopData());
            //-------------------  END  ------------------------------\\
            //------------------ Tiki SHOP---------------------\\
            tiki = (IFFTikiShopData)reader.Read(new IFFTikiShopData());
            //-----------------------------------------------\\

            //-------------------- TIME IFF--------------\\
            date = (IFFDate)reader.Read(new IFFDate());
            //--------------------------------------------------\\
        }
        public uint getPersonagem()
        {
            return _TypeIdValues().CharacterType;
        }
        /// <summary>
        /// Envia uma notificao ao Editor/Dev 
        /// voce não pode listar este item pois o valor ira 
        /// ativar um codigo no ProjectG de alerta
        /// </summary>
        public void SendMessage()
        {
            bool result = Shop.flag_shop.can_send_mail_and_personal_shop
             || Shop.flag_shop.block_mail_and_personal_shop
             || Shop.flag_shop.is_saleable;
            if (result && Shop.ItemPrice >= 1000000)
                MessageBox.Show($"\nBe careful, you activated an item, but did not change its value\n check this item({TypeID})", "Pangya Editor v2", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
        }
        
       
        public string GetItemName()
        {
            return Name;
        }

        public uint GetPrice()
        {
            return Shop.ItemPrice;
        }

        public sbyte GetShopPriceType()
        {
            return (sbyte)Shop.flag_shop.PriceType;
        }

        public bool IsBuyable()
        {
            if (Enabled == 1 && Shop.flag_shop.MoneyFlag == 0 || (int)Shop.flag_shop.MoneyFlag == 1 || (int)Shop.flag_shop.MoneyFlag == 2)
            {
                return true;
            }
            return false;
        }

        public bool IsExist()
        {
            return Convert.ToBoolean(Enabled);
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
        public IFFCommon()
        {
            Name = "[NEW ITEM] by LuisMK";
            Icon = "Icon.tga";
            date = new IFFDate();
            tiki = new IFFTikiShopData();
            Shop = new IFFShopData();
        }
        //conversion this 
        public virtual IFFCommon CreateNewItem()
        {
            Name = "[NEW ITEM] by LuisMK";
            Icon = "[NEW ICON] by LuisMK";
            date = new IFFDate();
            tiki = new IFFTikiShopData();
            Shop = new IFFShopData();
            return this;
        }
        public uint TypeItem()
        {
            return (uint)(int)((TypeID & 0x3fc0000) / Math.Pow(2.0, 18.0));
        }
        public bool IsDupItem()
        {
            return (Enabled == 1 && Shop.flag_shop.IsDuplication);
        }
        public bool IsSellItem()
        {
            return (Enabled == 1 && Shop.flag_shop.is_saleable);
        }

        public bool IsGiftItem()
        {
            // É saleable ou giftable nunca os 2 juntos por que é a flag composta Somente Purchase(compra)
            // então faço o xor nas 2 flag se der o valor de 1 é por que ela é um item que pode presentear
            // Ex: 1 + 1 = 2 Não é
            // Ex: 1 + 0 = 1 OK
            // Ex: 0 + 1 = 1 OK
            // Ex: 0 + 0 = 0 Não é
            byte is_giftable = Convert.ToByte(Shop.flag_shop.IsGift);
            byte _is_saleable = Convert.ToByte(Shop.flag_shop.is_saleable);
            return (Enabled == 1 && Shop.flag_shop.IsTypeCash
                    && (_is_saleable ^ is_giftable) == 1);
        }

        public bool IsOnlyDisplay()
        {
            return (Enabled == 1 && Shop.flag_shop.IsDisplay);
        }

        public bool IsOnlyPurchase()
        {
            return (Enabled == 1 && Shop.flag_shop.is_saleable
                    && Shop.flag_shop.IsGift);
        }

        public bool IsOnlyGift()
        {
            return (Enabled == 1 && Shop.flag_shop.IsTypeCash
                    && Shop.flag_shop.is_saleable && Shop.flag_shop.IsGift == false);
        }

        public bool IsPSQ()
        {
            return (Enabled == 1 && Shop.flag_shop.can_send_mail_and_personal_shop);
        }
      /// <summary>
      /// verifica é pang, cookie ou esta oculto dentro do shopping
      /// </summary>
      /// <returns>0= cookies, 1= pang, 2= hide </returns>
        public int GetTypeCash()
        {
            //se testar o flag do tipo de moeda antes, não vai dar certo
            //tem que testar o flag hide primeiro
            var result = Shop.flag_shop.IsHide ? 2 : Shop.flag_shop.IsTypeCash ? 0 : 1;
            return result;
        }
        public IFFValues _TypeIdValues() => GetTypeIDValues(TypeID);
        public uint GetTypeCard()
        {
            return GetItemSubGroupIdentify22(TypeID);
        }
        
    }
}
