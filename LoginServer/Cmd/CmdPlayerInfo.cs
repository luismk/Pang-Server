using LoginServer.TYPE;
using PangyaAPI.SQL;
using System;
using System.Data;

namespace LoginServer.Cmd
{
    public class CmdPlayerInfo: Pangya_DB
    {
        readonly int m_uid = -1;
        readonly player_info m_pi = new player_info();
        protected override string _getName { get; set; } = "CmdPlayerInfo";

        public CmdPlayerInfo(int _uid) : this(true)
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
                m_pi.uid = int.Parse(_result.data[0].ToString());
                if (!string.IsNullOrEmpty(_result.data[1].ToString()))
                   m_pi.id = _result.data[1].ToString();
                if (!string.IsNullOrEmpty(_result.data[2].ToString()))
                    m_pi.nickname = _result.data[2].ToString();
                if (!string.IsNullOrEmpty(_result.data[3].ToString()))
                   m_pi.pass = _result.data[3].ToString();
                m_pi.m_cap = int.Parse(_result.data[4].ToString());
                m_pi.level =short.Parse(_result.data[5].ToString());
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

        protected override Response prepareConsulta()
        {
            var r = procedure("pangya.ProcGetPlayerInfoLogin", new string[] { "@IDUSER" }, new type_SqlDbType[] { type_SqlDbType.Int }, new string[] { m_uid.ToString() }, ParameterDirection.Input);
            checkResponse(r, "nao conseguiu pegar o info do player: " + (m_uid));
            return r;
        }


        public PlayerInfo getInfo()
        {
            return m_pi.GetInfo();
        }

    }
}
