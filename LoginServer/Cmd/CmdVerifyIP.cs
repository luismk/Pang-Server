using LoginServer.TYPE;
using PangyaAPI.SQL.DATA.TYPE;
using PangyaAPI.SQL;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PangyaAPI.Utilities;

namespace LoginServer.Cmd
{
    public class CmdVerifyIP : Pangya_DB
    {
        int m_uid = -1;
        string m_ip;
        private bool m_last_verify;

        protected override string _getName { get; set; } = "CmdVerifyIP";

        public CmdVerifyIP(int _uid, string _ip) : this(true)
        {
            m_uid = _uid;
            m_ip = _ip;
        }

        public CmdVerifyIP(bool wait = false) : base(wait)
        {
        }

        protected override void lineResult(ctx_res _result, uint _index_result)
        {
            checkColumnNumber(1);

            int uid_req = int.Parse(_result.data[0].ToString());

            if (uid_req != m_uid)
                throw new exception("[CmdVerifyIP::lineResult][Error] o uid recuperado para verificar o ip access do player e diferente. UID_req: " + (m_uid) + " != " + (uid_req), STDA_ERROR_TYPE.PANGYA_DB);

            m_last_verify = true;
        }

        protected override Response prepareConsulta()
        {
            m_last_verify = false;
            var r = procedure("pangya.ProcVerifyIP", new string[] { "@UID", "@IP" }, new type_SqlDbType[] { type_SqlDbType.Int, type_SqlDbType.NVarChar }, new string[] { m_uid.ToString(), m_ip }, ParameterDirection.Input);
            checkResponse(r, "nao conseguiu verificar o ip de accesso do player: " + (m_uid));
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

      
        public string getIP()
        {
            return m_ip;
        }

        public void setIP(string _ip)
        {
            m_ip = _ip;
        }
       public bool getLastVerify()
        {
            return m_last_verify;
        }
    }
}
