
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
    public class CmdListIpBan: Pangya_DB
    {
        List<string> v_list_ip_ban;
        protected override string _getName { get; set; } = "CmdListIpBan";

        public CmdListIpBan() : this(true)
        {
            v_list_ip_ban =new List<string>();
        }

        public CmdListIpBan(bool wait = false) : base(wait)
        {
        }

        protected override void lineResult(ctx_res _result, uint _index_result)
        {
            checkColumnNumber(_result.cols);
            try
            {
                if (!_result.IsNotNull(0))
                    v_list_ip_ban.Add(_result.data[0].ToString());
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
        }

        protected override Response prepareConsulta(database _db)
        {
            v_list_ip_ban.Clear();

            var r = consulta(_db, "SELECT ip FROM pangya.pangya_ip_table");

            checkResponse(r, "nao conseguiu recuperar a lista de MAC Address");
            return r;
        }


        public List<string> getList()
        {
            return v_list_ip_ban;
        }

    }
}
