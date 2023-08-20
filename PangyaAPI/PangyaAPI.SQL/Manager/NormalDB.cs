using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using _smp = PangyaAPI.Utilities.Log;
using PangyaAPI.Utilities;
using PangyaAPI.SQL.Manager;
namespace PangyaAPI.SQL
{
    public delegate void callback_response(int _msg_id,
        Pangya_DB _db,
        object _arg);

    public class NormalDB
    {
        public class msg_t : IDisposable
        {
            public msg_t()
            {
                this.id = 0;
                this._pangya_db = null;
                this.func = null;
                this.arg = null;
            }

            public msg_t(msg_t t)
            {

            }
            public msg_t(int _id,
                Pangya_DB __pangya_db,
                Action<int, Pangya_DB, object> _callback_response,
                object _arg)
            {
                this.id = _id;
                this._pangya_db = __pangya_db;
                this.func = _callback_response;
                this.arg = _arg;
            }
            public void Dispose()
            { // Destrutor

                // Libera memória allocada no pangya_db, se for para desalocar ela mesmo aqui
                if (_pangya_db != null && !_pangya_db.isWaitable())
                {
                    if (_pangya_db != null)
                    {
                        _pangya_db.Dispose();
                    }
                }

                _pangya_db = null;
            }
            public void clear()
            {

            }
            public void execFunc()
            {
                if (func == null)
                {
                    return;
                }

                try
                {
                    if (_pangya_db == null)
                    {
                        throw new System.Exception("_pangya_db is nullptr");
                    }

                    func.Invoke(id, _pangya_db, arg);
                    sucess = true;
                }
                catch (exception e)
                {
                    _smp.Message_Pool.push("[NormalDB::mgs_t::execFunc][Error] " + e.getFullMessageError());
                }
            }
            public void execQuery(database _db)
            {
                if (_db.hasGoneAway()
                    || !_db.is_connected()
                    || !_db.is_valid())
                {
                    throw new System.Exception("[NormalDB::msg_t::execQuery][Error] argument database _db is invalid");
                }

                if (_pangya_db == null)
                {
                    throw new System.Exception("[NormalDB::mgs_t::execQuery][Error] _pangya_db is nullptr");
                }
                sucess = false;
                _pangya_db.exec();
            }
            public void execQuery()
            {

                if (_pangya_db == null)
                {
                    throw new System.Exception("[NormalDB::mgs_t::execQuery][Error] _pangya_db is nullptr");
                }
                sucess = false;
                _pangya_db.exec();
            }
            public void wakeupWaiter()
            {
                if (_pangya_db == null)
                {
                    throw new System.Exception("[NormalDB::mgs_t::wakeupWaiter][Error] _pangya_db is nullptr");
                }

                _pangya_db.wakeupWaiter();
                _pangya_db = null;
            }
            public bool isWaitable()
            {
                if (_pangya_db == null)
                {
                    throw new System.Exception("[NormalDB::mgs_t::isWaitable][Error] _pangya_db is nullptr");
                }

                return _pangya_db.isWaitable();
            }
            public void setException(string _exception_msg)
            {
                if (_pangya_db == null)
                {
                    throw new System.Exception("[NormalDB::mgs_t::setException][Error] _pangya_db is nullptr");
                }

                _pangya_db.m_exception = new exception("[NormalDB::msg_t::setException][Error] " + "" + " . " + _exception_msg);
            }
            protected int id; // ID da msg
            protected Pangya_DB _pangya_db;
            protected Action<int, Pangya_DB, object> func;
            protected object arg;
            public bool sucess;
        }

        public enum TT_DB : uint
        {
            TT_NORMAL_EXEC_QUERY,
            TT_NORMAL_RESPONSE
        }


        public NormalDB()
        {
            m_unExec = new List<msg_t>();
            m_unResponse = new List<msg_t>();
        }

        public void Dispose() { }


        public void init()
        {

            loadIni();

            _db = new mssql(m_ctx_db.ip, m_ctx_db.name, m_ctx_db.user, m_ctx_db.pass, m_ctx_db.port);

            _db.connect();

            m_pExec = new Thread(new ThreadStart(runExecQuery))
            {
                IsBackground = true
            };
            m_pExec.Start();
            m_pResponse = new Thread(new ThreadStart(runResponse))
            {
                IsBackground = true
            };
            m_pResponse.Start();
        }

        public void sendCloseAndWait() { }



        public int add(msg_t _msg) { m_unExec.Add(_msg); return 0; }

        public int add(int _id, Pangya_DB _pangya_db, Action<int, Pangya_DB, object> _callback_response, object _arg)
        {
            return add(new msg_t(_id, _pangya_db, _callback_response, _arg));
        }


        public void checkIsDeadAndRevive() { }


        public void freeAllWaiting(string _msg)
        {

            foreach (var el in m_unExec)
            {

                if (forceWakeMsg(el, _msg))
                    continue;

                // push again msg not waitable
                m_unExec.Add(el);
            }
        }


        void close() { }


        bool forceWakeMsg(msg_t _msg, string _exception_msg)
        {
            if (_msg == null || !_msg.isWaitable())
                return false;

            _msg.setException(_exception_msg);

            _msg.wakeupWaiter();


            _msg = null;

            return true;
        }


        public void runExecQuery()
        {
            if (m_unExec.Count > 0)
            {
                msg_t _msg = m_unExec[0];
                _msg.execQuery(_db);
                if (m_unResponse.Any(c => c == _msg) == false)
                {
                    m_unResponse.Add(_msg);
                }

                if (m_unResponse.Any(c => c == _msg))
                {
                    m_unResponse.Remove(_msg);
                    m_unExec.Remove(_msg);
                }
            }
        }

        void runResponse()
        {
            bool sai = false;
            if (m_unResponse.Count > 0)
            {
                msg_t _msg = m_unResponse[0];
                if (_msg != null)
                    _msg.execFunc();
            }

        }

        public bool loadIni()
        {
            IniHandle ini = new IniHandle("server.ini");

            m_ctx_db.ip = ini.ReadString("NORMAL_DB", "DBIP");
            m_ctx_db.name = ini.ReadString("NORMAL_DB", "DBNAME");
            m_ctx_db.user = ini.ReadString("NORMAL_DB", "DBUSER");
            m_ctx_db.pass = ini.ReadString("NORMAL_DB", "DBPASS");
            m_ctx_db.port = ini.ReadInt16("NORMAL_DB", "DBPORT", 1433);
            return true;
        }

        protected Thread m_pExec;
        protected Thread m_pResponse;

        protected ctx_db m_ctx_db = new ctx_db();

        protected bool m_state;
        protected uint m_continue_exec;
        protected uint m_continue_response;
        protected uint m_free_all_waiting;

        List<msg_t> m_unExec = new List<msg_t>();
        List<msg_t> m_unResponse = new List<msg_t>();
        private mssql _db;
    }
}
