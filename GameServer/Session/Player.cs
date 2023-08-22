using GameServer.TYPE;
using PangyaAPI.SuperSocket.Interface;
using PangyaAPI.SuperSocket.SocketBase;

namespace GameServer.Session
{
    public class Player : AppSession<Player, PangyaRequestInfo>, IAppSession
    {
        public PlayerInfo m_pi { get; set; }
        public GMInfo m_gi { get; set; }
       
        public Player()
        {
            m_pi = new PlayerInfo();
            m_gi = new GMInfo();
        }
        public string m_ip => GetAdress;
        public override string GetNickname()
        {
            return m_pi.nickname;
        }

        public override uint GetUID()
        {
            return ((uint)m_pi.uid);
        }

        public override string GetID()
        {
            return m_pi.id;
        }
        public string getIP()
        {
            return GetAdress;
        }
        public virtual int getCapability() { return (int)m_pi.m_cap.ulCapability; }

    }
}
