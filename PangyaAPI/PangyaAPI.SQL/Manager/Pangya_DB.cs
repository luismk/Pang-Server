using PangyaAPI.Utilities;
using result_set = PangyaAPI.SQL.Result_Set;
using response = PangyaAPI.SQL.Response;
using PangyaAPI.SQL.Manager;
using System;
namespace PangyaAPI.SQL
{
    public abstract partial class Pangya_DB
    {
        protected ctx_db m_ctx_db = new ctx_db();
        protected mssql _db = new mssql();
        public Pangya_DB()

        {
            m__exception = new exception("");
            loadIni();
            _db = new mssql(m_ctx_db.ip, m_ctx_db.name, m_ctx_db.user, m_ctx_db.pass, m_ctx_db.port);
            _db.connect();
        }


        public void Dispose() { }

        bool loadIni()
        {
            IniHandle ini = new IniHandle("server.ini");

            m_ctx_db.ip = ini.ReadString("NORMAL_DB", "DBIP");
            m_ctx_db.name = ini.ReadString("NORMAL_DB", "DBNAME");
            m_ctx_db.user = ini.ReadString("NORMAL_DB", "DBUSER");
            m_ctx_db.pass = ini.ReadString("NORMAL_DB", "DBPASS");
            m_ctx_db.port = ini.ReadInt16("NORMAL_DB", "DBPORT", 1433);
            return true;
        }
        public virtual void exec()
        {
            loadIni();
            uint num_result = 0;
            try
            {
                response r = null;
                if ((r = prepareConsulta()) != null)
                {
                    foreach (var _result in r.getResultSet())      
                    {
                        lineResult(_result.getFirstLine(), num_result);
                        num_result++;
                    }                       
                    clear_response(r);
                }
            }
            catch (Exception e)
            {
                m_exception = new exception(e.Message);
            }
        }

        public virtual exception getException() { return m_exception; }


        public virtual void ExecCmd() { exec(); }

        public virtual response _insert(string _query)
        {
            return _db.ExecQuery(_query);
        }
        public virtual response _update(string _query) { return _db.ExecQuery(_query); }

        public virtual response _delete(string _query) { return _db.ExecQuery(_query); }

        public virtual response consulta(string _query) { return _db.ExecQuery(_query); }

        public virtual response procedure(string _name, string values = null) { return _db.ExecProc(_name, values); }



        public virtual void postAndWaitResponseQuery(exec_query _query)
        {
            _query.waitEvent(-1); //wait
        }


        public virtual void clear_result(result_set _rs) { }

        public virtual void clear_response(response _res) { }

        public virtual void checkColumnNumber(uint _number_cols1)
        {
            if (_number_cols1 <= 0)
                throw new exception("[Pangya_DB::" + _getName + "::checkColumnNumber][Error] numero de colunas retornada pela consulta sao diferente do esperado.");
        }
        public virtual void checkColumnNumber(uint _number_cols1, uint _number_cols2)
        {
            if (_number_cols1 != 0 && _number_cols1 != _number_cols2)
                throw new exception("[Pangya_DB::" + _getName + "::checkColumnNumber][Error] numero de colunas retornada pela consulta sao diferente do esperado.");
        }

        public virtual void checkResponse(response r, string _exception_msg)
        {
            if (r == null || (r.getNumResultSet() <= 0 && r.getRowsAffected() == -1))
                throw new exception("[Pangya_DB::" + _getName + "::checkResponse][Error] " + _exception_msg);
        }

        protected abstract void lineResult(ctx_res _result, uint _index_result);
        protected abstract response prepareConsulta();

        protected virtual string _getName { get => GetType().Name; }

        public static bool is_valid_c_string(ref string _ptr_c_string)
        {
            return _ptr_c_string != null && _ptr_c_string[0] != 0;
        }       

        protected exception m__exception { get; set; }
        public exception m_exception { get => m__exception; set => m__exception = value; }
    }
}