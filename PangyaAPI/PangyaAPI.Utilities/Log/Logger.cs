﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PangyaAPI.Utilities.Log
{
    public enum type_msg : int
    {
        CL_ONLY_CONSOLE,
        CL_FILE_TIME_LOG_AND_CONSOLE,
        CL_FILE_LOG_AND_CONSOLE,
        CL_ONLY_FILE_LOG,
        CL_ONLY_FILE_TIME_LOG,
        CL_ONLY_FILE_LOG_IO_DATA,
        CL_FILE_LOG_IO_DATA_AND_CONSOLE,
        CL_ONLY_FILE_LOG_TEST,
        CL_FILE_LOG_TEST_AND_CONSOLE,
    };
    public class Message : IDisposable
    {
        public Message()
        { }

        public Message(string s, type_msg _tipo = 0)
        {
            m_message = s;
            m_tipo = (type_msg)_tipo;
            var time = DateTime.Now.ToString("[yyyy/MM/dd HH:mm:ss]");
            m_message = time + " " + m_message;
        }

        public void append(string s)
        {
            m_message += s;
        }
        public void set(string s)
        {
            m_message = s;
        }

        public string get() => m_message;
        public type_msg getTipo() => m_tipo;


        private string m_message;
        type_msg m_tipo;

        public void Dispose()
        {
            if (!string.IsNullOrEmpty(m_message))
            {
                m_message = "";
            }
        }
    }

    public static class Message_Pool
    {
        static readonly List<Message> m_message = new List<Message>();
        static DateTime date { get; set; }

        private static void LogOnly()
        {
            var _local = System.IO.Directory.GetCurrentDirectory() + "\\log";
            if (System.IO.Directory.Exists(_local) == false)
            {
                System.IO.Directory.CreateDirectory(_local);

            }
            var _file = System.IO.Directory.GetCurrentDirectory() + "\\log\\log " + date.ToString("ddMMyyyyHHmmss") + ".log";
            var m = getMessage();
            using (System.IO.StreamWriter w = System.IO.File.AppendText(_file))
            {
                w.WriteLine(m.get());
            }
        }

        private static void LogAndConsole()
        {
            LogOnly();
            console_log();
        }

        static void console_log()
        {
            Message m = getMessage();

            if (m != null)
            {

                Console.WriteLine(m.get());
            }
            else
                throw new Exception("Message is null. message_pool::console_log()");
        }
        public static void push(string s, type_msg _tipo = 0)
        {
            if (date == DateTime.MinValue)
            {
                date = DateTime.Now;
            }
                push(new Message(s, _tipo));
        }
        public static void push(string s, Exception exception, type_msg _tipo = 0)
        {
            s += " Exception: " +exception.Message;
            if (date == DateTime.MinValue)
            {
                date = DateTime.Now;
            }
            push(new Message(s, _tipo));
        }
        public static void push(Exception exception, type_msg _tipo = 0)
        {
            var s =  "[CodeException]: " + exception.Message;
            if (date == DateTime.MinValue)
            {
                date = DateTime.Now;
            }
            push(new Message(s, _tipo));
        }

        public static void push(string msg2, Exception ex, string msg, int _code = 0, type_msg _tipo = 0)
        {
            if (date == DateTime.MinValue)
            {
                date = DateTime.Now;
            }
            var s = string.Format("[{0}.{1}] : {2}/Line({3})", msg, msg2, ex.Message, _code);
            push(new Message(s, _tipo));
        }
        public static void push(int msg2, string msg, int _code = 0, type_msg _tipo = 0)
        {
            if (date == DateTime.MinValue)
            {
                date = DateTime.Now;
            }
            var s = string.Format("[{0}] : {1}/Line({2})", msg, msg2, _code);
            push(new Message(s, _tipo));
        }
        public static void push(Message m)
        {
            m_message.Add(m);


            switch (m.getTipo())
            {
                case type_msg.CL_FILE_LOG_AND_CONSOLE:
                    LogAndConsole();
                    break;
                case type_msg.CL_ONLY_CONSOLE:
                    console_log();
                    break;
                case type_msg.CL_FILE_TIME_LOG_AND_CONSOLE:
                    break;
                case type_msg.CL_ONLY_FILE_LOG:
                    LogOnly();
                    break;
                case type_msg.CL_ONLY_FILE_TIME_LOG:
                    break;
                case type_msg.CL_ONLY_FILE_LOG_IO_DATA:
                    break;
                case type_msg.CL_FILE_LOG_IO_DATA_AND_CONSOLE:
                    break;
                case type_msg.CL_ONLY_FILE_LOG_TEST:
                    break;
                case type_msg.CL_FILE_LOG_TEST_AND_CONSOLE:
                    break;
                default:
                    break;
            }
            m_message.Clear();
        }
        static Message getMessage() { return getFirstMessage(); }

        static Message getFirstMessage() { return m_message[0]; }

        public static bool checkUpdateDayLog()
        {

            bool ret = false;

            var ti_day = new DateTime();          
            // Criar novos Logs que trocou o Dia do Log
            if (ti_day.Year < DateTime.Now.Year || ti_day.Month < DateTime.Now.Month || ti_day.Day < DateTime.Now.Day)
            {

                // Criou novos logs, trocou o dia do log
                if (DateTime.Now.Hour == 0 && DateTime.Now.Minute == 0 && DateTime.Now.Second == 0 && date.Millisecond == 0)
                { ret = true; date = DateTime.Now;
                }
            }


            return ret;
        }
    }
}
