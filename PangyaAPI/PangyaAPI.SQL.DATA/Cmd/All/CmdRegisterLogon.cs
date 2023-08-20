using PangyaAPI.SQL;

using System.Data;

namespace PangyaAPI.SQL.DATA.Cmd
{
    public class CmdRegisterLogon: Pangya_DB
    {
        int m_uid = -1;
        int m_option = 0;

        public CmdRegisterLogon(int _uid, int _option) : this(true)
        {
            m_uid = _uid;
            m_option = _option;
        }
        public CmdRegisterLogon(bool wait = false) : base(wait)
        {
        }

        protected override void lineResult(ctx_res _result, uint _index_result)
        {
            // Não usa por que é um UPDATE
            return;
        }

        protected override Response prepareConsulta(database _db)
        {
         

            var r = procedure(_db, "pangya.ProcRegisterLogon", new string[] { "@IDUSER", "@OPTION" }, new type_SqlDbType[] { type_SqlDbType.Int, type_SqlDbType.Int }, new string[] { m_uid.ToString(), m_option.ToString() }, ParameterDirection.Input);

            checkResponse(r, "nao conseguiu registrar o logon do player: " + (m_uid) + ", na option: " + (m_option));
            return r;
        }

        public int getOption()
        {
            return m_option;
        }


        public int getUID()
        {
            return m_uid;
        }
    }
}
