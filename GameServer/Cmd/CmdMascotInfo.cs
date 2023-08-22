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
	public class CmdMascotInfo : Pangya_DB
	{
		uint m_uid = uint.MaxValue;
		public enum TYPE : int
		{
			ALL,
			ONE,
		}
		TYPE m_type;
		uint m_item_id;
		SortedList<uint/*ID*/, MascotInfoEx> v_mi;
		protected override string _getName { get; set; } = "CmdMascotInfo";

		public CmdMascotInfo(uint _uid, TYPE _type, uint _item_id = 0)
		{
			m_uid = _uid;
			m_type = _type;
			m_item_id = _item_id;
			v_mi = new SortedList<uint, MascotInfoEx>();

		}

		public CmdMascotInfo(uint _uid, int _type, uint _item_id)
		{
			m_uid = _uid;
			m_type = (TYPE)_type;
			m_item_id = _item_id;
			v_mi = new SortedList<uint, MascotInfoEx>();

		}

		protected override void lineResult(ctx_res _result, uint _index_result)
		{
			checkColumnNumber(45);
			try
			{

				MascotInfoEx mi = new MascotInfoEx();
				var i = 0;


				mi.id = Convert.ToUInt32(_result.data[0]);
				mi._typeid = Convert.ToUInt32(_result.data[2]);
				mi.level = (byte)Convert.ToUInt32(_result.data[3]);
				mi.exp = Convert.ToUInt32(_result.data[4]);
				mi.flag = (byte)Convert.ToUInt32(_result.data[5]);
				if (_result.data[6] != null)
					mi.message = _result.data[6].ToString() ;
				mi.tipo = (short)Convert.ToUInt32(_result.data[7]);
				mi.is_cash = (byte)Convert.ToUInt32(_result.data[8]);
                mi.data.CreateTime(_result.GetDateTime(9));


                var it = v_mi.Where(c => c.Key == mi.id);

				if (it.FirstOrDefault().Value == null || (it.Count() == 1 && it.FirstOrDefault().Value._typeid != mi._typeid))
					v_mi.Add(mi.id, mi);
				else if (v_mi.Where(c => c.Key == mi.id).Count() > 1)
				{

					var er = v_mi.Where(c => c.Key == mi.id);

					it = er.Where(c => c.Value._typeid == mi._typeid);

					// N�o tem um igual add um novo
					if (it == er/*End*/)
					{

						v_mi.Add(mi.id, mi);

						_smp.Message_Pool.push("[CmdMascotInfoInfo::lineResult][WARNING] player[UID=" + (m_uid) + "] adicionou MascotInfo[TYPEID="
								+ (mi._typeid) + ", ID=" + (mi.id) + "], com mesmo id e typeid diferente de outro MascotInfoEx que tem no multimap");
					}
					else
					{
						// Tem um MascotInfoEx com o mesmo ID e TYPEID (DUPLICATA)
						_smp.Message_Pool.push("[CmdMascotInfoInfo::lineResult][WARNING] player[UID=" + (m_uid) + "] tentou adicionar no multimap um MascotInfo[TYPEID="
								+ (it.First().Value._typeid) + ", ID=" + (it.First().Value.id) + "] com o mesmo ID e TYPEID, DUPLICATA");

					}
				}
				else
					// Tem um MascotInfoEx com o mesmo ID e TYPEID (DUPLICATA)
					_smp.Message_Pool.push("[CmdMascotInfoInfo::lineResult][WARNING] player[UID=" + (m_uid) + "] tentou adicionar no multimap um MascotInfo[TYPEID="
							+ (it.First().Value._typeid) + ", ID=" + (it.First().Value.id) + "] com o mesmo ID e TYPEID, DUPLICATA");

			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);

			}
		}

		protected override Response prepareConsulta(database _db)
		{
			var m_szConsulta = new string[] { "pangya.ProcGetMascotInfo", "pangya.ProcGetMascotInfo_One" };
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


		public SortedList<uint/*ID*/, MascotInfoEx> getInfo()
		{
			return v_mi;
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
