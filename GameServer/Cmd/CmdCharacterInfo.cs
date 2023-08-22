using GameServer.TYPE;
using PangyaAPI.SQL.DATA.TYPE;
using PangyaAPI.SQL;
using PangyaAPI.SQL.TYPE;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _smp = PangyaAPI.Utilities.Log;

namespace GameServer.Cmd
{
	public class CmdCharacterInfo : Pangya_DB
	{
		uint m_uid = uint.MaxValue;
		public enum TYPE : int
		{
			ALL,
			ONE,
		}
		TYPE m_type;
		uint m_item_id;
		SortedList<uint/*ID*/, CharacterInfoEx> v_ce;
		protected override string _getName { get; set; } = "CmdCharacterInfo";

		public CmdCharacterInfo(uint _uid, TYPE _type, uint _item_id =0)
		{
			m_uid = _uid;
			m_type = _type;
			m_item_id = _item_id;
			v_ce = new SortedList<uint, CharacterInfoEx>();

		}
		/// <summary>
		/// inicia a consulta
		/// </summary>
		/// <param name="_uid">player id</param>
		/// <param name="_type">todos os item = 1 </param>
		/// <param name="_item_id"></param>
		public CmdCharacterInfo(uint _uid, int _type, uint _item_id = 0)
		{
			m_uid = _uid;
			m_type = (TYPE)_type;
			m_item_id = _item_id;
			v_ce = new SortedList<uint, CharacterInfoEx>();
		}

		protected override void lineResult(ctx_res _result, uint _index_result)
		{
			checkColumnNumber(81);
			try
			{

				CharacterInfoEx ce = new CharacterInfoEx();
				var i = 0;

				ce.id = Convert.ToUInt32(_result.data[0]);
				ce._typeid = Convert.ToUInt32(_result.data[1]);
				for (i = 0; i < 24; i++)
					ce.parts_id[i] = Convert.ToUInt32(_result.data[2 + i]);        // 2 + 24
				for (i = 0; i < 24; i++)
					ce.parts_typeid[i] = Convert.ToUInt32(_result.data[26 + i]);   // 26 + 24
				ce.default_hair = (byte)Convert.ToUInt32(_result.data[50]);
				ce.default_shirts = (byte)Convert.ToUInt32(_result.data[51]);
				ce.gift_flag = (byte)Convert.ToUInt32(_result.data[52]);
				for (i = 0; i < 5; i++)
					ce.PCL[i] = (byte)Convert.ToUInt32(_result.data[53 + i]); // 53 + 5
				ce.Purchase = (byte)Convert.ToUInt32(_result.data[58]);
				for (i = 0; i < 5; i++)
					ce.AuxPart[i] = 0;// Convert.ToUInt32(_result.data[59 + i]);				// 59 + 5
				for (i = 0; i < 4; i++)
					ce.Cut_in[i] = 0;//Convert.ToUInt32(_result.data[64 + i]);					// 64 + 4 Cut-in deveria guarda no db os outros 3 se for msm os 4 que penso q seja, � sim no JP USA os 4
				ce.MasteryPoint = 0;//Convert.ToUInt32(_result.data[68]);
				for (i = 0; i < 4; i++)
					ce.Card_Character[i] = 0;// Convert.ToUInt32(_result.data[69 + i]);		// 69 + 4
				for (i = 0; i < 3; i++)
					ce.Card_Caddie[i] = 0;//Convert.ToUInt32(_result.data[73 + i]);			// 73 + 4
				for (i = 0; i < 3; i++)
					ce.Card_NPC[i] = 0;//Convert.ToUInt32(_result.data[77 + i]);				// 77 + 4


				var it = v_ce.Where(c => c.Key == ce.id);
				if (!it.Any())
					v_ce.Add(ce.id, ce);
            }
            catch (Exception ex)
			{
				Console.WriteLine(ex.Message);

			}
		}

		protected override Response prepareConsulta(database _db)
		{
			var m_szConsulta = new string[] { "pangya.USP_CHAR_EQUIP_LOAD_S4", "pangya.USP_CHAR_EQUIP_LOAD_S4_ONE" };
			var param = new string[] { "@IDUSER" };
			var type_sql = new type_SqlDbType[] { type_SqlDbType.Int };
			var values = new string[] { m_uid.ToString() };
			if (m_type == TYPE.ONE)
			{
				param = new string[] { "@IDUSER", "@IDITEM" };
				values = new string[] { m_uid.ToString(), m_item_id.ToString() };
				type_sql = new type_SqlDbType[] { type_SqlDbType.Int, type_SqlDbType.Int };
			}
			var r = procedure(_db, m_type == TYPE.ALL ? m_szConsulta[0] : m_szConsulta[1], param,
				type_sql, values, ParameterDirection.Input);
			checkResponse(r, "nao conseguiu pegar o member info do player: " + (m_uid));
			return r;
		}


		public SortedList<uint/*ID*/, CharacterInfoEx> getAllInfo()
		{
			return v_ce;
		}
		public CharacterInfoEx getInfo()
		{
			return v_ce.First().Value;
		}


		public uint getUID()
		{
			return m_uid;
		}

		public void setUID(uint _uid)
		{
			m_uid = _uid;
		}

		public TYPE getType()
		{
			return m_type;
		}

		public void setType(TYPE _type)
		{
			m_type = _type;
		}

		public uint getItemID()
		{
			return m_item_id;
		}

		public void setItemID(uint _item_id)
		{
			m_item_id = _item_id;
		}
	}
}
