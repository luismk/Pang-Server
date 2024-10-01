using LoginServer.TYPE;
using PangyaAPI.SQL.DATA.TYPE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginServer
{
    public class PlayerInfo : player_info
    {
        public PlayerInfo()
        {

        }

        public PlayerInfo(string _id, BlockFlag _block_flag, short _level, int cap, string nick, string pwd, int _uid)
        {
            this.id = _id;
            this.block_flag = _block_flag;
            this.level = _level;
            this.m_cap = cap;
            this.nickname = nick;
            this.pass = pwd;
            this.uid = _uid;
        }

        public byte m_state { get; set; }
        public byte m_place { get; set; }
        public int m_server_uid { get; set; }       // Server UID em que player está conectado
    }         
}
