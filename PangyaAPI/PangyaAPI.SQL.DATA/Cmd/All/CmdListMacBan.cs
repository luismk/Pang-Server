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
    public class CmdListMacBan: Pangya_DB
    {
        List<string> v_list_mac_ban;
        protected override string _getName { get; } = "CmdListMacBan";

        public CmdListMacBan()
        {
            v_list_mac_ban = new List<string>();
        }

        protected override void lineResult(ctx_res _result, uint _index_result)
        {
            checkColumnNumber(1);
            try
            {
                if (!string.IsNullOrEmpty(_result.data[0].ToString()))
                    v_list_mac_ban.Add(_result.data[0].ToString());
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
        }

        protected override Response prepareConsulta()
        {
            v_list_mac_ban.Clear();

            var r = consulta( "SELECT mac FROM pangya.pangya_mac_table");

            checkResponse(r, "nao conseguiu recuperar a lista de MAC Address");
            return r;
        }


        public List<string> getList()
        {
            if (v_list_mac_ban == null)
            {
                v_list_mac_ban = new List<string>();
            }
            return v_list_mac_ban;
        }

    }
}
