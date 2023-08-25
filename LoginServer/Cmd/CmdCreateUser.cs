using LoginServer.TYPE;
using PangyaAPI.SQL.DATA.TYPE;
using PangyaAPI.SQL;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PangyaAPI.Utilities;

namespace LoginServer.Cmd
{
    public class CmdCreateUser : Pangya_DB
    {
        string m_id;
        string m_pass;
        string m_ip;
        int m_server_uid;

        int m_uid;
        protected override string _getName { get; set; } = "CmdCreateUser";

        public CmdCreateUser(string _id, string _pass, string _ip, int _server_uid)
        {
            m_id = _id;
            m_ip = _ip;
            m_pass = _pass;
            m_server_uid = _server_uid;
        }

        protected override void lineResult(ctx_res _result, uint _index_result)
        {
            try
            {
                checkColumnNumber(1);

                m_uid = int.Parse(_result.data[0].ToString());
            }
            catch (Exception ex)
            {
                m_exception = (exception)ex;
            }      
        }

        protected override Response prepareConsulta()
        {
            m_uid = 0;

            if (m_id.empty() || m_pass.empty() || m_ip.empty())
                throw new exception("[CmdCreateUser::prepareConsulta][Error] argumentos invalidos.[ID=" + m_id + ",PASSWORD=" + m_pass + ",IP=" + m_ip + "]", STDA_ERROR_TYPE.PANGYA_DB);


            var r = procedure("pangya.ProcNewUser", new string[] { "@UserID", "@pass", "@IPaddr","@serverUID", "@accountUserID" }, new type_SqlDbType[] { type_SqlDbType.NVarChar, type_SqlDbType.NVarChar, type_SqlDbType.NVarChar, type_SqlDbType.Int, type_SqlDbType.Int}, new string[] { m_id.ToString(),m_pass, m_ip, m_server_uid.ToString(), "0" }, ParameterDirection.Input);
            checkResponse(r, "nao conseguiu criar um usuario[ID=" + m_id + ",PASSWORD=" + m_pass + ",IP=" + m_ip + ",SERVER UID=" + (m_server_uid) + "]"); 
            return r;
        }



        public int getUID()
        {
            return m_uid;
        }

        public void setUID(int _uid)
        {
            m_uid = _uid;
        }

        public int getServerUID()
        {
            return m_server_uid;
        }

        public void setServerUID(int _id)
        {
            m_server_uid = _id;
        }

        public int getIP()
        {
            return m_server_uid;
        }

        public void setIP(string _ip)
        {
            m_ip = _ip;
        }
    }
}
