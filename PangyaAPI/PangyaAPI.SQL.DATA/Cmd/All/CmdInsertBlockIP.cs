﻿using PangyaAPI.SQL.DATA.TYPE;
using PangyaAPI.SQL;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PangyaAPI.SQL.DATA.Cmd
{
    public class CmdInsertBlockIp: Pangya_DB
    {
        string m_ip;
        string m_mask;
        protected override string _getName { get; set; } = "CmdInsertBlockIp";

        public CmdInsertBlockIp(string _ip_address, string mask) : this(true)
        {
            m_ip = _ip_address;
            m_mask = mask;
        }

        public CmdInsertBlockIp(bool wait = false) : base(wait)
        {
        }

        protected override void lineResult(ctx_res _result, uint _index_result)
        {
            // é um update;
            return;
        }

        protected override Response prepareConsulta()
        {
            if (string.IsNullOrEmpty(m_ip) || string.IsNullOrEmpty(m_mask))
                throw new Exception("[CmdInsertBlockIP::prepareConsulta][Error] m_ip[" + m_ip + "] or m_mask["
            + m_mask + "] is invalid");

            var r = procedure("pangya.ProcInsertBlocIP", new string[] { "@IP" }, new type_SqlDbType[] { type_SqlDbType.NVarChar },
                new string[] { m_ip }, ParameterDirection.Input);

            checkResponse(r, "nao conseguiu inserir um Block IP[IP=" + m_ip + ", MASK=" + m_mask + "]");
            return r;
        }

        public string getIP()
        {
            return m_ip;
        }

        public void setIP(string _ip_address)
        {
            if (!string.IsNullOrEmpty(m_ip))
                m_ip = _ip_address;
        }


        public string geMask()
        {
            return m_mask;
        }

        public void setMask(string mask)
        {
            if (!string.IsNullOrEmpty(m_mask))
                m_mask = mask;
        }
    }
}
