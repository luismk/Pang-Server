using PangyaAPI.SQL;

namespace GameServer.Cmd
{
    internal class CmdGrandPrixClear : Pangya_DB
    {
        private uint uid;

        public CmdGrandPrixClear(uint uid)
        {
            this.uid = uid;
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