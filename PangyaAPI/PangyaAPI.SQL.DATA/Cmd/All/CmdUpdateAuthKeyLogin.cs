﻿using System;
namespace PangyaAPI.SQL.DATA.Cmd
{
    public class CmdUpdateAuthKeyLogin: Pangya_DB
    {
        int m_uid = -1;
        byte m_valid = 0;
        protected override string _getName { get; } = "CmdUpdateAuthKeyLogin";

        public CmdUpdateAuthKeyLogin(int _uid, byte _valid)
        {
            m_valid = _valid;
            m_uid = _uid;
        }

        public CmdUpdateAuthKeyLogin()
        {
        }

        protected override void lineResult(ctx_res _result, uint _index_result)
        {
          
        }

        protected override Response prepareConsulta()
        {
            if (m_uid == 0 || m_uid == -1)
                throw new Exception("[CmdUpdateAuthKeyLogin::prepareConsulta][Error] m_uid is invalid(zero).");


            var r = _update("UPDATE pangya.authkey_login SET valid = " + m_valid.ToStrings() + " WHERE UID = " +m_uid);
            checkResponse(r, "nao conseguiu pegar o Auth Server Key do Server[UID=" + (m_uid) + "]");
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

        public byte getValid()
        {
            return m_valid;
        }

        public void setValid(byte _valid)
        {
            m_valid = _valid;
        }

    }
}
