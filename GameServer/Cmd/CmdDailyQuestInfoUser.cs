using PangyaAPI.SQL;

namespace GameServer.Cmd
{
    internal class CmdDailyQuestInfoUser : Pangya_DB
    {
        private uint uid;
        private object gET;

        public CmdDailyQuestInfoUser(uint uid, object gET)
        {
            this.uid = uid;
            this.gET = gET;
        }
        protected override void lineResult(ctx_res _result, uint _index_result)
        {
        }

        protected override Response prepareConsulta()
        {
            return null;
        }
    }
}