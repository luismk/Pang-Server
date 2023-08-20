using PangyaAPI.SQL;

using System;
using System.Data;

namespace PangyaAPI.SQL.DATA.Cmd
{
    public class CmdSaveNick: Pangya_DB
    {
        int m_uid = -1;
        string m_nick = "";

        public CmdSaveNick(int _uid, string _nick)
        {
            m_uid = _uid;
            m_nick = _nick;
        }
        protected override void lineResult(ctx_res _result, uint _index_result)
        {
          
        }

        protected override Response prepareConsulta(database _db)
        {
            if (string.IsNullOrEmpty(m_nick))
                throw new Exception("[CmdSaveNick::prepareConsulta][Error] Nick invalid");

            var r = procedure(_db, "pangya.ProcSaveNickname", new string[] { "@UID", "@NICKNAME" }, new type_SqlDbType[] { type_SqlDbType.Int, type_SqlDbType.VarChar }, new string[] { m_uid.ToString(), m_nick }, ParameterDirection.Input);
            checkResponse(r, "nao conseguiu atualizar o nick: " + m_nick + ", do player: " + m_uid);
            return r;
        }

    }
}
