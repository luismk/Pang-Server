
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
    public class CmdUpdateAuthServerKey: Pangya_DB
    {
        AuthServerKey m_ask;

        protected override string _getName { get; set; } = "CmdUpdateAuthServerKey";

        public CmdUpdateAuthServerKey(AuthServerKey _ask) : this(true)
        {
            m_ask = _ask;
        }

        public CmdUpdateAuthServerKey(bool wait = false) : base(wait)
        {
        }

        protected override void lineResult(ctx_res _result, uint _index_result)
        {
          
        }

        protected override Response prepareConsulta(database _db)
        {
            if (m_ask.server_uid == 0u)
                throw new Exception("[CmdUpdateAuthServerKey::prepareConsulta][Error] AuthServerKey m_ask.server_uid is invalid(zero).");

            string key = "null";
            if (!string.IsNullOrEmpty(m_ask.key))
                key = _db.makeText(m_ask.key);


            var r = procedure(_db, "pangya.ProcUpdateAuthServerKey", new string[] { "@SERVER_UID", "@KEY", "@VALID" }, new type_SqlDbType[] { type_SqlDbType.Int, type_SqlDbType.VarChar, type_SqlDbType.TinyInt }, new string[] { m_ask.server_uid.ToString(), m_ask.key, m_ask.valid.ToString() }, ParameterDirection.Input);

            checkResponse(r, "nao conseguiu atualizar Auth Server Key[SERVER_UID=" + (m_ask.server_uid)
                        + ", KEY=" + key + ", VALID=" + m_ask.valid + "]");
            return r;
        }


        public AuthServerKey getInfo()
        {
            return m_ask;
        }
    }
}
