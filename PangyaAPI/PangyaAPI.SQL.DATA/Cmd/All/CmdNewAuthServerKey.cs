
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
    public class CmdNewAuthServerKey: Pangya_DB
    {
        readonly int m_server_uid = -1;
        string m_key;
        protected override string _getName { get; set; } = "CmdNewAuthServerKey";

        public CmdNewAuthServerKey(int _uid) : this(true)
        {
            m_key = "";
            m_server_uid = _uid;
        }

        public CmdNewAuthServerKey(bool wait = false) : base(wait)
        {
        }

        protected override void lineResult(ctx_res _result, uint _index_result)
        {
            checkColumnNumber(1);
            try
            {
                m_key = _result.data[0].ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        protected override Response prepareConsulta(database _db)
        {
            if (m_server_uid == 0 || m_server_uid == -1)
                throw new Exception("[CmdNewAuthServerKey::prepareConsulta][Error] m_server_uid is invalid(zero).");


            var r = procedure(_db, "pangya.ProcGetNewAuthServerKey", new string[] { "@UID" }, new type_SqlDbType[] { type_SqlDbType.Int }, new string[] { m_server_uid.ToString() }, ParameterDirection.Input);

            checkResponse(r, "Server[UID=" + (m_server_uid) + "] nao conseguiu gerar uma nova key para o Auth Server");
            return r;
        }


        public string getInfo()
        {
            return m_key;
        }
    }
}
