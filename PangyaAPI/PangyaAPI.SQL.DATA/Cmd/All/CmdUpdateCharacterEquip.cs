
using PangyaAPI.SQL.DATA.TYPE;
using PangyaAPI.SQL;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PangyaAPI.SQL.DATA.Cmd
{
    public class CmdUpdateCharacterEquip: Pangya_DB
    {
        int m_uid;
        int m_character_id;
        protected override string _getName { get; set; } = "CmdUpdateCharacterEquip";
        public CmdUpdateCharacterEquip(int _uid, int character_id) : this(true)
        {
            m_uid = _uid;
            m_character_id = character_id;
        }

        public CmdUpdateCharacterEquip(bool _wait) : base(_wait)
        {
        }

        protected override void lineResult(ctx_res _result, uint _index_result)
        {
            //so um update
            return;
        }

        protected override Response prepareConsulta(database _db)
        {

            var r = procedure(_db, "pangya.USP_FLUSH_CHARACTER",
                new string[]
                {
                    "@IDUSER",
                    "@IDCHARACTER",
                }, new type_SqlDbType[]
                {
                    type_SqlDbType.Int,
                    type_SqlDbType.Int
                }, new string[]
            {
                m_uid.ToString(),
                m_character_id.ToString()
            }, ParameterDirection.Input);
            checkResponse(r, "nao conseguiu atualizar o character[ID=" + (m_character_id) + "] equipado do player: " + (m_uid));
            return r;
        }

        public int getUID()
        {
            return m_uid;
        }

        public void setUID(int _uid)
        {
            m_uid = _uid;
        }

        public int getCharacterID()
        {
            return m_character_id;
        }

        public void setCharacterID(int charID)
        {
            m_character_id = charID;
        }
    }
}
