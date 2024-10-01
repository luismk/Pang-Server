﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Text;
using System.Threading;
using System.Web.SessionState;
using PangyaAPI.Cryptor.HandlePacket;
using PangyaAPI.SuperSocket.Cryptor;
using PangyaAPI.SuperSocket.Interface;
using PangyaAPI.Utilities;
using PangyaAPI.Utilities.BinaryModels;
using _smp = PangyaAPI.Utilities.Log;
namespace PangyaAPI.SuperSocket.SocketBase
{
    /// <summary>
    /// AppSession base class
    /// </summary>
    /// <typeparam name="TAppSession">The type of the app session.</typeparam>
    /// <typeparam name="TRequestInfo">The type of the request info.</typeparam>
    public abstract partial class AppSession<TAppSession, TRequestInfo> : IAppSession, IAppSession<TAppSession, TRequestInfo>
        where TAppSession : AppSession<TAppSession, TRequestInfo>, IAppSession, new()
        where TRequestInfo : class, IRequestInfo
    {
        #region Properties
        public bool Disposed { get; set; }

        /// <summary>
        /// Gets the app server instance assosiated with the session.
        /// </summary>
        public virtual AppServerBase<TAppSession, TRequestInfo> AppServer { get; private set; }

        /// <summary>
        /// Gets the app server instance assosiated with the session.
        /// </summary>
        IAppServer IAppSession.AppServer
        {
            get { return this.AppServer; }
        }

        /// <summary>
        /// Gets or sets the charset which is used for transfering text message.
        /// </summary>
        /// <value>
        /// The charset.
        /// </value>
        public Encoding Charset { get; set; }
       
        private bool m_Connected = false;

        /// <summary>
        /// Gets a value indicating whether this <see cref="IAppSession"/> is connected.
        /// </summary>
        /// <value>
        ///   <c>true</c> if connected; otherwise, <c>false</c>.
        /// </value>
        public bool Connected
        {
            get { return m_Connected; }
            internal set { m_Connected = value; }
        }
        byte Key { get; set; }

        public byte m_key
        {
            get { return Key; }
        }
        public uint m_oid { get; set; }
        public string GetAdress
        {
            get
            {
                if (Connected)
                {
                    return RemoteEndPoint.Address.ToString() + ":" + RemoteEndPoint.Port.ToString();
                }
                else
                {
                    return "0.0.0.0:00";
                }
            }
        }
        public Socket m_sock => this.SocketSession.m_Socket;

        /// <summary>
        /// Gets the local listening endpoint.
        /// </summary>
        public IPEndPoint LocalEndPoint
        {
            get { return SocketSession.LocalEndPoint; }
        }

        /// <summary>
        /// Gets the remote endpoint of client.
        /// </summary>
        public IPEndPoint RemoteEndPoint
        {
            get { return SocketSession.RemoteEndPoint; }
        }

        /// <summary>
        /// Gets or sets the last active time of the session.
        /// </summary>
        /// <value>
        /// The last active time.
        /// </value>
        public DateTime LastActiveTime { get; set; }

        /// <summary>
        /// Gets the start time of the session.
        /// </summary>
        public DateTime StartTime { get; private set; }

        /// <summary>
        /// Gets the socket session of the AppSession.
        /// </summary>
        public ISocketSession SocketSession { get; private set; }

        /// <summary>
        /// Gets the config of the server.
        /// </summary>
        public IServerConfig Config
        {
            get { return AppServer.Config; }
        }

        IReceiveFilter<TRequestInfo> m_ReceiveFilter;

        public int m_time_start;
        public int m_tick;
        public int m_tick_bot;
        // session autorizada pelo server, fez o login corretamente
        public int m_is_authorized;
        // Marca na session que o socket, levou DC, chegou ao limit de retramission do TCP para transmitir os dados
        // TCP sockets is that the maximum retransmission count and timeout have been reached on a bad(or broken) link
        public bool m_connection_timeout;
        public bool m_state;
        public bool m_connected_to_send;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="AppSession&lt;TAppSession, TRequestInfo&gt;"/> class.
        /// </summary>
        public AppSession()
        {
            this.StartTime = DateTime.Now;
            this.LastActiveTime = this.StartTime;
        }
        public virtual string GetNickname() { return ""; }
        public virtual string GetID() { return ""; }
        public virtual uint GetUID() { return 0; }

        /// <summary>
        /// Initializes the specified app session by AppServer and SocketSession.
        /// </summary>
        /// <param name="appServer">The app server.</param>
        /// <param name="appSession">The socket session.</param>
        public virtual void Initialize(IAppServer<TAppSession, TRequestInfo> appServer, ISocketSession appSession)
        {
            if (appServer is null)
            {
                throw new ArgumentNullException(nameof(appServer));
            }
            var Server = appServer as AppServerBase<TAppSession, TRequestInfo>;
            
                AppServer = Server;
                Charset = Server.TextEncoding;
                SocketSession = appSession;
                m_oid = appSession.m_oid = Server.NextConnectionID;
                m_ReceiveFilter = Server.ReceiveFilterFactory.CreateFilter(appServer, this, appSession.RemoteEndPoint);
                IReceiveFilterInitializer filterInitializer = m_ReceiveFilter as IReceiveFilterInitializer;
                filterInitializer?.Initialize(Server, this);            
            appSession.Initialize(this);
            OnInit();
        }
        /// <summary>
        /// Starts the session.
        /// </summary>
        void IAppSession.StartSession()
        {
            OnSessionStarted();
        }

        /// <summary>
        /// Called when [init].
        /// </summary>
        protected virtual void OnInit()
        {
            this.m_time_start = this.m_tick = Environment.TickCount;
            Key = (byte)new Random().Next(1, 15);
            m_Connected = true;
            SetTimeStartAndTick(Environment.TickCount);
            this.SetState(true);
            this.SetConnected(true);
        }

        /// <summary>
        /// Called when [session started].
        /// </summary>
        protected virtual void OnSessionStarted()
        {
            ////metodo que faz uma checagem em tempo em tempo[verificar se a conexao ta ativa ainda]
            //// Define um tempo limite em milissegundos (exemplo: 5 segundos)
            //Thread checkConnectionThread = new Thread(() =>
            //{
            //int timeoutMillis = 5000;

            //while (m_sock != null)
            //{
            //        // Verifica se a conexão está ativa após o tempo limite
            //        if (m_sock!= null)                    {

            //            bool isConnected = m_sock.Poll(timeoutMillis * 1000, SelectMode.SelectRead);

            //            if (isConnected && m_sock != null && m_sock.Available == 0)
            //            {
            //                Console.WriteLine("A conexão está ativa.");
            //            }
            //            else
            //            {
            //                Console.WriteLine("A conexão não está mais ativa.");
            //            }
            //        }
            //    }
            //});

            //// Inicia a thread de verificação
            //checkConnectionThread.Start();
        }

        /// <summary>
        /// Called when [session closed].
        /// </summary>
        /// <param name="reason">The reason.</param>
        internal protected virtual void OnSessionClosed(CloseReason reason)
        {
            //metodo que faz uma dispose em tempo em tempo[verificar se a conexao ta ativa ainda]

            Dispose();
        }

        public bool GetState()
        {
            return m_state;
        }

        public void SetState(bool state)
        {
            m_state = state;
        }

        public void SetConnected(bool connected)
        {
            SetConnectedToSend(connected);
        }

        public void SetConnectedToSend(bool connectedToSend)
        {
            m_connected_to_send = connectedToSend;
        }

        /// <summary>
        /// Handles the exceptional error, it only handles application error.
        /// </summary>
        /// <param name="e">The exception.</param>
        protected virtual void HandleException(Exception e)
        {
            _smp.Message_Pool.push("[AppSession::HandleException][LogException]: " + e.Message);
            this.Close(CloseReason.ApplicationError);
        }

        /// <summary>
        /// Handles the unknown request.
        /// </summary>
        /// <param name="requestInfo">The request info.</param>
        protected virtual void HandleUnknownRequest(TRequestInfo requestInfo)
        {

        }

        internal void InternalHandleUnknownRequest(TRequestInfo requestInfo)
        {
            HandleUnknownRequest(requestInfo);
        }

        internal void InternalHandleExcetion(Exception e)
        {
            HandleException(e);
        }

        /// <summary>
        /// Closes the session by the specified reason.
        /// </summary>
        /// <param name="reason">The close reason.</param>
        public virtual void Close(CloseReason reason)
        {
            this.SocketSession.Close(reason);
        }

        /// <summary>
        /// Closes this session.
        /// </summary>
        public virtual void Close()
        {
            Close(CloseReason.ServerClosing);
        }

        #region Sending processing

        /// <summary>
        /// Try to send the data to client.
        /// </summary>
        /// <param name="data">The data which will be sent.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="length">The length.</param>
        /// <returns>Indicate whether the message was pushed into the sending queue</returns>
        public virtual bool TrySend(byte[] data, int offset, int length)
        {
            return InternalTrySend(new ArraySegment<byte>(data, offset, length));
        }

        /// <summary>
        /// Sends the data to client.
        /// </summary>
        /// <param name="data">The data which will be sent.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="length">The length.</param>
        public virtual void Send(byte[] data, int offset, int length, bool compress = false)
        {
            var new_data = new byte[length];
            Buffer.BlockCopy(data, 0, new_data, 0, length);
            if (compress)
            {
                new_data = data.ServerEncrypt(m_key);
            }
            InternalSend(new ArraySegment<byte>(new_data, 0, new_data.Length));
        }

        public virtual void Send(byte[] data, bool compress = false)
        {
            if (compress)
            {
                data = data.ServerEncrypt(m_key);
            }
            InternalSend(new ArraySegment<byte>(data, 0, data.Length));
        }

        public virtual void SendResponse(byte[] data)
        {
            InternalSend(new ArraySegment<byte>(data, 0, data.Length));
        }

        public virtual void Send(ref Packet packet, bool compress = false)
        {
            var new_data = packet.GetBytes();
            _smp.Message_Pool.push("[AppSession::Send][log]: " + new_data.HexDump(), _smp.type_msg.CL_ONLY_CONSOLE);
            Console.WriteLine(Environment.NewLine);
            new_data = new_data.ServerEncrypt(m_key);  
            InternalSend(new ArraySegment<byte>(new_data, 0, new_data.Length));
            
        }
        public virtual void Send(PangyaBinaryWriter packet)
        {
            InternalSend(new ArraySegment<byte>(packet.GetBytes, 0, packet.GetBytes.Length));
        }
        private bool InternalTrySend(ArraySegment<byte> segment)
        {
            if (!SocketSession.TrySend(segment))
                return false;

            LastActiveTime = DateTime.Now;
            return true;
        }

        /// <summary>
        /// Try to send the data segment to client.
        /// </summary>
        /// <param name="segment">The segment which will be sent.</param>
        /// <returns>Indicate whether the message was pushed into the sending queue</returns>
        public virtual bool TrySend(ArraySegment<byte> segment)
        {
            if (!m_Connected)
                return false;

            return InternalTrySend(segment);
        }


        private void InternalSend(ArraySegment<byte> segment)
        {
            if (!m_Connected)
                return;

            if (InternalTrySend(segment))
                return;

            var sendTimeOut = Config.SendTimeOut;

            //Don't retry, timeout directly
            if (sendTimeOut < 0)
            {
                throw new TimeoutException("The sending attempt timed out");
            }

            var timeOutTime = sendTimeOut > 0 ? DateTime.Now.AddMilliseconds(sendTimeOut) : DateTime.Now;

            var spinWait = new SpinWait();

            while (m_Connected)
            {
                spinWait.SpinOnce();

                if (InternalTrySend(segment))
                    return;

                //If sendTimeOut = 0, don't have timeout check
                if (sendTimeOut > 0 && DateTime.Now >= timeOutTime)
                {
                    throw new TimeoutException("The sending attempt timed out");
                }
            }
        }

        /// <summary>
        /// Sends the data segment to client.
        /// </summary>
        /// <param name="segment">The segment which will be sent.</param>
        public virtual void Send(ArraySegment<byte> segment)
        {
            InternalSend(segment);
        }

        private bool InternalTrySend(IList<ArraySegment<byte>> segments)
        {
            if (!SocketSession.TrySend(segments))
                return false;

            LastActiveTime = DateTime.Now;
            return true;
        }

        /// <summary>
        /// Try to send the data segments to client.
        /// </summary>
        /// <param name="segments">The segments.</param>
        /// <returns>Indicate whether the message was pushed into the sending queue; if it returns false, the sending queue may be full or the socket is not connected</returns>
        public virtual bool TrySend(IList<ArraySegment<byte>> segments)
        {
            if (!m_Connected)
                return false;

            return InternalTrySend(segments);
        }

        private void InternalSend(IList<ArraySegment<byte>> segments)
        {
            if (!m_Connected)
                return;

            if (InternalTrySend(segments))
                return;

            var sendTimeOut = Config.SendTimeOut;

            //Don't retry, timeout directly
            if (sendTimeOut < 0)
            {
                throw new TimeoutException("The sending attempt timed out");
            }

            var timeOutTime = sendTimeOut > 0 ? DateTime.Now.AddMilliseconds(sendTimeOut) : DateTime.Now;

            var spinWait = new SpinWait();

            while (m_Connected)
            {
                spinWait.SpinOnce();

                if (InternalTrySend(segments))
                    return;

                //If sendTimeOut = 0, don't have timeout check
                if (sendTimeOut > 0 && DateTime.Now >= timeOutTime)
                {
                    throw new TimeoutException("The sending attempt timed out");
                }
            }
        }

        /// <summary>
        /// Sends the data segments to client.
        /// </summary>
        /// <param name="segments">The segments.</param>
        public virtual void Send(IList<ArraySegment<byte>> segments)
        {
            InternalSend(segments);
        }

        /// <summary>
        /// Sends the response.
        /// </summary>
        /// <param name="message">The message which will be sent.</param>
        /// <param name="paramValues">The parameter values.</param>
        public virtual void Send(string message, params object[] paramValues)
        {
            var data = this.Charset.GetBytes(string.Format(message, paramValues));
            InternalSend(new ArraySegment<byte>(data, 0, data.Length));
        }

        #endregion

        #region Receiving processing

        /// <summary>
        /// Gets the maximum allowed length of the request.
        /// </summary>
        /// <returns></returns>
        protected virtual int GetMaxRequestLength()
        {
            return AppServer.Config.MaxRequestLength;
        }

        /// <summary>
        /// Filters the request.
        /// </summary>
        /// <param name="readBuffer">The read buffer.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="length">The length.</param>
        /// <param name="toBeCopied">if set to <c>true</c> [to be copied].</param>
        /// <param name="rest">The rest, the size of the data which has not been processed</param>
        /// <param name="offsetDelta">return offset delta of next receiving buffer.</param>
        /// <returns></returns>
        TRequestInfo FilterRequest(byte[] readBuffer, int offset, int length, bool toBeCopied, out int rest, out int offsetDelta)
        {
            try
            {
                var ReceiveFilter = m_ReceiveFilter as PangyaReceiveFilter;
                if (!AppServer.OnRawDataReceived(this, readBuffer, offset, length))
                {
                    rest = 0;
                    offsetDelta = 0;
                    return null;
                }

                var currentRequestLength = ReceiveFilter.LeftBufferSize;

                var requestInfo = ReceiveFilter.Filter(m_key, readBuffer, offset, length);
                rest = 0;

                if (ReceiveFilter.State == FilterState.Error)
                {
                    rest = 0;
                    offsetDelta = 0;
                    Close(CloseReason.ProtocolError);
                    return null;
                }

                var offsetAdapter = ReceiveFilter as IOffsetAdapter;

                offsetDelta = offsetAdapter != null ? offsetAdapter.OffsetDelta : 0;

                if (requestInfo == null)
                {
                    //current buffered length
                    currentRequestLength = ReceiveFilter.LeftBufferSize;
                }
                else
                {
                    //current request length
                    currentRequestLength = currentRequestLength + length - rest;
                }

                var maxRequestLength = GetMaxRequestLength();

                if (currentRequestLength >= maxRequestLength)
                {
                    //if (Logger.IsErrorEnabled)
                    _smp.Message_Pool.push(string.Format("[AppSession::FilterRequest()][Error]: Max request length: {0}, current processed length: {1}", maxRequestLength, currentRequestLength));

                    Close(CloseReason.ProtocolError);
                    return null;
                }

                return requestInfo as TRequestInfo;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Processes the request data.
        /// </summary>
        /// <param name="readBuffer">The read buffer.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="length">The length.</param>
        /// <param name="toBeCopied">if set to <c>true</c> [to be copied].</param>
        /// <returns>
        /// return offset delta of next receiving buffer
        /// </returns>
        int IAppSession.ProcessRequest(byte[] readBuffer, int offset, int length, bool toBeCopied)
        {
            while (Connected)
            {
                var requestInfo = FilterRequest(readBuffer, offset, length, toBeCopied, out int rest, out int offsetDelta);

                if (requestInfo != null)
                {
                    try
                    {
                        AppServer.ExecuteRequest(this as TAppSession, requestInfo);
                    }
                    catch (Exception e)
                    {
                        HandleException(e);
                    }
                }

                if (rest <= 0)
                {
                    return offsetDelta;
                }

                //Still have data has not been processed
                offset = offset + length - rest;
                length = rest;
            }
            throw new exception("[APPSESSION][Error]: Error, não esta mais conectado ou esta enviando dados invalidos!", STDA_ERROR_TYPE.SESSION);
        }


        public void Dispose()
        {
            // Suppress finalization.
            GC.SuppressFinalize(this);
        }

        public void HandleExceptionalError(Exception e)
        {
            //  //throw new NotImplementedException();
        }
        public bool Clear()
        {
            return true;
        }

        internal void SetOID(uint index)
        {
            m_oid = index;
        }

        internal void SetTimeStartAndTick(int tickCount)
        {
            m_tick = tickCount;
        }
        #endregion
    }

    /// <summary>
    /// AppServer basic class for whose request infoe type is StringRequestInfo
    /// </summary>
    /// <typeparam name="TAppSession">The type of the app session.</typeparam>
    public abstract class AppSession<TAppSession> : AppSession<TAppSession, StringRequestInfo>
        where TAppSession : AppSession<TAppSession, StringRequestInfo>, IAppSession, new()
    {

        private bool m_AppendNewLineForResponse = false;

        private static string m_NewLine = "\r\n";

        /// <summary>
        /// Initializes a new instance of the <see cref="AppSession&lt;TAppSession&gt;"/> class.
        /// </summary>
        public AppSession()
           
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AppSession&lt;TAppSession&gt;"/> class.
        /// </summary>
        /// <param name="appendNewLineForResponse">if set to <c>true</c> [append new line for response].</param>
        public AppSession(bool appendNewLineForResponse)
        {
            m_AppendNewLineForResponse = appendNewLineForResponse;
        }

        /// <summary>
        /// Handles the unknown request.
        /// </summary>
        /// <param name="requestInfo">The request info.</param>
        protected override void HandleUnknownRequest(StringRequestInfo requestInfo)
        {
            Send(new byte[0]);
        }

        /// <summary>
        /// Processes the sending message.
        /// </summary>
        /// <param name="rawMessage">The raw message.</param>
        /// <returns></returns>
        protected virtual string ProcessSendingMessage(string rawMessage)
        {
            if (!m_AppendNewLineForResponse)
                return rawMessage;

            if (AppServer.Config.Mode == SocketMode.Udp)
                return rawMessage;

            if (string.IsNullOrEmpty(rawMessage) || !rawMessage.EndsWith(m_NewLine))
                return rawMessage + m_NewLine;
            else
                return rawMessage;
        }

        /// <summary>
        /// Sends the response.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="paramValues">The param values.</param>
        /// <returns>Indicate whether the message was pushed into the sending queue</returns>
        public override void Send(string message, params object[] paramValues)
        {
            base.Send(ProcessSendingMessage(message), paramValues);
        }

    }

    /// <summary>
    /// AppServer basic class for whose request infoe type is StringRequestInfo
    /// </summary>
    public class AppSession : AppSession<AppSession>
    {

    }
}
