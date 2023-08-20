using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PangyaAPI.Player.Data
{
    public partial class player_info
    {
        public int uid { get; set; }
        public int m_cap { get; set; }
        public BlockFlag block_flag { get; set; }
        public short level { get; set; }
        public string id { get; set; }
        public string nickname { get; set; }
        public string pass { get; set; }
        public player_info()
        {
            block_flag = new BlockFlag();
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
        public LoginData()
        {
            id = "";
            v_opt_unkn = new long[2];
            mac_address = "";
        }
        public override string ToString()
        {
            string data = $": [USER = {id}], [PWD = {password}], [MAC = {mac_address}]";
            return data;
        }
    }
}
