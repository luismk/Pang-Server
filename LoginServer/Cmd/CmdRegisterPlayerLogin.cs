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
        protected override string _getName { get; } = "CmdRegisterPlayerLogin";

        public CmdRegisterPlayerLogin(int _uid, string _ip, int server_uid)
        {
            m_uid = _uid;
            m_ip = _ip;
            m_server_uid = server_uid;
        }

        public CmdRegisterPlayerLogin()
        {
        }

        protected override void lineResult(ctx_res _result, uint _index_result)
        {
            // N�o usa por que � um UPDATE
            return;
        }

        protected override Response prepareConsulta()
        {
            var r = procedure("pangya.ProcRegisterLogin", m_uid.ToString() + ", " + m_ip + ", " + m_server_uid.ToString());
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
