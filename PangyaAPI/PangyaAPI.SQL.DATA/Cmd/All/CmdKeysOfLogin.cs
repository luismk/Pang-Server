﻿using PangyaAPI.SQL.DATA.TYPE;
using PangyaAPI.SQL;

using System;
using System.Data;

namespace PangyaAPI.SQL.DATA.Cmd
{
    public class CmdKeysOfLogin: Pangya_DB
    {
         int m_uid = -1;
        KeysOfLogin m_keys_of_login;
        protected override string _getName { get; set; } = "CmdKeysOfLogin";

        public CmdKeysOfLogin(int _uid) : this(true)
        {
            m_keys_of_login = new KeysOfLogin();
            m_uid = _uid;
        }

        public CmdKeysOfLogin(bool wait = false) : base(wait)
        {
        }

        protected override void lineResult(ctx_res _result, uint _index_result)
        {
            checkColumnNumber(3);
            try
            {
                if (!string.IsNullOrEmpty(_result.data[0].ToString()))
                  m_keys_of_login.keys[0] = _result.data[0].ToString();
                if (!string.IsNullOrEmpty(_result.data[1].ToString()))
                   m_keys_of_login.keys[1] = _result.data[1].ToString();
                m_keys_of_login.valid = byte.Parse(_result.data[2].ToString());

                if (m_keys_of_login.keys[0][0] == '\0' || m_keys_of_login.keys[1][0] == '\0')
                    throw new Exception("[CmdKeysOfLogin::lineResult][Error] a consulta retornou as chaves nula do player: " + (m_uid));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
        }

        protected override Response prepareConsulta()
        {
            var r = procedure("pangya.ProcGetMacrosUser", new string[] { "@IDUSER" }, new type_SqlDbType[] { type_SqlDbType.Int }, new string[] { m_uid.ToString() }, ParameterDirection.Input);
            checkResponse(r, "nao conseguiu pegar o macro do player: " + (m_uid));
            return r;
        }


        public KeysOfLogin getKeys()
        {
            return m_keys_of_login;
        }
        public void setKeys(KeysOfLogin _keys_of_login)
        {
            m_keys_of_login = _keys_of_login;
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
