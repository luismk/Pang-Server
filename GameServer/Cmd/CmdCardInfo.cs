using PangyaAPI.SQL;

namespace GameServer.Cmd
{
    internal class CmdCardInfo : Pangya_DB
    {
        private uint uid;
        private object aLL;

        public CmdCardInfo(uint uid, object aLL)
        {
            this.uid = uid;
            this.aLL = aLL;
        }
        protected override void lineResult(ctx_res _result, uint _index_result)
        {
        }

        protected override Response prepareConsulta(database _db)
        {
            return null;
        }
    }
}