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
    public class CmdFirstSetCheck : Pangya_DB
    {
        int m_uid = -1;
        bool m_check;
        protected override string _getName { get; } = "CmdFirstSetCheck";

        public CmdFirstSetCheck(int _uid, bool _check = false)
        {
            m_uid = _uid;
            m_check = _check;
        }

        public CmdFirstSetCheck()
        {
        }

        protected override void lineResult(ctx_res _result, uint _index_result)
        {
            checkColumnNumber(1);
            try
            {
                m_check = (_result.data[0].ToString()) == "1" ? true : false;
            }
            catch (Exception)
            {

                throw;
            }
        }

        protected override Response prepareConsulta()
        {
            var r = consulta( "SELECT FIRST_SET FROM pangya.account WHERE uid = " + m_uid.ToString());
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
