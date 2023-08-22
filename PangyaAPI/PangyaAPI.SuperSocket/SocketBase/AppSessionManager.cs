using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using PangyaAPI.SuperSocket.Interface;
using _smp = PangyaAPI.Utilities.Log;
namespace PangyaAPI.SuperSocket.SocketBase
{
    public class AppSessionManager
    {
        protected List<IAppSession> m_sessions;
        List<IAppSession> m_session_del;    // Session para deletar
        AppServer Server { get; set; }
        protected uint m_max_session;
        uint m_TTL;				// Time To Live
        public IAppSession this[int index]
        {
            get
            {
                return m_sessions[index];
            }
            set
            {
                m_sessions[index] = value;
            }
        }

        public List<IAppSession> Values
        {
            get { return m_sessions.Where(c => c.SocketSession != null).ToList(); }
        }

        public AppSessionManager(int _max_session)
        {
            m_max_session = (uint)_max_session;
            m_sessions = new List<IAppSession>();
            m_session_del = new List<IAppSession>();
            for (uint i = 0; i < _max_session; i++)
            {
                m_sessions.Add(new AppSession());
            }
        }

        public void clear()
        {
            // Limpa as session para deletar
            if (!(m_session_del.Count == 0))
                m_session_del.Clear();

            // Não Vazias
            if (!(m_sessions.Count == 0))
            {

                for (int i = 0; i < m_sessions.Count(); ++i)
                    if (m_sessions[i] != null)
                        m_sessions[i].Close();

                m_sessions.Clear();
            }
        }

        protected uint m_count
        {
            get; set;
        }


        public IAppSession addSession(TcpClient _sock)
        {
            uint index;

#pragma warning disable CS0652 // A comparação com constante integral é inútil. A constante está fora do intervalo do tipo
            if ((index = findSessionFree()) == uint.MaxValue)
            {
                throw new Exception("[session_manager::addSession][ERR_SESSION] already goal limit session estabilized.");
            }
#pragma warning restore CS0652 // A comparação com constante integral é inútil. A constante está fora do intervalo do tipo


            AppSession pSession = null;

            pSession = (AppSession)m_sessions[(int)index];
            pSession.m_oid = index;

            m_count++;
            m_sessions[(int)index] = pSession;
            return pSession;
        }
        public bool deleteSession(AppSession _session)
        {
            if (_session == null)
                throw new Exception("[session_manager::deleteSession][ERR_SESSION] _session is nullptr.");


            bool ret = false;
            if ((ret = _session.Clear()))
                m_count--;

            _session.Dispose();

            return ret;
        }

        public uint getNumSessionOnline() { return m_count; }

        public IAppSession findSessionByOID(uint value)
        {
            for (int i = 0; i < value; i++)
            {
                if (m_sessions[i].m_oid == value)
                    return m_sessions[i];
            }
            return null;
        }

        public IAppSession findSessionByNickname(string _nickname)
        {
            for (int i = 0; i < m_count; i++)
            {
                if (m_sessions[i].SocketSession != null && _nickname == m_sessions[i].GetNickname())
                    return m_sessions[i];
            }
            return null;
        }

        public IAppSession findSessionByUID(uint _uid)
        {
            for (int i = 0; i < m_count; i++)
            {
                if (m_sessions[i].SocketSession != null && _uid == m_sessions[i].GetUID())
                    return m_sessions[i];
            }
            return null;
        }

        public List<IAppSession> findAllSessionByUID(uint _uid)
        {
            List<IAppSession> v_s = new List<IAppSession>();
            for (int i = 0; i < m_count; i++)
            {
                if (m_sessions[i].SocketSession != null && _uid == m_sessions[i].GetUID())
                    v_s.Add(m_sessions[i]);
            }
            return v_s;
        }
        public uint findSessionFree()
        {
            for (int i = 0; i < m_sessions.Count; i++)
            {
                if (m_sessions[i].SocketSession == null)
                {
                    return (uint)i;
                }
            }
            return uint.MaxValue;
        }

        public uint numSessionConnected() { return m_count; }

        public bool isFull()
        {
            bool ret = false;
            uint count = (uint)m_sessions.Where(c => c.SocketSession != null).Count();

            ret = count == m_count;
            return ret;
        }

        public void checkSessionLive(object state)
        {
            if (Monitor.TryEnter(state))
            {
                try
                {
                    var sessionSource = Server.SessionSource;

                    if (sessionSource == null)
                        return;

                    DateTime now = DateTime.Now;
                    DateTime timeOut = now.AddSeconds(0 - Server.Config.IdleSessionTimeOut);

                    var timeOutSessions = sessionSource.Where(s => s.Value.LastActiveTime <= timeOut).Select(s => s.Value);

                    Parallel.ForEach(timeOutSessions, s =>
                    {
                        _smp.Message_Pool.push(string.Format("The session will be closed for {0} timeout, the session start time: {1}, last active time: {2}!", now.Subtract(s.LastActiveTime).TotalSeconds, s.StartTime, s.LastActiveTime));

                        s.Close(CloseReason.TimeOut);
                    });
                }
                catch (Exception e)
                {
                    _smp.Message_Pool.push("Clear idle session error!", e);
                }
                finally
                {
                    Monitor.Exit(state);
                }
            }

        }
    }
}
