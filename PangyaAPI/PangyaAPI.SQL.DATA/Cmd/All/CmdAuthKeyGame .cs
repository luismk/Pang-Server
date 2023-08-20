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
    public class CmdAuthKeyLogin: Pangya_DB
    {
        string m_auth_key_login;
        int m_uid;
        protected override string _getName { get; set; } = "CmdAuthKeyLogin";

        public CmdAuthKeyLogin(int _uid) : this(true)
        {
            m_auth_key_login = "";
            m_uid = _uid;
        }

        public CmdAuthKeyLogin(bool wait = false) : base(wait)
        {
        }

        protected override void lineResult(ctx_res _result, uint _index_result)
        {
            checkColumnNumber(1);
            try
            {
                if (!string.IsNullOrEmpty(_result.data[0].ToString()))
                    m_auth_key_login = (_result.data[0].ToString());

                if (string.IsNullOrEmpty(m_auth_key_login))
                    throw new Exception("[CmdAuthKey::lineResult][Error] retornou nulo na consulta auth key do player: " + (m_uid));

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
        }

        protected override Response prepareConsulta(database _db)
        {
            m_auth_key_login = "";
            var r = procedure(_db, "pangya.ProcGeraAuthKeyLogin", new string[] { "@IDUSER" }, new type_SqlDbType[] { type_SqlDbType.Int }, new string[] { m_uid.ToString() }, ParameterDirection.Input);

            checkResponse(r, "nao conseguiu pegar a auth key do login server do player: " + (m_uid).ToString());
            return r;
        }


        public string getAuthKey()
        {
            return m_auth_key_login;
        }


        public void setAuthKey(string _auth_key)
        {
            m_auth_key_login = _auth_key;
        }

        public int getUID()
        {
            return m_uid;
        }

        public void setUID(int _uid)
        {
            m_uid = _uid;
        }
    }
}
