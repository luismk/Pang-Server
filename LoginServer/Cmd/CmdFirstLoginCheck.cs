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
    public class CmdFirstLoginCheck : Pangya_DB
    {
        int m_uid = -1;
        bool m_check;
        protected override string _getName { get; set; } = "CmdFirstLoginCheck";

        public CmdFirstLoginCheck(int _uid, bool _check = false) : this(true)
        {
            m_uid = _uid;
            m_check = _check;
        }

        public CmdFirstLoginCheck(bool wait = false) : base(wait)
        {
        }

        protected override void lineResult(ctx_res _result, uint _index_result)
        {
            checkColumnNumber(1);
            try
            {
                m_check = _result.GetInt32(0) == 1 ? false : true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        protected override Response prepareConsulta(database _db)
        {
            var r = consulta(_db, "SELECT FIRST_LOGIN FROM pangya.account WHERE uid = " + m_uid.ToString());
            checkResponse(r, "nao conseguiu verificar o first login do player: " + (m_uid));
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

        public bool getLastCheck()
        {
            return m_check;
        }

        public void setCheck(bool _check)
        {
            m_check = _check;
        }
    }
}
