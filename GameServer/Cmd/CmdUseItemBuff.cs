﻿using GameServer.TYPE;
using PangyaAPI.SQL.DATA.TYPE;
using PangyaAPI.SQL;
using PangyaAPI.SQL.TYPE;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PangyaAPI.Utilities;
namespace GameServer.Cmd
{
    public class CmdUseItemBuff : Pangya_DB
    {
        readonly uint m_uid = uint.MaxValue;
        uint m_time;
        ItemBuffEx m_ib = new ItemBuffEx();
        protected override string _getName { get; set; } = "CmdUseItemBuff";

        public CmdUseItemBuff(uint _uid, ItemBuffEx _ib, uint _time)
        {
            m_uid = _uid;
            m_ib = _ib;
            m_time = _time;
        }

        protected override void lineResult(ctx_res _result, uint _index_result)
        {
            checkColumnNumber(29);
            try
            {
                m_ib.index = Convert.ToUInt32(_result.data[0]);

                if (m_ib.index == long.MaxValue)
                    throw new exception("[CmdUseItemBuff::lineResult][Error] m_ib[index=" + (m_ib.index) + "] is invalid, nao conseguiu usar o Item Buff[TYPEID="
                            + (m_ib._typeid) + "] para o player[UID=" + (m_uid) + "]");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
        }

        protected override Response prepareConsulta(database _db)
        {
            if (m_uid == 0)
                throw new exception("[CmdUseItemBuff::prepareConsulta][Error] m_uid is invalid(zero)");

            if (m_ib._typeid == 0 || m_time == 0)
                throw new exception("[CmdUseItemBuff::prepareConsulta][Error] m_ib[TYPEID=" + (m_ib._typeid) + ", TEMPO=" + (m_time) + "] is invalid");


            var r = procedure(_db, "pangya.ProcUseItemBuff", new string[] { "@IDUSER" }, new type_SqlDbType[] { type_SqlDbType.Int }, new string[] { m_uid.ToString() }, ParameterDirection.Input);
            m_ib.index = long.MaxValue;

            checkResponse(r, "nao conseguiu usar Item[TYPEID=" + (m_ib._typeid) + ", TIPO=" + (m_ib.tipo) + ", PERCENT="
                    + (m_ib.percent) + ", TEMPO=" + (m_time) + "] do player[UID=" + (m_uid) + "]");
            return r;
        }


        public ItemBuffEx getInfo()
        {
            return m_ib;
        }

    }
}
