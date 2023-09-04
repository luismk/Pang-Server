﻿
using PangyaAPI.SQL;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PangyaAPI.SQL.DATA.Cmd
{
    public class CmdVerifyID: Pangya_DB
    {
        int m_uid = -1;
        string m_id = "";

        public CmdVerifyID(string ID)
        {
            m_id = ID;
        }
        public CmdVerifyID()
        {
        }

        protected override void lineResult(ctx_res _result, uint _index_result)
        {
            checkColumnNumber(2);
            try
            {
                m_uid = int.Parse(_result.data[0].ToString());
                var id_req = _result.data[1].ToString();

                if (m_id != id_req)
                    throw new Exception("[CmdVerifyID::lineResult][Error] ID do player info nao e igual ao requisitado. ID Req: " + m_id + " != " + id_req);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
        }

        protected override Response prepareConsulta()
        {
            if (string.IsNullOrEmpty(m_id))
                throw new Exception("[CmdVerify::prepareConsulta][Error] ID invalid");

            m_uid = 0;

            var r = procedure("pangya.ProcVerifyID", new string[] { "@ID" }, new type_SqlDbType[] { type_SqlDbType.VarChar }, new string[] { m_id }, ParameterDirection.Input);

            checkResponse(r, "nao conseguiu verificar se existe o ID: " + m_id);
            return r;
        }

        public string getID()
        {
            return m_id;
        }


        public int getUID()
        {
            return m_uid;
        }
    }
}
