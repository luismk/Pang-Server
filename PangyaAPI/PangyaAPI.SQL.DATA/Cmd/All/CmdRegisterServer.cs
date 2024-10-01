using PangyaAPI.SQL.DATA.TYPE;
using PangyaAPI.SQL;

using System;
using System.Collections.Generic;
using System.Data;

namespace PangyaAPI.SQL.DATA.Cmd
{
    public class CmdRegisterServer: Pangya_DB
    {
        ServerInfoEx m_si;
        protected override string _getName { get; } = "CmdRegisterServer";

        public CmdRegisterServer(ServerInfoEx _si) 
        {
            m_si = _si;
        }

        protected override void lineResult(ctx_res _result, uint _index_result)
        {
          
        }

        protected override Response prepareConsulta()
        {
            var r = procedure("pangya.ProcRegServer_New", (m_si.UID).ToString() + ", " + (m_si.Name).ToString() + ", " + (m_si.IP).ToString()
               + ", " + (m_si.Port).ToString() + ", " + (m_si.Tipo).ToString() + ", " + (m_si.MaxUser).ToString()
               + ", " + (m_si.Curr_User).ToString() + ", " + (m_si.Rate.pang).ToString() + ", " + (m_si.Version).ToString()
               + ", " + (m_si.Version_Client).ToString() + ", " + (m_si.Property.ulProperty).ToString() + ", " + (m_si.AngelicWingsNum).ToString()
               + ", " + (m_si.EventFlag.usEventFlag).ToString() + ", " + (m_si.Rate.exp).ToString() + ", " + (m_si.ImgNo).ToString()
               + ", " + (m_si.Rate.scratchy).ToString() + ", " + (m_si.Rate.club_mastery).ToString() + ", " + (m_si.Rate.treasure).ToString()
               + ", " + (m_si.Rate.papel_shop_rare_item).ToString() + ", " + (m_si.Rate.papel_shop_cookie_item).ToString() + ", " + (m_si.Rate.chuva).ToString());

            checkResponse(r, "nao conseguiu registrar o server[GUID=" + (m_si.UID) + ", PORT=" + (m_si.Port) + ", NOME=" + (m_si.Name) + "] no banco de dados");
            return r;
        }

        public ServerInfoEx getServerList()
        {
            return this.m_si;
        }


        public void setInfo(ServerInfoEx _si)
        {
            m_si = _si;
        }
    }
}
