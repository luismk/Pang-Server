﻿using PangyaAPI.SQL.DATA.TYPE;
using PangyaAPI.SQL;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PangyaAPI.SQL.DATA.Cmd
{
    public class CmdAuthKeyGame: Pangya_DB
    {
        string m_auth_key_game;
        int m_uid;
        int m_server_uid;

        protected override string _getName { get; set; } = "CmdAuthKeyGame";

        public CmdAuthKeyGame(int _uid, int _server_uid)
        {
            m_auth_key_game = "";
            m_uid = _uid;
            m_server_uid = _server_uid;
        }


        protected override void lineResult(ctx_res _result, uint _index_result)
        {
            checkColumnNumber(1);
            try
            {
                if (!string.IsNullOrEmpty(_result.data[0].ToString()))
                    m_auth_key_game = (_result.data[0].ToString());

                if (string.IsNullOrEmpty(m_auth_key_game))
                    throw new Exception("[CmdAuthKey::lineResult][Error] retornou nulo na consulta auth key do player: " + (m_uid));

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
        }

        protected override Response prepareConsulta(database _db)
        {
            m_auth_key_game = "";
            var r = procedure(_db, "pangya.ProcGeraAuthKeyGame", new string[] { "@IDUSER", "SERVERID" }, new type_SqlDbType[] { type_SqlDbType.Int, type_SqlDbType.Int }, new string[] { m_uid.ToString(), m_server_uid.ToString() }, ParameterDirection.Input);

            checkResponse(r, "nao conseguiu pegar a auth key do game server do player: " + (m_uid).ToString());
            return r;
        }


        public string getAuthKey()
        {
            return m_auth_key_game;
        }


        public void setAuthKey(string _auth_key)
        {
            m_auth_key_game = _auth_key;
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
