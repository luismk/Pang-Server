using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginServer.TYPE
{
    public partial class player_info
    {
        public int uid { get; set; }
        public int m_cap { get; set; }
        public PangyaAPI.SQL.DATA.TYPE.BlockFlag block_flag { get; set; }
        public short level { get; set; }
        public string id { get; set; }
        public string nickname { get; set; }
        public string pass { get; set; }
        public player_info()
        {
            block_flag = new PangyaAPI.SQL.DATA.TYPE.BlockFlag();
            id = "";
            nickname = "";
            pass = "";
        }
        public PlayerInfo GetInfo()
        {
            return new PlayerInfo(id, block_flag, level, m_cap, nickname, pass, uid);
        }
    }
    public class LoginData
    {
        public string id { get; set; }
        public string password { get; set; }//
        public byte opt_count { get; set; }
        public long[] v_opt_unkn { get; set; } = new long[2];
        public string mac_address { get; set; }
        public new string ToString()
        {
            string data = $": [USER = {id}], [PWD = {password}], [MAC = {mac_address}]";
            return data;
        }
    }
}