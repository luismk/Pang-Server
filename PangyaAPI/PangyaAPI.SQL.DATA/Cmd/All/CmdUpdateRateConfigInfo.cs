using PangyaAPI.SQL.DATA.TYPE;
using PangyaAPI.SQL;

using System;
using System.Collections.Generic;
using System.Data;

namespace PangyaAPI.SQL.DATA.Cmd
{
    public class CmdUpdateRateConfigInfo: Pangya_DB
    {
        int m_server_uid = -1;
        bool m_error = false;
        RateConfigInfo m_rci;
        protected override string _getName { get; set; } = "CmdUpdateRateConfigInfo";
       

        public CmdUpdateRateConfigInfo(int _uid, RateConfigInfo _rate)
        {
            m_server_uid = _uid;
            m_rci = _rate;
        }
        public CmdUpdateRateConfigInfo()
        {
        }

        protected override void lineResult(ctx_res _result, uint _index_result)
        {
           
        }

        protected override Response prepareConsulta()
        {

            if (m_server_uid == 0u)
                throw new Exception("[CmdUpdateRateConfigInfo][Error] server_uid[VALUE=" + (m_server_uid) + "] is invalid.");
            //var r = consulta( "SELECT pangya_server_list.[Name], pangya_server_list.[UID], pangya_server_list.[IP], pangya_server_list.[Port], pangya_server_list.MaxUser, pangya_server_list.CurrUser, pangya_server_list.property, pangya_server_list.AngelicWingsNum, pangya_server_list.EventFlag, pangya_server_list.EventMap, pangya_server_list.ImgNo, pangya_server_list.AppRate, pangya_server_list.ScratchRate FROM {0}.pangya_server_list WHERE pangya_server_list.[Type] = 1");

            var r = procedure("pangya.ProcUpdateRateConfigInfo", new string[] { "@SERVER_UID", "@GRAND_ZODIAC_EVENT", "@SCRATCHY", "@PAPEL_SHOP_RARE_ITEM", "@PAPEL_SHOP_COOKIE_ITEM", "@TREASURE", "@PANG", "@EXP", "@CLUB_MASTERY", "@CHUVA", "@MEMORIAL_SHOP", "@ANGEL_EVENT", "@GRAND_PRIX_EVENT", "@GOLDEN_TIME_EVENT", "@LOGIN_REWARD_EVENT", "@BOT_GM_EVENT", "@SMART_CALCULATOR" }, new type_SqlDbType[] { type_SqlDbType.Int, type_SqlDbType.Int, type_SqlDbType.SmallInt,
                    type_SqlDbType.Int, type_SqlDbType.SmallInt,
                    type_SqlDbType.SmallInt, type_SqlDbType.SmallInt,
                    type_SqlDbType.SmallInt, type_SqlDbType.SmallInt,
                    type_SqlDbType.SmallInt, type_SqlDbType.SmallInt, type_SqlDbType.Int, type_SqlDbType.SmallInt,
                    type_SqlDbType.Int, type_SqlDbType.SmallInt,
                    type_SqlDbType.SmallInt, type_SqlDbType.SmallInt,
                    type_SqlDbType.SmallInt, type_SqlDbType.SmallInt,
                    type_SqlDbType.SmallInt, type_SqlDbType.SmallInt, type_SqlDbType.Int, type_SqlDbType.SmallInt,
                    type_SqlDbType.Int, type_SqlDbType.SmallInt,
                    type_SqlDbType.SmallInt, type_SqlDbType.SmallInt,
                    type_SqlDbType.SmallInt, type_SqlDbType.SmallInt,
                    type_SqlDbType.SmallInt, type_SqlDbType.SmallInt,  type_SqlDbType.SmallInt, type_SqlDbType.SmallInt}, new string[] { m_server_uid.ToString(), m_rci.grand_zodiac_event_time.ToString(), m_rci.scratchy.ToString(), m_rci.papel_shop_rare_item.ToString(), m_rci.papel_shop_cookie_item.ToString(), m_rci.treasure.ToString(), m_rci.pang.ToString(), m_rci.exp.ToString(), m_rci.club_mastery.ToString(), m_rci.chuva.ToString(), m_rci.memorial_shop.ToString(), m_rci.angel_event.ToString(), m_rci.grand_prix_event.ToString(), m_rci.golden_time_event.ToString(), m_rci.login_reward_event.ToString(), m_rci.bot_gm_event.ToString(), m_rci.smart_calculator.ToString() }, ParameterDirection.Input);

            checkResponse(r, "nao conseguiu atualizar o Rate Config Info[SERVER_UID=" + (m_server_uid) + ", " + m_rci.ToString() + "]");
            return r;
        }

        public RateConfigInfo GetInfo()
        {
            return this.m_rci;
        }


        public bool isError()
        {
            return m_error;
        }
    }
}
