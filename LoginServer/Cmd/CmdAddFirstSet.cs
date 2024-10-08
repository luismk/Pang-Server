﻿using LoginServer.TYPE;
using PangyaAPI.SQL.DATA.TYPE;
using PangyaAPI.SQL;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginServer.Cmd
{
    public class CmdAddFirstSet : Pangya_DB
    {
        int m_uid = -1;
        protected override string _getName { get; } = "CmdAddFirstSet";

        public CmdAddFirstSet(int _uid)
        {
            m_uid = _uid;
        }

        public CmdAddFirstSet()
        {
        }

        protected override void lineResult(ctx_res _result, uint _index_result)
        {
            // N�o usa por que � um UPDATE
        }

        protected override Response prepareConsulta()
        {
            var r = procedure("pangya.ProcFirstSet", m_uid.ToString());
            checkResponse(r, "nao conseguiu add first set do player: " + m_uid);
            return r;
        }



        public int getUID()
        {
            return m_uid;
        }

        public void setUID(int _uid)
        {
            m_uid = _uid;
        }
    }
}
