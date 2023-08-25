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
    public class CmdInsertBlockMac: Pangya_DB
    {
        string m_mac_address;
        protected override string _getName { get; set; } = "CmdInsertBlockMac";

        public CmdInsertBlockMac(string _mac_address) : this(true)
        {
            m_mac_address = _mac_address;
        }

        public CmdInsertBlockMac(bool wait = false) : base(wait)
        {
        }

        protected override void lineResult(ctx_res _result, uint _index_result)
        {
            // é um update;
            return;
        }

        protected override Response prepareConsulta()
        {
            if (string.IsNullOrEmpty(m_mac_address))
                throw new Exception("[CmdInsertBlockMAC::prepareConsulta][Error] m_mac_address is empty.");

            var r = procedure("pangya.ProcInsertBlockMAC", new string[] { "@MAC" }, new type_SqlDbType[] { type_SqlDbType.NVarChar },
                new string[] { m_mac_address }, ParameterDirection.Input);

            checkResponse(r, "nao conseguiu inserir o MAC ADDRESS[" + m_mac_address + "] para a lista de MAC bloqueado");
            return r;

        }

        public string getMACAddress()
        {
            return m_mac_address;
        }

        public void setMACAddress(string _mac_address)
        {
            m_mac_address = _mac_address;
        }
    }
}
