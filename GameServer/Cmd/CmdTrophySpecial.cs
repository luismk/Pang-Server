using PangyaAPI.SQL;

namespace GameServer.Cmd
{
    internal class CmdTrophySpecial : Pangya_DB
    {
        private uint uid;
        private object cURRENT;
        private object nORMAL;

        public CmdTrophySpecial(uint uid, object cURRENT, object nORMAL)
        {
            this.uid = uid;
            this.cURRENT = cURRENT;
            this.nORMAL = nORMAL;
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