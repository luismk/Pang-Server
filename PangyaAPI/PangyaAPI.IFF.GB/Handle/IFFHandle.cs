using PangyaAPI.IFF.Collections;
using PangyaAPI.ZIP;
using System;

namespace PangyaAPI.IFF.Handle
{
    /// update in 26/04/2022 - 01:14 PM by LuisMK
    public partial class IFFHandle
	{
        #region Fields
        public string FileName { get; set; }

		public ZipFiles Zip { get; set; }

		public bool Update { get; set; }
        #endregion

        #region  Fields IFF Collections 
        /// <summary>
        /// Is File Data(Part.iff)
        /// </summary>
        public PartCollection Part { get; set; } = new PartCollection();
		/// <summary>
		/// Is File Data(Ball.iff)
		/// </summary>
		public BallCollection Ball { get; set; } = new BallCollection();

		/// <summary>
		/// Is File Data(Caddie.iff)
		/// </summary>
		public CaddieCollection Caddie { get; set; } = new CaddieCollection();

		/// <summary>
		/// Is File Data(Item.iff)
		/// </summary>
		public ItemCollection Item { get; set; } = new ItemCollection();

		/// <summary>
		/// Is File Data(SetItem.iff)
		/// </summary>
		public SetItemCollection SetItem { get; set; } = new SetItemCollection();

		/// <summary>
		/// Is File Data(HairStyle.iff)
		/// </summary>
		public HairStyleCollection HairStyle { get; set; } = new HairStyleCollection();

		/// <summary>
		/// Is File Data(Skin.iff)
		/// </summary>
		public SkinCollection Skin { get; set; } = new SkinCollection();

		/// <summary>
		/// Is File Data(CaddieItem.iff)
		/// </summary>
		public CaddieItemCollection CaddieItem { get; set; } = new CaddieItemCollection();

		/// <summary>
		/// Is File Data(Mascot.iff)
		/// </summary>
		public MascotCollection Mascot { get; set; } = new MascotCollection();

		/// <summary>
		/// Is File Data(CutinInformation.iff)
		/// </summary>
		public CutinInformationCollection CutinInformation { get; set; } = new CutinInformationCollection();

		/// <summary>
		/// Is File Data(GrandPrixData.iff)
		/// </summary>
		public GrandPrixDataCollection GrandPrixData { get; set; } = new GrandPrixDataCollection();

		/// <summary>
		/// Is File Data(Card.iff)
		/// </summary>
		public CardCollection Card { get; set; } = new CardCollection();


        /// <summary>
        /// Is File Data(Club.iff)
        /// </summary>
        public ClubCollection Club { get; set; } = new ClubCollection();


        /// <summary>
        /// Is File Data(ClubSet.iff)
        /// </summary>
        public ClubSetCollection ClubSet { get; set; } = new ClubSetCollection();

		/// <summary>
		/// Is File Data(LevelUpPrizeItem.iff)
		/// </summary>
		public LevelUpPrizeItemCollection LevelUpPrizeItem { get; set; } = new LevelUpPrizeItemCollection();

		/// <summary>
		/// Is File Data(Character.iff)
		/// </summary>
		public CharacterCollection Character { get; set; } = new CharacterCollection();

		/// <summary>
		/// Is File Data(GrandPrixSpecialHole.iff)
		/// </summary>
		public GrandPrixSpecialHoleCollection GrandPrixSpecialHole { get; set; } = new GrandPrixSpecialHoleCollection();

		/// <summary>
		/// Is File Data(GrandPrixRankReward.iff)
		/// </summary>
		public GrandPrixRankRewardCollection GrandPrixRankReward { get; set; } = new GrandPrixRankRewardCollection();

		/// <summary>
		/// Is File Data(MemorialShopCoinItem.iff)
		/// </summary>
		public MemorialShopCoinItemCollection MemorialShopCoinItem { get; set; } = new MemorialShopCoinItemCollection();

		/// <summary>
		/// Is File Data(MemorialShopItemRare.iff)
		/// </summary>
		public MemorialShopRareItemCollection MemorialShopItemRare { get; set; } = new MemorialShopRareItemCollection();

		/// <summary>
		/// Is File Data(CadieMagicBox.iff)
		/// </summary>
		public CadieMagicBoxCollection CadieMagicBox { get; set; } = new CadieMagicBoxCollection();

        /// <summary>
        /// Is File Data(CadieMagicBoxRandom.iff)
        /// </summary>
        public CadieMagicBoxRandomCollection CadieMagicBoxRandom { get; set; } = new CadieMagicBoxRandomCollection();

        /// <summary>
        /// Is File Data(AuxPart.iff)
        /// </summary>
        public AuxPartCollection AuxPart { get; set; } = new AuxPartCollection();

		/// <summary>
		/// Is File Data(Desc.iff)
		/// </summary>
		public DescCollection Desc { get; set; } = new DescCollection();

        /// <summary>
        /// Is File Data(Achievement.iff)
        /// </summary>
        public AchievementCollection Achievement { get; set; } = new AchievementCollection();


        /// <summary>
        /// Read data from the QuestStuff.iff file
        /// </summary>
        public QuestStuffCollection QuestStuff { get; set; } = new QuestStuffCollection();

        /// <summary>
        /// Read data from the QuestItem.iff file
        /// </summary>
        public QuestItemCollection QuestItem { get; set; } = new QuestItemCollection();

        /// <summary>
        /// Is File Data(Furniture.iff)
        /// </summary>
        public FurnitureCollection Furniture { get; set; } = new FurnitureCollection();

        /// <summary>
        /// Is File Data(FurnitureAbility.iff)
        /// </summary>
        public FurnitureAbilityCollection FurnitureAbility { get; set; } = new FurnitureAbilityCollection();


        /// <summary>
        /// Is File Data(Ability.iff)
        /// </summary>
        public AbilityCollection Ability { get; set; } = new AbilityCollection();

        /// <summary>
        /// Is File Data(TikiSpecialTable.iff)
        /// </summary>
        public TikiSpecialTableCollection TikiSpecialTable { get; set; } = new TikiSpecialTableCollection();

        /// <summary>
        /// Is File Data(Achievement.iff)
        /// </summary>
        public TikiRecipeCollection TikiRecipe { get; set; } = new TikiRecipeCollection();

        /// <summary>
        /// Is File Data(TikiPointTable.iff)
        /// </summary>
        public TikiPointTableCollection TikiPointTable { get; set; } = new TikiPointTableCollection();

        /// <summary>
        /// Read data from the Match.iff file
        /// </summary>
        public MatchCollection Match { get; set; } = new MatchCollection();

        /// <summary>
        /// Read data from the Enchant.iff file
        /// </summary>
        public EnchantCollection Enchant { get; set; } = new EnchantCollection();

        /// <summary>
        /// Read data from the SetEffectTable.iff file
        /// </summary>
        public SetEffectTableCollection SetEffectTable { get; set; } = new SetEffectTableCollection();
        /// <summary>
        /// Read data from the Course.iff file
        /// </summary>
        public CourseCollection Course { get; set; } = new CourseCollection();
        #endregion

        #region IFF Constructor's

        public IFFHandle()
		{
			Zip = new ZipFiles();
		}

        public IFFHandle(string namefile)
        {
            Zip = new ZipFiles();
            LoadAll(namefile);
        }

        #endregion
    }
    public class ClubStatus
    {
        public ushort Power { get; set; }
        public ushort Control { get; set; }
        public ushort Impact { get; set; }
        public ushort Spin { get; set; }
        public ushort Curve { get; set; }
        public byte ClubType { get; set; }
        public byte ClubSPoint { get; set; }

        // ClubStatus
        public byte[] GetClubArray()
        {
            byte[] result = new byte[5];
            result[0] = (byte)Power;
            result[1] = (byte)Control;
            result[2] = (byte)Impact;
            result[3] = (byte)Spin;
            result[4] = (byte)Curve;
            return result;
        }

        public ClubStatus GetClubPlayer(ClubStatus ClubPlayerData)
        {
            ClubStatus result;
            result = this + ClubPlayerData;
            return result;
        }

        public int GetClubTotal(ClubStatus ClubPlayerData, bool IsRankUp)
        {
            int result;
            result = (Power + Control + Impact + Spin + Curve + ClubPlayerData.Power + ClubPlayerData.Control + ClubPlayerData.Impact + ClubPlayerData.Spin + ClubPlayerData.Curve);
            if (IsRankUp)
            {
                result += 1;
            }
            return result;
        }


        public static ClubStatus operator -(ClubStatus X, ClubStatus Y)
        {
            ClubStatus result = new ClubStatus()
            {
                Power = Convert.ToUInt16(Y.Power - X.Power),
                Control = Convert.ToUInt16(Y.Control - X.Control),
                Impact = Convert.ToUInt16(Y.Impact - X.Impact),
                Spin = Convert.ToUInt16(Y.Spin - X.Spin),
                Curve = Convert.ToUInt16(Y.Curve - X.Curve)
            };

            return result;
        }

        public static ClubStatus operator +(ClubStatus X, ClubStatus Y)
        {
            ClubStatus result = new ClubStatus()
            {
                Power = Convert.ToUInt16(X.Power + Y.Power),
                Control = Convert.ToUInt16(X.Control + Y.Control),
                Impact = Convert.ToUInt16(X.Impact + Y.Impact),
                Spin = Convert.ToUInt16(X.Spin + Y.Spin),
                Curve = Convert.ToUInt16(X.Curve + Y.Curve)
            };
            return result;
        }

    } // end ClubStatus
}
