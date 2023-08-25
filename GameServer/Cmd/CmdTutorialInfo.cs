using GameServer.TYPE;
using PangyaAPI.SQL;
using System;
using System.Data;

namespace GameServer.Cmd
{
    public class CmdTutorialInfo : Pangya_DB
    {
        private uint m_uid;
        TutorialInfo m_ti;
        protected override string _getName { get; set; } = "CmdTutorialInfo";
        public CmdTutorialInfo(uint _uid)
        {
            m_uid = (_uid);
            m_ti = new TutorialInfo();
        }
        protected override void lineResult(ctx_res _result, uint _index_result)
        {
            checkColumnNumber(3);
            try
            {
                m_ti.rookie = Convert.ToUInt32(_result.data[0]);
                m_ti.beginner = Convert.ToUInt32(_result.data[1]);
                m_ti.advancer = Convert.ToUInt32(_result.data[2]);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
        }

        protected override Response prepareConsulta()
        {
            var m_szConsulta = new string[] { };
            var param = new string[] { "@IDUSER"};
            var type_sql = new type_SqlDbType[] { type_SqlDbType.Int };
            var values = new string[] { m_uid.ToString() };

            var r = procedure("pangya.GetTutorial", param,
                type_sql, values, ParameterDirection.Input);
            checkResponse(r, "nao conseguiu pegar o member info do player: " + (m_uid));
            return r;
        }


        public TutorialInfo getInfo()
        {
            return m_ti;
        }

        public uint getUID()
        {
            return m_uid;
        }

        public void setUID(uint _uid)
        {
            m_uid = _uid;
        }
    }
}