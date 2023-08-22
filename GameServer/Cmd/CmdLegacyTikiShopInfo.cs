using PangyaAPI.SQL;

namespace GameServer.Cmd
{
    internal class CmdLegacyTikiShopInfo : Pangya_DB
    {
        private uint uid;

        public CmdLegacyTikiShopInfo(uint uid)
        {
            this.uid = uid;
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