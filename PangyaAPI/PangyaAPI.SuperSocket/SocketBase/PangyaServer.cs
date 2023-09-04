using PangyaAPI.IFF.Handle;
using PangyaAPI.SuperSocket.Interface;
using PangyaAPI.Utilities;
using System;

namespace PangyaAPI.SuperSocket.SocketBase
{
    public abstract partial class PangyaServer<T> : AppServer<T, PangyaRequestInfo>
     where T : AppSession<T, PangyaRequestInfo>, IAppSession, new()
    {
        public PangyaServer() :base(new DefaultReceiveFilterFactory<PangyaReceiveFilter, PangyaRequestInfo>())
        {
            try
            {
                if (LoadingFiles())
                {
                    ConfigInit();
                    IFF = new IFFHandle("data//pangya_gb.iff");
                }
                NewRequestReceived += ProcessNewMessage;
            }
            catch (Exception ex)
            {
                WriteConsole.Error(ex.Message);
            }
        }
        private void ProcessNewMessage(T session, PangyaRequestInfo requestInfo)
        {
            if (requestInfo == null)
            {
                OnSessionClosed(session);
            }
            else
            {
                dispach_packet_same_thread(session, requestInfo._packet);
            }
        }

        public bool StartingServer()
        {
            try
            {
                var result = Setup(m_si.IP, m_si.Port, m_si.Name);
                if (result == false)
                {
                    Console.WriteLine("Failed to Setup!");
                    Console.ReadKey();
                }
                result = Start();
                if (result == false)
                {
                    Console.WriteLine("Failed to start!");
                    Console.ReadKey();
                }
                return result;
            }
            catch (Exception ex)
            {
                WriteConsole.Error(ex.Message);
                return false;
            }
        }


        protected override void OnNewSessionConnected(T session)
        {
            onAcceptCompleted(session);
            if (session.Connected)
            {
                WriteConsole.WriteLine($"[AppServer::onAcceptCompleted][Log] Player[OID => {session?.m_oid}, ID => {session?.GetID()}, Connection => {session?.GetAdress}]", ConsoleColor.Green);
            }
            m_si.Curr_User = SessionCount;
            base.OnNewSessionConnected(session);
        }

        protected override void OnSessionClosed(T session, CloseReason reason = CloseReason.ClientClosing)
        {
            base.OnSessionClosed(session, reason);
            m_si.Curr_User = SessionCount;
        }
    }
}
