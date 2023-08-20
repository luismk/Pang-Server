using System;
using PangyaAPI.Utilities;
using System.Data;
using response = PangyaAPI.SQL.Response;
using static PangyaAPI.SQL.DefineConstants;
namespace PangyaAPI.SQL
{
    public abstract class database : IDisposable
	{
		public enum ERROR_TYPE : uint
		{
			INVALID_HANDLE,
			INVALID_PARAMETER,
			ALLOC_HANDLE_FAIL_ENV,
			ALLOC_HANDLE_FAIL_DBC,
			ALLOC_HANDLE_FAIL_STMT,
			SET_ATTR_ENV_FAIL,
			CONNECT_DRIVER_FAIL,
			EXEC_QUERY_FAIL,
			FETCH_QUERY_FAIL,
			MORE_RESULTS,
			GERAL_ERROR,
			HAS_CONNECT
		}
		public database()
        {
			loadIni();
			this.m_state = false;
			this.m_connected = false;
		}
		public database(string _db_address,
			string _db_name,
			string _user_name,
			string _user_pass,
			short _db_port)
		{
			this.m_db_address = _db_address;
			this.m_db_name = _db_name;
			this.m_user_name = _user_name;
			this.m_user_pass = _user_pass;
			this.m_db_port = Convert.ToUInt16(_db_port);
			this.m_state = false;
			this.m_connected = false;

		}
		public virtual void Dispose()
		{
		}

		public abstract void init();

		public bool is_valid()
		{
			return m_state;
		}

		public bool is_connected()
		{
			return m_connected;
		}

		public abstract bool hasGoneAway();

		public abstract void connect();
		public abstract void connect(string _db_address,
			string _db_name,
			string _user_name,
			string _user_pass,
			ushort _db_port);
		public abstract void reconnect();
		public abstract void disconnect();

		public abstract response ExecQuery(string _query);
		public abstract response ExecProc(string _proc_name, string[] _params, type_SqlDbType[] tipo = null, string[] valor = null, ParameterDirection Direcao = ParameterDirection.Input);
		public abstract string makeText(string _value);

		public abstract string makeEscapeKeyword(string _value);
		public string parseEscapeKeyword(string _value)
		{
			return System.Text.RegularExpressions.Regex.Replace(_value,
				DB_ESCAPE_KEYWORD_A + "([\\w\\.\\-_\\s]+)" + DB_ESCAPE_KEYWORD_A,
				this.makeEscapeKeyword("$1"));
		}


		private bool members_empty()
		{
			return (string.IsNullOrEmpty(m_db_name) || string.IsNullOrEmpty(m_db_address) || string.IsNullOrEmpty(m_user_name) || string.IsNullOrEmpty(m_user_pass));
		}

		public bool loadIni()
		{
			IniHandle ini = new IniHandle("server.ini");
			m_db_address = ini.ReadString("NORMAL_DB", "DBIP");
			m_db_name = ini.ReadString("NORMAL_DB", "DBNAME");
			m_user_name = ini.ReadString("NORMAL_DB", "DBUSER");
			m_user_pass = ini.ReadString("NORMAL_DB", "DBPASS");
			m_db_port = ini.ReadUInt16("NORMAL_DB", "DBPORT", 1433);

			return true;
		}

		protected bool m_state;
		protected bool m_connected;

		protected string m_db_address;
		protected string m_db_name;
		protected string m_user_name;
		protected string m_user_pass;
		protected ushort m_db_port;
	}

internal static class DefineConstants
{
	public const string DB_ESCAPE_KEYWORD_A = "-//";
#if !_WIN32 && __linux__
	public const int INFINITE = -1;
#endif
#if !_WIN32 && __linux__
	public const int ERROR_SUCCESS = 0;
#endif
#if !_WIN32 && __linux__
	public const int INVALID_HANDLE_VALUE = -1;
#endif
#if !_WIN32 && __linux__
	public const int SOCKET_ERROR = -1;
#endif
#if !_PATH_SEPARETOR && _WIN32
	public const string _PATH_SEPARETOR = "\\";
#endif
#if !_PATH_SEPARETOR && !_WIN32 && __linux__
	public const string _PATH_SEPARETOR = "/";
#endif
#if !_INI_PATH && _WIN32
	public const string _INI_PATH = "\\server.ini";
#endif
#if !_INI_PATH && !_WIN32 && __linux__
	public const string _INI_PATH = "/server.ini";
#endif
}

}
