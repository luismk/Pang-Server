﻿using PangyaAPI.SQL;
using PangyaAPI.SQL.TYPE;
using System.Data;

namespace GameServer.Cmd
{
    public class CmdRegisterLogonServer : Pangya_DB
    {
       uint m_uid = 0;
        int m_server_uid = 0;

        public CmdRegisterLogonServer(uint _uid, int _server_uid) 
        {
            m_uid = _uid;
            m_server_uid = _server_uid;
        }

        protected override void lineResult(ctx_res _result, uint _index_result)
        {
            // Não usa por que é um UPDATE
            return;
        }

        protected override Response prepareConsulta(database _db)
        {
            var r = procedure(_db, "pangya.ProcRegisterLogonServer", new string[] { "@IDUSER", "@gameserver_id" }, new type_SqlDbType[] { type_SqlDbType.Int, type_SqlDbType.Int }, new string[] { m_uid.ToString(), m_server_uid.ToString() }, ParameterDirection.Input);

            checkResponse(r, "nao conseguiu registrar o logon do player: " + (m_uid) + ", na option: " + (m_server_uid));
            return r;
        }

        public int getOption()
        {
            return m_server_uid;
        }


        public uint getUID()
        {
            return m_uid;
        }
    }
}
