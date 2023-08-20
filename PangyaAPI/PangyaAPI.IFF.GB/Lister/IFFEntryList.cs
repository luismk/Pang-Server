using PangyaAPI.IFF.Extensions;
using PangyaAPI.IFF.Models;
using PangyaAPI.IFF.StructModels;
using PangyaAPI.Utilities.BinaryModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace PangyaAPI.IFF.Lister
{
    /// <summary>
    /// create By LuisMK D:
    /// </summary>
    /// <typeparam name="T">Ex: Mascot.cs</typeparam>
    [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
    public partial class IFFEntryList<T> : List<T>
    {
        /// <summary>
        /// Header IFF(cabeçario do IFF, contem Contagem dos itens existentes no *.iff, ID de ligacao, Versão do IFF 
        /// </summary>
        public IFFHeader Header { get; set; } = new IFFHeader();
        /// <summary>
        /// Atualiza o IFF/Update for IFF
        /// </summary>
        public bool Update { get; set; }
        /// <summary>
        /// class construtor(classe construtura do IFFList)
        /// </summary>
        public IFFEntryList()
        {
            Header = new IFFHeader();
            Update = false;
        }
        /// <summary>
        /// class construtor(classe construtura do IFFList)
        /// </summary>
        /// <param name="path">local onde fica o arquivo/ local</param>
        public IFFEntryList(string path)
        {
            Header = new IFFHeader();
            Update = false;
            Load(File.ReadAllBytes(path));
        }
        public bool CheckVersionIFF(bool check)
        {
            if (Header.CheckMagicNumber())
            {
                throw new Exception($"[IFFEntryList::Error]:" +
                                   $"Version Actual: 13 \n " +
                                   $"Version File: {Header.Version} \n" +
                                   $"Version-incompatible file structure\n");
            }
            else if (check)
            {
                throw new Exception(
                      $"[IFFEntryList::Error]: version-incompatible file structure: ({Marshal.SizeOf((T)Activator.CreateInstance(typeof(T)))})");

            }
            else if (Header.CheckMagicNumber() && check)
            {
                throw new Exception($"[IFFEntryList::Error]:" +
                    $"Version Actual: 13 \n " +
                    $"Version File: {Header.Version} \n" +
                    $"Version-incompatible file structure\n");
            }
            return true;
        }
        public virtual string GetItemName(uint TypeID)
        {
            try
            {
                foreach (var item in this)
                {
                    if (item is IFFCommon)//verifica se é IFFCommon
                    {
                        var item2 = item as IFFCommon;
                        if (item2.TypeID == TypeID)
                        {
                            return item2.Name;
                        }
                    }
                    else
                    {
                        return "";
                    }
                }
            }
            catch { return ""; }
            return "";
        }
        /// <summary>
        ///so obtem se for IFFCommon
        /// </summary>
        /// <param name="TypeID"></param>
        /// <returns>retorna o tipo</returns>
        public T GetItem(uint TypeID)
        {
            foreach (var item in this)
            {
                if (item is IFFCommon)//verifica se é IFFCommon
                {
                    var item2 = item as IFFCommon;
                    if (item2.TypeID == TypeID)
                    {
                        return item;
                    }
                }
                else
                {
                    return CreateItem();
                }
            }
            return CreateItem();
        }
        public IFFCommon GetItemCommon(uint TypeID)
        {
            foreach (var item in this)
            {
                if (item is IFFCommon)//verifica se é IFFCommon
                {
                    var item2 = item as IFFCommon;
                    if (item2.TypeID == TypeID)
                    {
                        return item2;
                    }
                }
                else
                {
                    return new IFFCommon().CreateNewItem();
                }
            }
            return new IFFCommon().CreateNewItem();
        }

        public virtual uint GetPrice(uint TypeID)
        {
            return GetItemCommon(TypeID).Shop.ItemPrice;
        }

        public virtual sbyte GetShopPriceType(uint TypeID)
        {
            return (sbyte)GetItemCommon(TypeID).Shop.flag_shop.PriceType;
        }

        public virtual bool IsBuyable(uint TypeID)
        {
            var item = GetItemCommon(TypeID);
            if (item.Enabled == 1 && item.Shop.flag_shop.MoneyFlag == 0 || (int)item.Shop.flag_shop.MoneyFlag == 1 || (int)item.Shop.flag_shop.MoneyFlag == 2)
            {
                return true;
            }
            return false;
        }

        public virtual bool IsExist(uint TypeID)
        {
            var item = GetItemCommon(TypeID);

            return Convert.ToBoolean(item.Enabled);
        }

        public virtual bool LoadItem(uint ID, ref T item)
        {
            if (!this.TryGetValue(ID, out T value))
            {
                return false;
            }
            item = value;
            return true;
        }

        public virtual bool TryGetValue(uint ID, out T value)
        {
            if (GetItem(ID) != null)
            {
                value = GetItem(ID);
                return true;
            }
            value = CreateItem();
            return false;
        }
        //adiciona e atualiza o cabecario do iff
        public virtual void AddItem(T item)
        {
            var OldCount = Count;
            this.Add(item);
            if (Count > Header.Count)//so atualiza se o  contador for maior
            {
                Header.Count = (short)Count;
                Update = true;
                Debug.WriteLine($"IFFEntryList::AddItemRange: Atualizou o IFF, Contador=> Novo({Count}), Antigo({OldCount}) ");
            }
        }

        public virtual void AddItemRange(IEnumerable<T> item)
        {
            var OldCount = Count;
            this.AddRange(item);
            if (Count > Header.Count)//so atualiza se o  contador for maior
            {
                Header.Count = (short)Count;
                Debug.WriteLine($"IFFEntryList::AddItemRange: Atualizou o IFF, Contador=> Novo({Count}), Antigo({OldCount}) ");
            }
        }
        /// <summary>
        /// parses the *.iff file, if all goes well it should read all data present
        /// </summary>
        /// <param name="data">contains all Information about the *.iff file, size, item count, version, link id</param>
        /// <exception cref="Exception">if I get exception, I must have done something wrong, correct me please?</exception>
        public virtual void Load(byte[] data)
        {
            PangyaBinaryReader Reader = null;

            try
            {
                Reader = new PangyaBinaryReader(new MemoryStream(data));
                Header = Reader.Read(new IFFHeader()) as IFFHeader;
                var size = (Reader.Size - 8L) / Header.Count;
                CheckVersionIFF((Reader.Size - 8L) / Header.Count != Marshal.SizeOf(typeof(T)));
                for (int i = 0; i < Header.Count; i++)
                {
                    //reader object and convert is class IFF
                    var item = (T)Reader.Read(CreateItem());                    
                    //add item in List<T>
                    AddItem(item);
                }
            }
            catch (Exception ex)
            {
                //show log error :(
                MessageBox.Show("["+GetDebuggerDisplay() +"::Error][Log]: \n"+ ex.Message, GetDebuggerDisplay(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                //is dispose memory :D
                Reader.Dispose();
            }
        }
        public T UpdateItem(T model)
        {
            int index;
            index = base.IndexOf(this.FirstOrDefault((T m) => (m as IFFCommon).TypeID == (model as IFFCommon).TypeID));
            base.RemoveAt(index);
            base.Insert(index, model);
            return model;
        }

        public bool Insert(T model)
        {
            if (this.FirstOrDefault((T i) => (i as IFFCommon).TypeID == (model as IFFCommon).TypeID) != null)
            {
                return false;
            }
            base.Insert(base.IndexOf(this.Last((T i) => (i as IFFCommon).TypeID < (model as IFFCommon).TypeID)) + 1, model);
            return true;
        }

        public uint GetNextIdLivreByModel(T model)
        {
            uint findId;
            for (findId = (model as IFFCommon).TypeID; this.Any((T i) => (i as IFFCommon).TypeID == findId); findId++)
            {
            }
            return findId;
        }
        /// <summary>
        /// save list load in iff 
        /// </summary>
        /// <param name="filePath">local file</param>
        public bool Save(string filePath)
        {
            try
            {
                using (PangyaBinaryWriter writer = new PangyaBinaryWriter())
                {
                    writer.WriteStruct(Header);
                    foreach (var entry in this)
                    {
                        writer.WriteStruct(entry);
                    }
                    writer.WriteFile(filePath);
                    Update = false;
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        private string GetDebuggerDisplay()
        {
            var _str = ToString().Replace("PangyaAPI.IFF.Collections.", "");
            return _str;
        }
        protected virtual T CreateItem()
        {
            
            return (T)Activator.CreateInstance(typeof(T));
        }
        ~IFFEntryList()
        {
            Debug.WriteLine("Destruiu a class IFFList!");
        }
    }
}