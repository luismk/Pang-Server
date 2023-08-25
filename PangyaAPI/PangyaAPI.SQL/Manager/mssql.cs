using System;
using Microsoft.VisualBasic.CompilerServices;
using PangyaAPI.Utilities;
using System.Data;
using result_set = PangyaAPI.SQL.Result_Set;
using response = PangyaAPI.SQL.Response;
using System.Data.SqlClient;

namespace PangyaAPI.SQL.Manager
{
    public class mssql : database
    {

        public string connect_str => (("Data Source=") + m_db_address + ";DATABASE=" + m_db_name + ";UID=" + m_user_name + ";PWD=" + m_user_pass + "; Min Pool Size=27;Max Pool Size=250; Connect Timeout=6");

        public mssql()
        { 
        
        }
        public mssql(string _db_address, string _db_name, string _user_name, string _user_pass, short _db_port) : base(_db_address, _db_name, _user_name, _user_pass, _db_port) { }

        ~mssql()
        {
            if (m_ctx.hDbc != null)
                m_ctx.hDbc.Dispose();

            if (m_ctx.hEnv != null)
                m_ctx.hEnv.Dispose();

            if (m_ctx.hStmt != null)
                m_ctx.hStmt.Dispose();
            init();
        }
        public override void init()
        {
            m_ctx.hDbc = new SqlConnection();
            m_ctx.hEnv = new SqlCommand();
            m_ctx.hStmt = new DataSet();
            m_state = true;
        }

        public virtual void destroy() 
        {

            if (is_connected())
                disconnect();

            if (m_ctx.hDbc != null)
                m_ctx.hDbc = null;

            if (m_ctx.hEnv != null)
                m_ctx.hEnv = null;

            m_state = false;
        }


        public override bool hasGoneAway()
        {
            return false;
        }


        public override void connect()
        {
            try
            {

                if (!is_valid())
                    init();

                if (is_connected())
                    throw new exception("[mssql::connect][Error] Ja esta connectado.");

                if (m_ctx.hDbc != null && m_ctx.hDbc.State == ConnectionState.Closed)
                {
                    m_ctx.hDbc.ConnectionString = connect_str;
                    m_ctx.hDbc.Open();
                }

                m_connected = true;
            }
            catch (Exception projectError)
            {
                ProjectData.SetProjectError(projectError);
                ProjectData.ClearProjectError();
                m_connected = false;
            }
            finally
            {
                m_ctx.hDbc.Close();
            }
        }

        public override void connect(string _db_address, string _db_name, string _user_name, string _user_pass, ushort _db_port) 
        {
            m_db_address = _db_address;
            m_db_name = _db_name;
            m_user_name = _user_name;
            m_user_pass = _user_pass;
            m_db_port = _db_port;

            connect();
        }

        public override void reconnect()
        {
            disconnect();
            connect();
        }

        public override void disconnect() {
            if (is_connected())
            {

                if (m_ctx.hDbc != null)
                    m_ctx.hDbc.Close();
            }

            m_connected = false;
        }


        public override response ExecQuery(string _query)
        {
            response res = new response();
            result_set result = null;
            uint numResults = 0;
            int numRows = 0;
            int i;
            try
            {
                HandleDiagnosticRecord(_query);
                if (m_ctx.hStmt != null)
                {
                    var _data = m_ctx.hStmt.Tables[m_db_name];
                    if (_data.Rows.Count == 1)
                    {
                        numResults = 1;
                    }
                    if (_data.Rows.Count > 1)
                    {
                        numResults = (uint)_data.Rows.Count - 1;
                    }
                    numRows = _data.Columns.Count;
                    res.setRowsAffected(numRows);
                    var ret = 0;
                    if (numResults > 0)
                    {
                        result = new result_set((uint)result_set.STATE_TYPE.HAVE_DATA, numResults, numRows);
                        while (ret < numResults)
                        {
                            for (i = 0; i < numResults; i++)
                            {
                                result.addLine();   // Adiciona linha
                                result.setRow(_data.Rows[i]);
                                res.addResultSet(result);
                            }
                            ret++;
                        }
                        res.addResultSet(result);
                    }
                }
                return res;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return res;
            }
        }
        public override response ExecProc(string _proc_name, string[] _params, type_SqlDbType[] tipo = null, string[] valor = null, ParameterDirection Direcao = ParameterDirection.Input)
        {
            response res = new response();
            result_set result = null;
            uint numResults = 0;
            int numRows = 0;
            int i;
            try
            {
                HandleDiagnosticRecord(_proc_name, _params, tipo, valor, Direcao);
                if (m_ctx.hStmt != null && m_ctx.hStmt.Tables[m_db_name] != null)
                {
                    var _data = m_ctx.hStmt.Tables[m_db_name];
                    if (_data != null && _data.Rows.Count == 1)
                    {
                        numResults = 1;
                    }
                    if (_data.Rows.Count > 1)
                    {
                        numResults = (uint)_data.Rows.Count - 1;
                    }
                    numRows = _data.Columns.Count;
                    res.setRowsAffected(numRows);
                    var ret = 0;
                    if (numResults > 0)
                    {
                        result = new result_set((uint)result_set.STATE_TYPE.HAVE_DATA, numResults, numRows);
                        for (i = 0; i < numResults; i++)
                        {
                            result.addLine();   // Adiciona linha
                            result.setRow(_data.Rows[i]);
                            res.addResultSet(result);
                        }
                    }
                }
                return res;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return res;
            }
        }
        public override string makeText(string _value)
        {
            return "N'" + _value + "'";
        }
        public override string makeEscapeKeyword(string _value)
        {
            return "[" + _value + "]";
        }


        public virtual void clear_stmt(string _query) { }

        protected class ctx_db
        {

            public void clear()
            {

            }
            public SqlCommand hEnv;
            public SqlConnection hDbc;
            public DataSet hStmt;
        }

        protected ctx_db m_ctx = new ctx_db();
        protected void HandleDiagnosticRecord(string query)
        {
            if (m_ctx.hDbc != null)
            {
                m_ctx.hDbc = new SqlConnection(connect_str);
                m_ctx.hDbc.Open();
                var da = new SqlDataAdapter(query, m_ctx.hDbc);
                da.Fill(m_ctx.hStmt, m_db_name);
                m_ctx.hDbc.Close();
            }
        }

        protected void HandleDiagnosticRecord(string _proc_name, string[] parameter = null, type_SqlDbType[] tipo = null, string[] valor = null, ParameterDirection Direcao = ParameterDirection.Input)
        {
            try
            {
                if (m_ctx.hDbc != null)
                {
                    m_ctx.hDbc = new SqlConnection(connect_str);
                    m_ctx.hDbc.Open();
                    m_ctx.hEnv = new SqlCommand(m_db_name + "." + _proc_name, m_ctx.hDbc)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    if (parameter != null && parameter.Length > 0)
                    {
                        for (int i = 0; i < parameter.Length; i++)
                        {
                            m_ctx.hEnv.Parameters.Add(new SqlParameter
                            {
                                ParameterName = parameter[i],
                                Value = valor[i],
                                SqlDbType = (SqlDbType)tipo[i],
                                Direction = Direcao
                            });
                        }
                    }
                    var da = new SqlDataAdapter(m_ctx.hEnv);
                    da.Fill(m_ctx.hStmt, m_db_name);
                }

            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                m_ctx.hDbc.Dispose();
            }
        }
    }
}
