using PangyaAPI.SQL;

namespace GameServer.Cmd
{
    internal class CmdGrandZodiacPontos : Pangya_DB
    {
        private uint uid;
        private object cGZT_GET;

        public CmdGrandZodiacPontos(uint uid, object cGZT_GET)
        {
            this.uid = uid;
            this.cGZT_GET = cGZT_GET;
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