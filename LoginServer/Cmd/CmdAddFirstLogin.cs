using PangyaAPI.SQL;

namespace LoginServer.Cmd
{
    public class CmdAddFirstLogin : Pangya_DB
    {
        int m_uid = -1;
        byte m_flag;
        protected override string _getName { get; } = "CmdAddFirstLogin";

        public CmdAddFirstLogin(int _uid, byte _flag)
        {
            m_uid = _uid;
            m_flag = _flag;
        }

        public CmdAddFirstLogin()
        {
        }

        protected override void lineResult(ctx_res _result, uint _index_result)
        {
            // N�o usa por que � um UPDATE
        }

        protected override Response prepareConsulta()
        {
            var r = consulta( "UPDATE pangya.account SET FIRST_LOGIN = " + m_flag + " WHERE UID = " + m_uid.ToString());
            checkResponse(r, "nao conseguiu atualizar o first login do player: " + (m_uid));
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

        public byte getFLag()
        {
            return m_flag;
        }

        public void setFlag(byte _flag)
        {
            m_flag = _flag;
        }
    }
}
