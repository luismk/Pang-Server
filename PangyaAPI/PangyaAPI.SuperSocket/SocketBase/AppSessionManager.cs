using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using PangyaAPI.SuperSocket.Interface;
using PangyaAPI.Utilities;
using static System.Collections.Specialized.BitVector32;

namespace PangyaAPI.SuperSocket.SocketBase
{
    public class AppSessionManager
    {
        private static uint m_count = 0u;
        private static bool m_is_init = false;
        private object m_cs = new object(); // Use object for locking in C#
        private List<AppSession> m_sessions = new List<AppSession>();

       
        private List<AppSession> m_sessionsDel = new List<AppSession>();
        private int m_max_session;
        public AppSessionManager(int maxSession)
        {
            m_max_session = maxSession;
            // Load config from server.ini
            ConfigInit();

            m_is_init = true;

            for (int i = 0; i < maxSession; i++)
            {
                Add(new AppSession());
            }
        }

        protected void Add(AppSession appSession)
        {
            m_sessions.Add(appSession);
        }

        public void Clear()
        {
            lock (m_cs)
            {
                m_sessionsDel.Clear();
                foreach (var session in m_sessions)
                {
                    if (session != null)
                    {
                        session.Dispose();
                    }
                }
                m_sessions.Clear();
            }
        }

        public virtual bool DeleteSession(AppSession session)
        {
            bool ret = true;

            lock (m_cs)
            {
                int oid = (int)session.m_oid;
                if ((ret = session.Clear()))
                {
                    if (m_count != 0)
                    {
                        m_count--;
                    }
                }
            }

            return ret;
        }

        // Other methods...

        private void ConfigInit()
        {
            // Load config from INI file and set m_TTL
        }

        private uint FindSessionFree()
        {
            for (int i = 0; i < m_max_session; ++i)
            {
                if (m_sessions[i].SocketSession == null)
                {
                    return Convert.ToUInt32(i);
                }
            }
            return uint.MaxValue;
        }


        public uint NumSessionOnline
        {
            get
            {
                uint curr_online = 0;
                lock (m_cs)
                {
                    curr_online = m_count;
                }
                return curr_online;
            }
        }

        public AppSession FindSessionByOID(uint _oid)
        {
            AppSession _session = null;
            lock (m_cs)
            {
                _session = (AppSession)m_sessions.FirstOrDefault(el =>
                {
                    return el.SocketSession != null &&
                           el.m_oid == _oid;
                });
            }
            return _session;
        }

        public AppSession FindSessionByUID(uint _uid)
        {
            AppSession _session = null;
            lock (m_cs)
            {
                _session = (AppSession)m_sessions.FirstOrDefault(el =>
                {
                    return el.SocketSession != null &&
                           el.GetUID() == _uid;
                });
            }
            return _session;
        }

        public List<AppSession> FindAllSessionByUID(uint _uid)
        {
            List<AppSession> v_s = new List<AppSession>();
            lock (m_cs)
            {
                v_s = m_sessions.Where(el =>
                {
                    return el.SocketSession != null &&
                           el.GetUID() == _uid;
                }).ToList();
            }
            return v_s;
        }

        public AppSession FindSessionByNickname(string _nickname)
        {
            AppSession s = null;
            lock (m_cs)
            {
                s = (AppSession)m_sessions.FirstOrDefault(el =>
                {
                    return el.SocketSession != null &&
                           string.Equals(el.GetNickname(), _nickname);
                });
            }
            return s;
        }
        public bool IsFull()
        {
            bool ret = false;
            lock (m_cs)
            {
                ret = m_sessions.Count(session =>
                {
                    return session.SocketSession != null;
                }) == m_sessions.Count;
            }
            return ret;
        }

    }
}
