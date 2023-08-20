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
    public class CmdChatMacroUser: Pangya_DB
    {
        readonly int m_uid = -1;
        chat_macro_user m_macro_user;
        protected override string _getName { get; set; } = "CmdChatMacroUser";

        public CmdChatMacroUser(int _uid) 
        {
            m_macro_user = new chat_macro_user().Init();
            m_uid = _uid;
        }

        protected override void lineResult(ctx_res _result, uint _index_result)
        {
            checkColumnNumber(9);
            try
            {
                for (int i = (int)0u; i < 9u; i++)
                {

                    if (_result.data[i] != null)
                    {
                        var _chat = _result.data[i].ToString();
                        // var _chat = verifyAndTranslate(_result.data[i].ToString(), 2/*fixed size*/);
                        // !@ possivel erro de violação de acesso
                        if (!string.IsNullOrEmpty(_chat))
                        {
                            try
                            {
                                m_macro_user.macro[i] = _result.data[i].ToString();
                            }
                            catch (Exception)
                            {

                                throw;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
        }

        protected override Response prepareConsulta(database _db)
        {
            var r = procedure(_db, "pangya.ProcGetMacrosUser", new string[] { "@IDUSER" }, new type_SqlDbType[] { type_SqlDbType.Int }, new string[] { m_uid.ToString() }, ParameterDirection.Input);
            checkResponse(r, "nao conseguiu pegar o macro do player: " + (m_uid));
            return r;
        }


        public chat_macro_user getMacroUser()
        {
            return m_macro_user;
        }

    }
}
