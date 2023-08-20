using LoginServer.TYPE;
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
    public class CmdRegisterPlayerLogin : Pangya_DB
    {
        int m_uid = -1;
        int m_server_uid;
        string m_ip;
        protected override string _getName { get; set; } = "CmdRegisterPlayerLogin";

        public CmdRegisterPlayerLogin(int _uid, string _ip, int server_uid) : this(true)
        {
            m_uid = _uid;
            m_ip = _ip;
            m_server_uid = server_uid;
        }

        public CmdRegisterPlayerLogin(bool wait = false) : base(wait)
        {
        }

        protected override void lineResult(ctx_res _result, uint _index_result)
        {
            // N�o usa por que � um UPDATE
            return;
        }

        protected override Response prepareConsulta(database _db)
        {
            var r = procedure(_db, "pangya.ProcRegisterLogin", new string[] { "@IDUSER", "@IP", "@SrvID" }, new type_SqlDbType[] { type_SqlDbType.Int , type_SqlDbType.NVarChar, type_SqlDbType.NVarChar }, new string[] { m_uid.ToString(), m_ip, m_server_uid.ToString() }, ParameterDirection.Input);
            checkResponse(r, "nao conseguiu registrar o login do player: " + (m_uid).ToString() + ", IP: " + m_ip);
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

        public int getServerUID()
        {
            return m_server_uid;
        }

        public void setServerUID(int _id)
        {
            m_server_uid = _id;
        }

        public int getIP()
        {
            return m_server_uid;
        }

        public void setIP(string _ip)
        {
            m_ip = _ip;
        }
    }
}
