﻿using PangyaAPI.SQL.DATA.TYPE;
using PangyaAPI.SQL;

using System;
using System.Data;
namespace PangyaAPI.SQL.DATA.Cmd
{
    public class CmdAuthKeyGameInfo: Pangya_DB
    {
        int m_uid = -1;
        int m_server_uid = -1;
        AuthKeyGameInfo m_akgi;
        protected override string _getName { get; set; } = "CmdAuthKeyGameInfo";

        public CmdAuthKeyGameInfo(int _uid, int _server_uid) : this(true)
        {
            m_akgi = new AuthKeyGameInfo();
            m_uid = _uid;
            m_server_uid= _server_uid;
        }

        public CmdAuthKeyGameInfo(bool wait = false) : base(wait)
        {
        }

        protected override void lineResult(ctx_res _result, uint _index_result)
        {
            checkColumnNumber(3);
            try
            {
                if (!string.IsNullOrEmpty(_result.data[0].ToString()))
                    m_akgi.key = _result.data[0].ToString();

                m_akgi.server_uid = int.Parse(_result.data[1].ToString());
                m_akgi.valid = byte.Parse(_result.data[2].ToString());
                if (m_akgi.key[0] == '\0')
                    throw new Exception("[CmdAuthKeyGameInfo::lineResult][Error] a consulta retornou uma auth key login invalid");
                if (m_akgi.server_uid != m_server_uid)
                    throw new Exception("[CmdAuthKeyGameInfo::lineResult][Error] o server uid retornado na consulta nao eh igual ao requisitado. server uid req: "
                            + (m_server_uid).ToString() + " != " + (m_akgi.server_uid).ToString());

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        protected override Response prepareConsulta(database _db)
        {

            if (m_uid == 0 || m_uid == -1)
                throw new Exception("[CmdAuthKeyGameInfo::prepareConsulta][Error] m_uid is invalid(zero).");

            var r = procedure(_db, "pangya.ProcGetAuthKeyGame", new string[] { "@UID", "@SERVERUID" }, new type_SqlDbType[] { type_SqlDbType.Int }, new string[] { m_uid.ToString(), m_server_uid.ToString() }, ParameterDirection.Input);

            checkResponse(r, "nao conseguiu pegar o auth key game do player: " + (m_uid) + ", do server uid: " + (m_server_uid));
            return r;
        }


        public int getUID()
        {
            return m_uid;
        }

       public void setUID(int _server_uid)
        {
            m_uid = _server_uid;
        }

        public AuthKeyGameInfo getInfo()
        {
            return m_akgi;
        }

        public void setInfo(AuthKeyGameInfo _akli)
        {
            m_akgi = _akli;
        }
    }
}
