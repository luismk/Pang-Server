using GameServer.TYPE;
using PangyaAPI.SQL.DATA.TYPE;
using PangyaAPI.SQL;
using PangyaAPI.SQL.TYPE;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Cmd
{
    public class CmdPlayerInfo : Pangya_DB
    {
        readonly uint m_uid = uint.MaxValue;
        player_info m_pi = new player_info();
        protected override string _getName { get; set; } = "CmdPlayerInfo";

        public CmdPlayerInfo(uint _uid) : this(true)
        {
            m_uid = _uid;
        }

        public CmdPlayerInfo(bool wait = false) : base(wait)
        {
        }

        protected override void lineResult(ctx_res _result, uint _index_result)
        {
            checkColumnNumber(8);
            try
            {
                // Aqui faz as coisas
                m_pi.uid = uint.Parse(_result.data[0].ToString());
                if (!string.IsNullOrEmpty(_result.data[1].ToString()))
                    m_pi.id = _result.data[1].ToString();
                if (!string.IsNullOrEmpty(_result.data[2].ToString()))
                    m_pi.nickname = _result.data[2].ToString();
                if (!string.IsNullOrEmpty(_result.data[3].ToString()))
                    m_pi.pass = _result.data[3].ToString();
                m_pi.level = short.Parse(_result.data[5].ToString());
                m_pi.block_flag.setIDState(ulong.Parse(_result.data[6].ToString()));
                m_pi.block_flag.m_id_state.block_time = (int.Parse(_result.data[7].ToString()));
                // Fim

                if (m_pi.uid != m_uid)
                    throw new Exception("[CmdPlayerInfo::lineResult][Error] UID do player info nao e igual ao requisitado. UID Req: " + (m_uid) + " != " + (m_pi.uid));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
        }

        protected override Response prepareConsulta(database _db)
        {
            var r = procedure(_db, "pangya.ProcGetPlayerInfoGame", new string[] { "@IDUSER" }, new type_SqlDbType[] { type_SqlDbType.Int }, new string[] { m_uid.ToString() }, ParameterDirection.Input);
            checkResponse(r, "nao conseguiu pegar o info do player: " + (m_uid));
            return r;
        }


        public PlayerInfo getInfo()
        {
            return m_pi.GetInfo();
        }

    }
}
