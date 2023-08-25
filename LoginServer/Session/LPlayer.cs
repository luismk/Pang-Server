using PangyaAPI.SuperSocket.Interface;
using PangyaAPI.SuperSocket.SocketBase;
using PangyaAPI.Utilities;
using System;

namespace LoginServer.Session
{
    /// <summary>
    /// class PLayer, aqui voce coloca todos os objetos e metodos que ira usar ao longo do desenvolvimento
    /// </summary>
    public partial class Player : AppSession<Player, PangyaRequestInfo>, IAppSession
    {
        public PlayerInfo m_pi { get; set; }
        public string m_ip => GetAdress;

        public Player()
        {
            m_pi = new PlayerInfo();
        }

        public override string GetNickname()
        {
            return m_pi.nickname;
        }

        public override uint GetUID()
        {
            return (uint)m_pi.uid;
        }

        public override string GetID()
        {
            return m_pi.id;
        }

        public void test(byte[] data, int length)
        {
            var new_data = new byte[length];
            Buffer.BlockCopy(data, 0, new_data, 0, length);
            new_data.DebugDump();
            SocketSession.m_Socket.Send(new_data);
        }
    }
}
