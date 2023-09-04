using PangyaAPI.SQL.DATA.TYPE;
using PangyaAPI.SQL;

using System;
using System.Collections.Generic;
using System.Data;

namespace PangyaAPI.SQL.DATA.Cmd
{
    public enum TYPE_SERVER : byte
    {
        GAME,
        MSN,
        LOGIN,
        RANK,
        AUTH,
    }
    public class CmdServerList: Pangya_DB
    {

        TYPE_SERVER m_type;
        List<ServerInfo> v_server_list;
        protected override string _getName { get; set; } = "CmdServerList";
        public CmdServerList(TYPE_SERVER _type)
        {
            v_server_list = new List<ServerInfo>();
            m_type = _type;
        }

        public CmdServerList()
        {
            v_server_list = new List<ServerInfo>();
        }

        protected override void lineResult(ctx_res _result, uint _index_result)
        {
            checkColumnNumber(13);
            try
            {
                ServerInfo si = new ServerInfo();

                if (!string.IsNullOrEmpty(_result.data[0].ToString()))
                    si.Name = (_result.data[0].ToString());
                si.UID = int.Parse(_result.data[1].ToString());
                if (!string.IsNullOrEmpty(_result.data[2].ToString()))
                    si.IP = _result.data[2].ToString();
                si.Port = int.Parse(_result.data[3].ToString());
                si.MaxUser = int.Parse(_result.data[4].ToString());
                si.Curr_User = int.Parse(_result.data[5].ToString());
                si.Property.ulProperty = uint.Parse(_result.data[6].ToString());
                si.AngelicWingsNum = int.Parse(_result.data[7].ToString());
                si.EventFlag.usEventFlag=(short.Parse(_result.data[8].ToString()));
                si.EventMap = short.Parse(_result.data[9].ToString());
                si.ImgNo = short.Parse(_result.data[10].ToString());
                si.AppRate = short.Parse(_result.data[11].ToString());
                si.Unknown = short.Parse(_result.data[12].ToString());    // Estava o rate_scratchy mas realoquei ele para o ServerInfoEx::Rate

                v_server_list.Add(si);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
        }

        protected override Response prepareConsulta()
        {
            v_server_list.Clear();

            var r = procedure("pangya.ProcGetServerList", new string[] { "@OPT" }, new type_SqlDbType[] { type_SqlDbType.Int }, new string[] { Convert.ToByte(m_type).ToString() }, ParameterDirection.Input);

            checkResponse(r, "nao conseguiu pegar o server list");
            return r;
        }

        public List<ServerInfo> getServerList()
        {
            return this.v_server_list;
        }


        public TYPE_SERVER getType()
        {
            return m_type;
        }

        public void setType(TYPE_SERVER _type)
        {
            m_type = _type;
        }
    }
}
