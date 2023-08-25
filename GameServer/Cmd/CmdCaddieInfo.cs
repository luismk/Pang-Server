using GameServer.TYPE;
using PangyaAPI.SQL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using _smp = PangyaAPI.Utilities.Log;

namespace GameServer.Cmd
{
    public class CmdCaddieInfo : Pangya_DB
    {
        uint m_uid = uint.MaxValue;
        public enum TYPE : int
        {
            ALL,
            ONE,
        }
        TYPE m_type;
        uint m_item_id;
        SortedList<uint/*ID*/, CaddieInfoEx> v_ci;
        protected override string _getName { get; set; } = "CmdCaddieInfo";

        public CmdCaddieInfo(uint _uid, TYPE _type, uint _item_id = 0)
        {
            m_uid = _uid;
            m_type = _type;
            m_item_id = _item_id;
            v_ci = new SortedList<uint, CaddieInfoEx>();

        }
        /// <summary>
        /// inicia a consulta
        /// </summary>
        /// <param name="_uid">player id</param>
        /// <param name="_type">todos os item = 0 </param>
        /// <param name="_item_id"></param>
        public CmdCaddieInfo(uint _uid, int _type, uint _item_id)
        {
            m_uid = _uid;
            m_type = (TYPE)_type;
            m_item_id = _item_id;
            v_ci = new SortedList<uint, CaddieInfoEx>();
        }

        protected override void lineResult(ctx_res _result, uint _index_result)
        {
            checkColumnNumber((uint)_result.getRow.ItemArray.Count());
            try
            {
                if (_result.getRow.ItemArray.Count() <= 1)
                {
                    return;
                }
                CaddieInfoEx ci = new CaddieInfoEx();
                var i = 0;

                ci.id = _result.GetUInt32(0);
                ci._typeid = _result.GetUInt32(1);
                ci.parts_typeid = _result.GetUInt32(3);
                ci.level = _result.GetByte(4);
                ci.exp = _result.GetUInt32(5);
                ci.rent_flag = _result.GetByte(6);
                if (_result.getRow.ItemArray[7] != null && _result.getRow.ItemArray[7].ToString() != "0")
                    ci.end_date.CreateTime(_result.GetDateTime(7));

                ci.purchase = _result.GetByte(8);
                if (!(_result.getRow.ItemArray[9] == DBNull.Value))
                    ci.end_parts_date.CreateTime(_result.GetDateTime(9));
                ci.check_end = _result.GetByte(10);

                var it = v_ci.Where(c => c.Key == ci.id);

                if (it.FirstOrDefault().Value == null || (it.Count() == 1 && it.FirstOrDefault().Value._typeid != ci._typeid))
                    v_ci.Add(ci.id, ci);
                else if (v_ci.Where(c => c.Key == ci.id).Count() > 1)
                {

                    var er = v_ci.Where(c => c.Key == ci.id);

                    it = er.Where(c => c.Value._typeid == ci._typeid);

                    // N�o tem um igual add um novo
                    if (it == er/*End*/)
                    {

                        v_ci.Add(ci.id, ci);

                        _smp.Message_Pool.push("[CmdCaddieInfoInfo::lineResult][WARNING] player[UID=" + (m_uid) + "] adicionou CaddieInfo[TYPEID="
                                + (ci._typeid) + ", ID=" + (ci.id) + "], com mesmo id e typeid diferente de outro CaddieInfoEx que tem no multimap");
                    }
                    else
                    {
                        // Tem um CaddieInfoEx com o mesmo ID e TYPEID (DUPLICATA)
                        _smp.Message_Pool.push("[CmdCaddieInfoInfo::lineResult][WARNING] player[UID=" + (m_uid) + "] tentou adicionar no multimap um CaddieInfo[TYPEID="
                                + (it.First().Value._typeid) + ", ID=" + (it.First().Value.id) + "] com o mesmo ID e TYPEID, DUPLICATA");

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
        }

        protected override Response prepareConsulta()
        {
            var m_szConsulta = new string[] { "pangya.ProcGetCaddieInfo", "pangya.ProcGetCaddieInfo_One" };
            var param = new string[] { "@IDUSER" };
            var type_sql = new type_SqlDbType[] { type_SqlDbType.Int };
            var values = new string[] { m_uid.ToString() };
            if (m_type == TYPE.ONE)
            {
                param = new string[] { "@IDUSER", "@CaddieID" };
                values = new string[] { m_uid.ToString(), m_item_id.ToString() };
                type_sql = new type_SqlDbType[] { type_SqlDbType.Int, type_SqlDbType.Int };
            }
            var r = procedure(m_type == TYPE.ALL ? m_szConsulta[0] : m_szConsulta[1], param,
                type_sql, values, ParameterDirection.Input);
            checkResponse(r, "nao conseguiu pegar o member info do player: " + (m_uid));
            return r;
        }


        public SortedList<uint/*ID*/, CaddieInfoEx> getInfo()
        {
            return v_ci;
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
