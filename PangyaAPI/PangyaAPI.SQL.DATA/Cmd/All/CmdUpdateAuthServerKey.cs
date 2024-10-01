﻿
using PangyaAPI.SQL.DATA.TYPE;
using PangyaAPI.SQL;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PangyaAPI.SQL.DATA.Cmd
{
    public class CmdUpdateAuthServerKey: Pangya_DB
    {
        AuthServerKey m_ask;

        protected override string _getName { get; } = "CmdUpdateAuthServerKey";

        public CmdUpdateAuthServerKey(AuthServerKey _ask)
        {
            m_ask = _ask;
        }

        public CmdUpdateAuthServerKey()
        {
        }

        protected override void lineResult(ctx_res _result, uint _index_result)
        {
          
        }

        protected override Response prepareConsulta()
        {
            if (m_ask.server_uid == 0u)
                throw new Exception("[CmdUpdateAuthServerKey::prepareConsulta][Error] AuthServerKey m_ask.server_uid is invalid(zero).");

            string key = "null";
            if (!string.IsNullOrEmpty(m_ask.key))
                key = _db.makeText(m_ask.key);


            var r = procedure("pangya.ProcUpdateAuthServerKey", m_ask.server_uid.ToString() + ", " + m_ask.key + ", " + m_ask.valid.ToString());

            checkResponse(r, "nao conseguiu atualizar Auth Server Key[SERVER_UID=" + (m_ask.server_uid)
                        + ", KEY=" + key + ", VALID=" + m_ask.valid + "]");
            return r;
        }


        public AuthServerKey getInfo()
        {
            return m_ask;
        }
    }
}
