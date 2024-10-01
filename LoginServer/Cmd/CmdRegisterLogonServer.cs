using PangyaAPI.SQL;

using System.Data;

namespace LoginServer.Cmd
{
    public class CmdRegisterLogonServer : Pangya_DB
    {
        int m_uid = -1;
        int m_server_uid = 0;

        public CmdRegisterLogonServer(int _uid, int _server_uid) 
        {
            m_uid = _uid;
            m_server_uid = _server_uid;
        }

        protected override void lineResult(ctx_res _result, uint _index_result)
        {
            // Não usa por que é um UPDATE
            return;
        }

        protected override Response prepareConsulta()
        {
            var r = procedure("pangya.ProcRegisterLogonServer", m_uid.ToString() + ", " + m_server_uid.ToString());

            checkResponse(r, "nao conseguiu registrar o logon do player: " + (m_uid) + ", na option: " + (m_server_uid));
            return r;
        }

        public int getOption()
        {
            return m_server_uid;
        }


        public int getUID()
        {
            return m_uid;
        }
    }
}
