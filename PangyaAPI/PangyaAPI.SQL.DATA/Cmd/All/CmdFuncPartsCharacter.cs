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
    public class CmdFuncPartsCharacter : Pangya_DB
    {
        private readonly int m_uid;
        private readonly int m_typeid;

        protected override string _getName { get; set; } = "CmdFuncPartsCharacter";
        public CmdFuncPartsCharacter(int _uid,  int _typeid)
        {
            m_uid = _uid;
            m_typeid = _typeid;
        }

        protected override void lineResult(ctx_res _result, uint _index_result)
        {
           //e um update
        }

        protected override Response prepareConsulta(database _db)
        {

            var r = procedure(_db, "pangya.FuncConcertaPartsCharacter",
                new string[]
                {
                    "@IDUSER",
                    "@_TYPEID",
                }, new type_SqlDbType[]
                {
                    type_SqlDbType.Int,
                    type_SqlDbType.Int,
                   
                }, new string[]
            {
                m_uid.ToString(),
               
               m_typeid.ToString()
            }, ParameterDirection.Input);
            checkResponse(r, "nao conseguiu concertar o character[TYPEID=" + (m_typeid) + "] para o player: " + (m_uid));
            return r;
        }
    }
}
