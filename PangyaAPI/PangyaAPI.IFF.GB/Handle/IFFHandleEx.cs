using PangyaAPI.IFF.Definitions;
using System;
using PangyaAPI.IFF.StructModels;
using static PangyaAPI.IFF.Extensions.IFFHandleExtension;
using System.IO;
using System.Diagnostics;
using System.Linq;

namespace PangyaAPI.IFF.Handle
{
    public partial class IFFHandle
    {
        #region Methods IFF
        public void LoadOne(string filepath)
        {
            FileName = filepath;
            var load = File.ReadAllBytes(FileName);

            string fileNameWithoutExtension = Path.GetFileName(filepath);

            switch (fileNameWithoutExtension)
            {
                case "Character.iff":
                    Character.Load(load);
                    break;
                case "Achievement.iff":
                    Achievement.Load(load);
                    break;
                case "QuestItem.iff":
                    QuestItem.Load(load);
                    break;
                case "QuestStuff.iff":
                    QuestStuff.Load(load);
                    break;
                case "Part.iff":
                    Part.Load(load);
                    break;
                case "Ball.iff":
                    Ball.Load(load);
                    break;
                case "Card.iff":
                    Card.Load(load);
                    break;
                case "Item.iff":
                    Item.Load(load);
                    break;
                case "SetItem.iff":
                    SetItem.Load(load);
                    break;
                case "HairStyle.iff":
                    HairStyle.Load(load);
                    break;
                case "Club.iff":
                    Club.Load(load);
                    break;
                case "ClubSet.iff":
                    ClubSet.Load(load);
                    break;
                case "Caddie.iff":
                    Caddie.Load(load);
                    break;
                case "Skin.iff":
                    Skin.Load(load);
                    break;
                case "CaddieItem.iff":
                    CaddieItem.Load(load);
                    break;
                case "Mascot.iff":
                    Mascot.Load(load);
                    break;
                case "CutinInfomation.iff":
                    CutinInformation.Load(load);
                    break;
                case "GrandPrixData.iff":
                    GrandPrixData.Load(load);
                    break;
                case "LevelUpPrizeItem.iff":
                    LevelUpPrizeItem.Load(load);
                    break;
                case "GrandPrixSpecialHole.iff":
                    GrandPrixSpecialHole.Load(load);
                    break;
                case "GrandPrixRankReward.iff":
                    GrandPrixRankReward.Load(load);
                    break;
                case "MemorialShopRareItem.iff":
                    MemorialShopItemRare.Load(load);
                    break;
                case "MemorialShopCoinItem.sff":
                case "MemorialShopCoinItem.iff":
                    MemorialShopCoinItem.Load(load);
                    break;
                case "CadieMagicBox.iff":
                    CadieMagicBox.Load(load);
                    break;
                case "CadieMagicBoxRandom.iff":
                    CadieMagicBoxRandom.Load(load);
                    break;
                case "AuxPart.iff":
                    AuxPart.Load(load);
                    break;
                case "Desc.iff":
                    Desc.Load(load);
                    break;
                case "Furniture.iff":
                    Furniture.Load(load);
                    break;
                case "FurnitureAbility.iff":
                    FurnitureAbility.Load(load);
                    break;
                case "TikiSpecialTable.iff":
                    TikiSpecialTable.Load(load);
                    break;
                case "TikiRecipe.iff":
                    TikiRecipe.Load(load);
                    break;
                case "TikiPointTable.iff":
                    TikiPointTable.Load(load);
                    break;
                case "Match.iff":
                    Match.Load(load);
                    break;
                case "Enchant.iff":
                    Enchant.Load(load);
                    break;
                case "SetEffectTable.iff":
                    SetEffectTable.Load(load);
                    break;
                case "Course.iff":
                    Course.Load(load);
                    break;
                case "Ability.iff":
                    Ability.Load(load);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Load and parse data from IFF file and save to each collector object		
        /// </summary>
        /// <param name="filepath">file name next to where it is</param>
        public void LoadAll(string filepath)
        {
            FileName = filepath;
            Zip.LoadFile(FileName);

            if (Zip.CheckFile()[0] == 80 && Zip.CheckFile()[1] == 75)
            {
                Character.Load(Zip.Reader("Character.iff"));
                Achievement.Load(Zip.Reader("Achievement.iff"));
                QuestItem.Load(Zip.Reader("QuestItem.iff"));
                QuestStuff.Load(Zip.Reader("QuestStuff.iff"));
                Part.Load(Zip.Reader("Part.iff"));
                Ball.Load(Zip.Reader("Ball.iff"));
                Card.Load(Zip.Reader("Card.iff"));
                Item.Load(Zip.Reader("Item.iff"));
                SetItem.Load(Zip.Reader("SetItem.iff"));
                HairStyle.Load(Zip.Reader("HairStyle.iff"));
                Club.Load(Zip.Reader("Club.iff"));
                ClubSet.Load(Zip.Reader("ClubSet.iff"));
                Caddie.Load(Zip.Reader("Caddie.iff"));
                Skin.Load(Zip.Reader("Skin.iff"));
                CaddieItem.Load(Zip.Reader("CaddieItem.iff"));
                Mascot.Load(Zip.Reader("Mascot.iff"));
                CutinInformation.Load(Zip.Reader("CutinInfomation.iff"));
                GrandPrixData.Load(Zip.Reader("GrandPrixData.iff"));
                LevelUpPrizeItem.Load(Zip.Reader("LevelUpPrizeItem.iff"));
                GrandPrixSpecialHole.Load(Zip.Reader("GrandPrixSpecialHole.iff"));
                GrandPrixRankReward.Load(Zip.Reader("GrandPrixRankReward.iff"));
                MemorialShopItemRare.Load(Zip.Reader("MemorialShopRareItem.iff"));
                MemorialShopCoinItem.Load(Zip.Reader("MemorialShopCoinItem.sff", "MemorialShopCoinItem.iff"));
                CadieMagicBox.Load(Zip.Reader("CadieMagicBox.iff", "CadieMagicBox.iff/"));
                CadieMagicBoxRandom.Load(Zip.Reader("CadieMagicBoxRandom.iff"));
                AuxPart.Load(Zip.Reader("AuxPart.iff"));
                Desc.Load(Zip.Reader("Desc.iff"));
                Furniture.Load(Zip.Reader("Furniture.iff"));
                FurnitureAbility.Load(Zip.Reader("FurnitureAbility.iff"));
                TikiSpecialTable.Load(Zip.Reader("TikiSpecialTable.iff"));
                TikiRecipe.Load(Zip.Reader("TikiRecipe.iff"));
                TikiPointTable.Load(Zip.Reader("TikiPointTable.iff"));
                Match.Load(Zip.Reader("Match.iff"));
                Enchant.Load(Zip.Reader("Enchant.iff"));
                SetEffectTable.Load(Zip.Reader("SetEffectTable.iff"));
                Course.Load(Zip.Reader("Course.iff"));
                Ability.Load(Zip.Reader("Ability.iff"));
                Log();
            }
        }

        /// <summary>
        /// Reaload and parse data from IFF file and save to each collector object		
        /// </summary>
        public void ReloadIff()
        {
            Zip.ReadLoadFile();
            Ability.Clear();

            Character.Clear();//("Character.iff"));
            Achievement.Clear();//("Achievement.iff"));
            Part.Clear();//("Part.iff"));
            Ball.Clear();//("Ball.iff"));
            Card.Clear();//("Card.iff"));
            Item.Clear();//("Item.iff"));
            SetItem.Clear();//("SetItem.iff"));
            HairStyle.Clear();//("HairStyle.iff"));
            Club.Clear();//("Club.iff"));
            ClubSet.Clear();//("ClubSet.iff"));
            Caddie.Clear();//("Caddie.iff"));
            Skin.Clear();//("Skin.iff"));
            CaddieItem.Clear();//("CaddieItem.iff"));
            Mascot.Clear();//("Mascot.iff"));
            CutinInformation.Clear();//("CutinInfomation.iff"));
            GrandPrixData.Clear();//("GrandPrixData.iff"));
            LevelUpPrizeItem.Clear();//("LevelUpPrizeItem.iff"));
            GrandPrixSpecialHole.Clear();//("GrandPrixSpecialHole.iff"));
            GrandPrixRankReward.Clear();//("GrandPrixRankReward.iff"));
            MemorialShopItemRare.Clear();//("MemorialShopRareItem.iff"));
            MemorialShopCoinItem.Clear();//("MemorialShopCoinItem.sff", "MemorialShopCoinItem.iff"));
            CadieMagicBox.Clear();//("CadieMagicBox.iff"));
            AuxPart.Clear();//("AuxPart.iff"));
            Desc.Clear();//("Desc.iff"));

            Ability.Load(Zip.Reader("Ability.iff"));

            Character.Load(Zip.Reader("Character.iff"));
            Achievement.Load(Zip.Reader("Achievement.iff"));
            Part.Load(Zip.Reader("Part.iff"));
            Ball.Load(Zip.Reader("Ball.iff"));
            Card.Load(Zip.Reader("Card.iff"));
            Item.Load(Zip.Reader("Item.iff"));
            SetItem.Load(Zip.Reader("SetItem.iff"));
            HairStyle.Load(Zip.Reader("HairStyle.iff"));
            Club.Load(Zip.Reader("Club.iff"));
            ClubSet.Load(Zip.Reader("ClubSet.iff"));
            Caddie.Load(Zip.Reader("Caddie.iff"));
            Skin.Load(Zip.Reader("Skin.iff"));
            CaddieItem.Load(Zip.Reader("CaddieItem.iff"));
            Mascot.Load(Zip.Reader("Mascot.iff"));
            CutinInformation.Load(Zip.Reader("CutinInfomation.iff"));
            GrandPrixData.Load(Zip.Reader("GrandPrixData.iff"));
            LevelUpPrizeItem.Load(Zip.Reader("LevelUpPrizeItem.iff"));
            GrandPrixSpecialHole.Load(Zip.Reader("GrandPrixSpecialHole.iff"));
            GrandPrixRankReward.Load(Zip.Reader("GrandPrixRankReward.iff"));
            MemorialShopItemRare.Load(Zip.Reader("MemorialShopRareItem.iff"));
            MemorialShopCoinItem.Load(Zip.Reader("MemorialShopCoinItem.sff", "MemorialShopCoinItem.iff"));
            CadieMagicBox.Load(Zip.Reader("CadieMagicBox.iff", "CadieMagicBox.iff"));
            AuxPart.Load(Zip.Reader("AuxPart.iff"));
            Desc.Load(Zip.Reader("Desc.iff"));
            Log();
        }
        public IFFCommon GetItemCommon(uint TypeID)
        {
            var item = new IFFCommon().CreateNewItem();
            switch (GetItemGroup(TypeID))
            {
                case IFFGROUP.ITEM_TYPE_CHARACTER:
                    {
                        item = Character.GetItemCommon(TypeID);
                    }
                    break;
                case IFFGROUP.ITEM_TYPE_HAIR_STYLE:
                    {
                        item = HairStyle.GetItemCommon(TypeID);
                    }
                    break;
                case IFFGROUP.ITEM_TYPE_PART:
                    {
                        item = Part.GetItemCommon(TypeID);
                    }
                    break;
                case IFFGROUP.ITEM_TYPE_CLUB:
                    {
                        item = ClubSet.GetItemCommon(TypeID);
                    }
                    break;
                case IFFGROUP.ITEM_TYPE_BALL:
                    {
                        item = Ball.GetItemCommon(TypeID);
                    }
                    break;
                case IFFGROUP.ITEM_TYPE_USE:
                    {
                        item = Item.GetItemCommon(TypeID);
                    }
                    break;
                case IFFGROUP.ITEM_TYPE_CADDIE:
                    {
                        item = Caddie.GetItemCommon(TypeID);
                    }
                    break;
                case IFFGROUP.ITEM_TYPE_CADDIE_ITEM:
                    {
                        item = CaddieItem.GetItemCommon(TypeID);
                    }
                    break;
                case IFFGROUP.ITEM_TYPE_SETITEM:
                    {
                        item = SetItem.GetItemCommon(TypeID);
                    }
                    break;
                case IFFGROUP.ITEM_TYPE_SKIN:
                    {
                        item = Skin.GetItemCommon(TypeID);
                    }
                    break;
                case IFFGROUP.ITEM_TYPE_MASCOT:
                    {
                        item = Mascot.GetItemCommon(TypeID);
                    }
                    break;
                case IFFGROUP.ITEM_TYPE_CARD:
                    {
                        item = Card.GetItemCommon(TypeID);
                    }
                    break;
                case IFFGROUP.ITEM_TYPE_AUX:
                    {
                        item = AuxPart.GetItemCommon(TypeID);
                    }
                    break;
                default:
                    {
                        var result = (uint)Math.Round((TypeID & 0xFC000000) / Math.Pow(2.0, 26.0));

                        Debug.WriteLine($"ItemGroup_Un -> {GetItemGroup(TypeID)}");
                    }
                    break;
            }
            return item;
        }

        //public uint GetRealQuantity(uint TypeId, uint Qty)
        //{
        //    switch (GetItemGroup(TypeId))
        //    {
        //        case IFFGROUP.ITEM_TYPE_USE:
        //            return Item.GetRealQuantity(TypeId, Qty);
        //        case IFFGROUP.ITEM_TYPE_BALL:
        //            return Ball.GetRealQuantity(TypeId, Qty);
        //    }
        //    return Qty;
        //}

        public uint GetRentalPrice(uint TypeID)
        {
            if (!(GetItemGroup(TypeID) == IFFGROUP.ITEM_TYPE_PART))
            {
                return 0;
            }
            return Part.GetRentalPrice(TypeID);
        }
        public string GetItemName(uint TypeId)
        {
            try
            {
                switch (GetItemGroup(TypeId))
                {
                    case IFFGROUP.ITEM_TYPE_CHARACTER:
                        return Character.GetItemName(TypeId);

                    case IFFGROUP.ITEM_TYPE_PART:
                        //Part
                        return Part.GetItemName(TypeId);
                    case IFFGROUP.ITEM_TYPE_HAIR_STYLE:
                        {
                            return HairStyle.GetItemName(TypeId);
                        }
                    case IFFGROUP.ITEM_TYPE_CLUB:
                        return ClubSet.GetItemName(TypeId);

                    case IFFGROUP.ITEM_TYPE_BALL:
                        // Ball
                        return Ball.GetItemName(TypeId);

                    case IFFGROUP.ITEM_TYPE_USE:
                        // Normal Item
                        return Item.GetItemName(TypeId);

                    case IFFGROUP.ITEM_TYPE_CADDIE:
                        // Cadie
                        return Caddie.GetItemName(TypeId);

                    case IFFGROUP.ITEM_TYPE_CADDIE_ITEM:
                        return CaddieItem.GetItemName(TypeId);

                    case IFFGROUP.ITEM_TYPE_SETITEM:
                        // Part
                        return SetItem.GetItemName(TypeId);

                    case IFFGROUP.ITEM_TYPE_SKIN:
                        return Skin.GetItemName(TypeId);

                    case IFFGROUP.ITEM_TYPE_MASCOT:
                        return Mascot.GetItemName(TypeId);

                    case IFFGROUP.ITEM_TYPE_CARD:
                        return Card.GetItemName(TypeId);

                    case IFFGROUP.ITEM_TYPE_AUX:
                        return AuxPart.GetItemName(TypeId);

                }
                return "Unknown Item Name";
            }
            catch (Exception)
            {

                return "Unknown Item Name";
            }
        }

        public byte GetItemdias(uint TypeId, uint Day)
        {
            switch (GetItemGroup(TypeId))
            {
                case IFFGROUP.ITEM_TYPE_CADDIE:
                    if (Caddie.GetSalary(TypeId) > 0)
                    {
                        return 2;
                    }
                    return 0;
                case IFFGROUP.ITEM_TYPE_MASCOT:
                    if (Mascot.GetSalary(TypeId, Day) > 0)
                    {
                        return 2;
                    }
                    return 0;
                case IFFGROUP.ITEM_TYPE_SKIN:
                    // SKIN FLAG
                    return Skin.GetSkinFlag(TypeId);
                default:
                    return 0;
            }
        }

        public uint GetPrice(uint TypeID, uint ADay)
        {
            switch (GetItemGroup(TypeID))
            {
                case IFFGROUP.ITEM_TYPE_BALL:
                    return Ball.GetPrice(TypeID);

                case IFFGROUP.ITEM_TYPE_CLUB:
                    return ClubSet.GetPrice(TypeID);

                case IFFGROUP.ITEM_TYPE_CHARACTER:
                    return Character.GetPrice(TypeID);

                case IFFGROUP.ITEM_TYPE_PART:
                    return Part.GetPrice(TypeID);

                case IFFGROUP.ITEM_TYPE_HAIR_STYLE:
                    return HairStyle.GetPrice(TypeID);

                case IFFGROUP.ITEM_TYPE_USE:
                    return Item.GetPrice(TypeID);

                case IFFGROUP.ITEM_TYPE_CADDIE:
                    return Caddie.GetPrice(TypeID);

                case IFFGROUP.ITEM_TYPE_CADDIE_ITEM:
                    return CaddieItem.GetPrice(TypeID, ADay);

                case IFFGROUP.ITEM_TYPE_SETITEM:
                    return SetItem.GetPrice(TypeID);

                case IFFGROUP.ITEM_TYPE_SKIN:
                    return Skin.GetPrice(TypeID, ADay);

                case IFFGROUP.ITEM_TYPE_MASCOT:
                    return Mascot.GetPrice(TypeID, ADay);

                case IFFGROUP.ITEM_TYPE_CARD:
                    return Card.GetPrice(TypeID);

            }
            return 0;
        }

        //public string GetSetItemStr(uint TypeId)
        //{
        //    if (!(GetItemGroup(TypeId) == IFFGROUP.ITEM_TYPE_SETITEM))
        //    {
        //        return "";
        //    }
        //    return SetItem.GetSetItemStr(TypeId);
        //}

        public sbyte GetShopPriceType(uint TypeID)
        {
            switch (GetItemGroup(TypeID))
            {
                case IFFGROUP.ITEM_TYPE_BALL:
                    return Ball.GetShopPriceType(TypeID);

                case IFFGROUP.ITEM_TYPE_CLUB:
                    return ClubSet.GetShopPriceType(TypeID);

                case IFFGROUP.ITEM_TYPE_CHARACTER:
                    return Character.GetShopPriceType(TypeID);

                case IFFGROUP.ITEM_TYPE_PART:
                    return Part.GetShopPriceType(TypeID);

                case IFFGROUP.ITEM_TYPE_HAIR_STYLE:
                    return HairStyle.GetShopPriceType(TypeID);

                case IFFGROUP.ITEM_TYPE_USE:
                    return Item.GetShopPriceType(TypeID);

                case IFFGROUP.ITEM_TYPE_CADDIE:
                    return Caddie.GetShopPriceType(TypeID);

                case IFFGROUP.ITEM_TYPE_CADDIE_ITEM:
                    return CaddieItem.GetShopPriceType(TypeID);

                case IFFGROUP.ITEM_TYPE_SETITEM:
                    return SetItem.GetShopPriceType(TypeID);

                case IFFGROUP.ITEM_TYPE_SKIN:
                    return Skin.GetShopPriceType(TypeID);

                case IFFGROUP.ITEM_TYPE_MASCOT:
                    return Mascot.GetShopPriceType(TypeID);

                case IFFGROUP.ITEM_TYPE_CARD:
                    return Card.GetShopPriceType(TypeID);

            }
            return 0;
        }


        public bool IsBuyable(uint TypeID)
        {
            switch (GetItemGroup(TypeID))
            {
                case IFFGROUP.ITEM_TYPE_BALL:
                    {
                        return Ball.IsBuyable(TypeID);
                    }
                case IFFGROUP.ITEM_TYPE_CLUB:
                    {
                        return ClubSet.IsBuyable(TypeID);
                    }
                case IFFGROUP.ITEM_TYPE_CHARACTER:
                    {
                        return Character.IsBuyable(TypeID);
                    }
                case IFFGROUP.ITEM_TYPE_PART:
                    {
                        return Part.IsBuyable(TypeID);
                    }
                case IFFGROUP.ITEM_TYPE_HAIR_STYLE:
                    {
                        return HairStyle.IsBuyable(TypeID);
                    }
                case IFFGROUP.ITEM_TYPE_USE:
                    {
                        return Item.IsBuyable(TypeID);
                    }
                case IFFGROUP.ITEM_TYPE_CADDIE:
                    {
                        return Caddie.IsBuyable(TypeID);
                    }
                case IFFGROUP.ITEM_TYPE_CADDIE_ITEM:
                    {
                        return CaddieItem.IsBuyable(TypeID);
                    }
                case IFFGROUP.ITEM_TYPE_SETITEM:
                    {
                        return SetItem.IsBuyable(TypeID);
                    }
                case IFFGROUP.ITEM_TYPE_SKIN:
                    {
                        return Skin.IsBuyable(TypeID);
                    }
                case IFFGROUP.ITEM_TYPE_MASCOT:
                    {
                        return Mascot.IsBuyable(TypeID);
                    }
                case IFFGROUP.ITEM_TYPE_CARD:
                    {
                        return Card.IsBuyable(TypeID);
                    }

            }
            return false;
        }
        public bool CheckCaddieBox(uint TypeID)
        {
            try
            {
                switch (GetItemGroup(TypeID))
                {
                    case IFFGROUP.ITEM_TYPE_CLUB:
                    case IFFGROUP.ITEM_TYPE_PART:
                    //  Part
                    case IFFGROUP.ITEM_TYPE_BALL:
                    //  Ball
                    case IFFGROUP.ITEM_TYPE_CADDIE:
                    case IFFGROUP.ITEM_TYPE_SETITEM:
                    case IFFGROUP.ITEM_TYPE_MASCOT:
                    case IFFGROUP.ITEM_TYPE_CARD:
                    case IFFGROUP.ITEM_TYPE_AUX:
                        return true;
                    default:
                        return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool IsExist(uint TypeID)
        {
            try
            {
                switch (GetItemGroup(TypeID))
                {
                    case IFFGROUP.ITEM_TYPE_CLUB:
                        return ClubSet.IsExist(TypeID);

                    case IFFGROUP.ITEM_TYPE_CHARACTER:
                        return Character.IsExist(TypeID);

                    case IFFGROUP.ITEM_TYPE_PART:
                        //  Part
                        return Part.IsExist(TypeID);
                    //Hair
                    case IFFGROUP.ITEM_TYPE_HAIR_STYLE:
                        return HairStyle.IsExist(TypeID);

                    case IFFGROUP.ITEM_TYPE_BALL:
                        //  Ball
                        return Ball.IsExist(TypeID);

                    case IFFGROUP.ITEM_TYPE_USE:
                        // Normal Item
                        return Item.IsExist(TypeID);

                    case IFFGROUP.ITEM_TYPE_CADDIE:
                        return Caddie.IsExist(TypeID);

                    case IFFGROUP.ITEM_TYPE_CADDIE_ITEM:
                        return CaddieItem.IsExist(TypeID);

                    case IFFGROUP.ITEM_TYPE_SETITEM:
                        return SetItem.IsExist(TypeID);

                    case IFFGROUP.ITEM_TYPE_SKIN:
                        return Skin.IsExist(TypeID);

                    case IFFGROUP.ITEM_TYPE_MASCOT:
                        return Mascot.IsExist(TypeID);

                    case IFFGROUP.ITEM_TYPE_CARD:
                        return Card.IsExist(TypeID);

                    case IFFGROUP.ITEM_TYPE_AUX:
                        return AuxPart.IsExist(TypeID);

                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public IFFTime GetIFFTime(DateTime now)
        {
            return new IFFTime(now);
        }
        public IFFCommon GetBase(uint TypeID)
        {
            var data = new IFFCommon().CreateNewItem();
            try
            {
                switch (GetItemGroup(TypeID))
                {
                    case IFFGROUP.ITEM_TYPE_CLUB:
                        return ClubSet.Find(c => c.TypeID == TypeID);

                    case IFFGROUP.ITEM_TYPE_CHARACTER:
                        return Character.Find(c => c.TypeID == TypeID);

                    case IFFGROUP.ITEM_TYPE_PART:
                        //  Part
                        return Part.Find(c => c.TypeID == TypeID);
                    //Hair
                    case IFFGROUP.ITEM_TYPE_HAIR_STYLE:
                        return HairStyle.Find(c => c.TypeID == TypeID);

                    case IFFGROUP.ITEM_TYPE_BALL:
                        //  Ball
                        return Ball.Find(c => c.TypeID == TypeID);

                    case IFFGROUP.ITEM_TYPE_USE:
                        // Normal Item
                        return Item.Find(c => c.TypeID == TypeID);

                    case IFFGROUP.ITEM_TYPE_CADDIE:
                        return Caddie.Find(c => c.TypeID == TypeID);

                    case IFFGROUP.ITEM_TYPE_CADDIE_ITEM:
                        return CaddieItem.Find(c => c.TypeID == TypeID);

                    case IFFGROUP.ITEM_TYPE_SETITEM:
                        return SetItem.Find(c => c.TypeID == TypeID);

                    case IFFGROUP.ITEM_TYPE_SKIN:
                        return Skin.Find(c => c.TypeID == TypeID);

                    case IFFGROUP.ITEM_TYPE_MASCOT:
                        return Mascot.Find(c => c.TypeID == TypeID);

                    case IFFGROUP.ITEM_TYPE_CARD:
                        return Card.Find(c => c.TypeID == TypeID);

                    case IFFGROUP.ITEM_TYPE_AUX:
                        return AuxPart.Find(c => c.TypeID == TypeID);

                }
                return data;
            }
            catch (Exception)
            {
                return data;
            }
        }



        public void Log()
        {
            #if _RELEASE 
            Debug.WriteLine("[Part.iff] => Load Files: {0}", Part.Count);
            Debug.WriteLine("[Card.iff] => Load Files: {0}", Card.Count);
            Debug.WriteLine("[Ball.iff] => Load Files: {0}", Ball.Count);
            Debug.WriteLine("[Caddie.iff] => Load Files: {0}", Caddie.Count);
            Debug.WriteLine("[Skin.iff] => Load Files: {0}", Skin.Count);
            Debug.WriteLine("[SetItem.iff] => Load Files: {0}", SetItem.Count);
            Debug.WriteLine("[Item.iff] => Load Files: {0}", Item.Count);
            Debug.WriteLine("[CutinInformation.iff] => Load Files: {0}", CutinInformation.Count);
            Debug.WriteLine("[HairStyle.iff] => Load Files: {0}", HairStyle.Count);
            Debug.WriteLine("[CaddieItem.iff] => Load Files: {0}", CaddieItem.Count);
            Debug.WriteLine("[Mascot.iff] => Load Files: {0}", Mascot.Count);
            Debug.WriteLine("[GrandPrixData.iff] => Load Files: {0}", GrandPrixData.Count);
            Debug.WriteLine("[ClubSet.iff] => Load Files: {0}", ClubSet.Count);
            Debug.WriteLine("[Club.iff] => Load Files: {0}", Club.Count);
            Debug.WriteLine("[LevelUpPrizeItem.iff] => Load Files: {0}", LevelUpPrizeItem.Count);
            Debug.WriteLine("[Character.iff] => Load Files: {0}", Character.Count);
            Debug.WriteLine("[GrandPrixSpecialHole.iff] => Load Files: {0}", GrandPrixSpecialHole.Count);
            Debug.WriteLine("[GrandPrixRankReward.iff] => Load Files: {0}", GrandPrixRankReward.Count);
            Debug.WriteLine("[MemorialShopCoinItem.iff] => Load Files: {0}", MemorialShopCoinItem.Count);
            Debug.WriteLine("[MemorialShopItemRare.iff] => Load Files: {0}", MemorialShopItemRare.Count);
            Debug.WriteLine("[CadieMagicBox.iff] => Load Files: {0}", CadieMagicBox.Count);
            Debug.WriteLine("[CadieMagicBoxRandom.iff] => Load Files: {0}", CadieMagicBoxRandom.Count);
            Debug.WriteLine("[AuxPart.iff] => Load Files: {0}", AuxPart.Count);
            Debug.WriteLine("[Achievement.iff] => Load Files: {0}", Achievement.Count);
            Debug.WriteLine("[QuestItem.iff] => Load Files: {0}", QuestItem.Count);
            Debug.WriteLine("[QuestStuff.iff] => Load Files: {0}", QuestStuff.Count);
            Debug.WriteLine("[Furniture.iff] => Load Files: {0}", Furniture.Count);
            Debug.WriteLine("[FurnitureAbility.iff] => Load Files: {0}",FurnitureAbility .Count);
            Debug.WriteLine("[Course.iff] => Load Files: {0}", Course.Count);
            Debug.WriteLine("[CadieMagicBoxRandom.iff] => Load Files: {0}", CadieMagicBoxRandom.Count);
            Debug.WriteLine("[TikiSpecialTable.iff] => Load Files: {0}", TikiSpecialTable.Count);
            Debug.WriteLine("[TikiPointTable.iff] => Load Files: {0}", TikiPointTable.Count);
            Debug.WriteLine("[Match.iff] => Load Files: {0}", Match.Count);
            Debug.WriteLine("[SetEffectTable.iff] => Load Files: {0}", SetEffectTable.Count);
            Debug.WriteLine("[Enchant.iff] => Load Files: {0}", Enchant.Count);
            
             
            #endif
        }

        public void UpdateIFF(string file)
        {
            Zip.UpdateFile(file);
        }

        public void UpdateIFF()
        {
            if (Update)
            {
                var path = Directory.GetCurrentDirectory() + "\\new_iffs\\";
                var count_update = 0;
                if (Part.Update)
                {
                    if (File.Exists(path + "Part.iff"))
                    {
                        UpdateIFF(path + "Part.iff");
                        count_update += 1;
                    }
                }
                else if (Ball.Update)
                {
                    if (File.Exists(path + "Ball.iff"))
                    {
                        UpdateIFF(path + "Ball.iff");
                        count_update += 1;
                    }
                }
                else if (Card.Update)
                {
                    if (File.Exists(path + "Card.iff"))
                    {
                        UpdateIFF(path + "Card.iff");
                        count_update += 1;
                    }
                }
                else if (Item.Update)
                {
                    if (File.Exists(path + "Item.iff"))
                    {
                        UpdateIFF(path + "Item.iff");
                        count_update += 1;
                    }
                }
                else if (SetItem.Update)
                {
                    if (File.Exists(path + "SetItem.iff"))
                    {
                        count_update += 1;
                        UpdateIFF(path + "SetItem.iff");
                    }
                }
                else if (HairStyle.Update)
                {
                    if (File.Exists(path + "HairStyle.iff"))
                    {
                        count_update += 1;
                        UpdateIFF(path + "HairStyle.iff");
                    }
                }//("HairStyle.else iff")){  }
                else if (Club.Update)
                {
                    if (File.Exists(path + "Club.iff"))
                    {
                        count_update += 1;
                        UpdateIFF(path + "Club.iff");
                    }
                }//("Club.else iff")){  }
                else if (ClubSet.Update)
                {
                    if (File.Exists(path + "ClubSet.iff"))
                    {
                        count_update += 1;
                        UpdateIFF(path + "ClubSet.iff");
                    }
                }
                else if (Caddie.Update)
                {
                    if (File.Exists(path + "Caddie.iff"))
                    {
                        count_update += 1;
                        UpdateIFF(path + "Caddie.iff");
                    }
                }
                else if (Skin.Update)
                {
                    if (File.Exists(path + "Skin.iff"))
                    {
                        count_update += 1;
                        UpdateIFF(path + "Skin.iff");
                    }
                }
                else if (CaddieItem.Update)
                {
                    if (File.Exists(path + "CaddieItem.iff"))
                    {
                        count_update += 1;
                        UpdateIFF(path + "CaddieItem.iff");
                    }
                }
                else if (Mascot.Update)
                {
                    if (File.Exists(path + "Mascot.iff"))
                    {
                        count_update += 1;
                        UpdateIFF(path + "Mascot.iff");
                    }
                }

                else if (MemorialShopItemRare.Update)
                {
                    if (File.Exists(path + "MemorialShopRareItem.iff"))
                    {
                        count_update += 1;
                        UpdateIFF(path + "MemorialShopRareItem.iff");
                    }
                }//("MemorialShopRareItem.else iff")){  }
                else if (MemorialShopCoinItem.Update)
                {
                    if (File.Exists(path + "MemorialShopCoinItem.sff.iff"))
                    {
                        count_update += 1;
                        UpdateIFF(path + "MemorialShopCoinItem.sff.iff");
                    }
                }//("MemorialShopCoinItem.sff", "MemorialShopCoinItem.else iff")){  }
                else if (CadieMagicBox.Update)
                {
                    if (File.Exists(path + "CadieMagicBox.iff"))
                    {
                        count_update += 1;
                        UpdateIFF(path + "CadieMagicBox.iff");
                    }
                }
                else if (CadieMagicBoxRandom.Update)
                {
                    if (File.Exists(path + "CadieMagicBoxRandom.iff"))
                    {
                        count_update += 1;
                        UpdateIFF(path + "CadieMagicBoxRandom.iff");
                    }
                }//("CadieMagicBox.else iff")){  }
                else if (AuxPart.Update)
                {
                    if (File.Exists(path + "AuxPart.iff"))
                    {
                        UpdateIFF(path + "AuxPart.iff");
                    }
                }
                if (count_update > 0)
                {
                    Zip.IffSaveBck();
                    Zip.IffSave();
                    Update = false;
                    Part.Update = false;
                    Ball.Update = false;//("Ball.else iff") = false;
                    Card.Update = false;//("Card.else iff") = false;
                    Item.Update = false;//("Item.else iff") = false;
                    SetItem.Update = false;//("SetItem.else iff") = false;
                    HairStyle.Update = false;//("HairStyle.else iff") = false;
                    Club.Update = false;//("Club.else iff") = false;
                    ClubSet.Update = false;//("ClubSet.else iff") = false;
                    Caddie.Update = false;//("Caddie.else iff") = false;
                    Skin.Update = false;//("Skin.else iff") = false;
                    CaddieItem.Update = false;//("CaddieItem.else iff") = false;
                    Mascot.Update = false;//("Mascot.else iff") = false;
                    MemorialShopItemRare.Update = false;//("MemorialShopRareItem.else iff") = false;
                    MemorialShopCoinItem.Update = false;//("MemorialShopCoinItem.sff", "MemorialShopCoinItem.else iff") = false;
                    CadieMagicBox.Update = false;//("CadieMagicBox.else iff") = false;
                    AuxPart.Update = false;
                    CadieMagicBoxRandom.Update = false;
                }
            }
        }

        public string GetDesc(uint typeID)
        {
            if (Desc.Where(c => c.TypeID == typeID).Any())
            {
                var item = Desc.Where(c => c.TypeID == typeID).First();
                return string.IsNullOrEmpty(item.Description) ? "No have" : item.Description;
            }
            return "Decription not found";
        }
        #endregion
    }
}
