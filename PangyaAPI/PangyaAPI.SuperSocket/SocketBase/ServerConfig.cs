﻿using PangyaAPI.SuperSocket.Interface;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System;
using PangyaAPI.SuperSocket.Ext;

namespace PangyaAPI.SuperSocket.SocketBase
{
    /// <summary>
    /// Server configruation model
    /// </summary>
    [Serializable]
    public partial class ServerConfig : IServerConfig
    {
        /// <summary>
        /// Default ReceiveBufferSize
        /// </summary>
        public const int DefaultReceiveBufferSize = 4096;

        /// <summary>
        /// Default MaxConnectionNumber
        /// </summary>
        public const int DefaultMaxConnectionNumber = 100;


        /// <summary>
        /// Default sending queue size
        /// </summary>
        public const int DefaultSendingQueueSize = 5;

        /// <summary>
        /// Default MaxRequestLength
        /// </summary>
        public const int DefaultMaxRequestLength = 1024;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerConfig"/> class.
        /// </summary>
        /// <param name="serverConfig">The server config.</param>
        public ServerConfig(IServerConfig serverConfig)
        {
            serverConfig.CopyPropertiesTo(this);

            this.Options = serverConfig.Options;
            this.OptionElements = serverConfig.OptionElements;


            if (serverConfig.Listeners != null && serverConfig.Listeners.Any())
            {
                List<ListenerConfig> transformedListeners = new List<ListenerConfig>();

                foreach (var listener in serverConfig.Listeners)
                {
                    ListenerConfig newListener = new ListenerConfig();
                    listener.CopyPropertiesTo(newListener); // Chame o método CopyPropertiesTo para copiar as propriedades
                    transformedListeners.Add(newListener);
                }

                this.Listeners = transformedListeners;
            }



        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerConfig"/> class.
        /// </summary>
        public ServerConfig()
        {
            MaxConnectionNumber = DefaultMaxConnectionNumber;
            Mode = SocketMode.Tcp;
            MaxRequestLength = DefaultMaxRequestLength;
            KeepAliveTime = 10 * 60;// 10 minutes
            KeepAliveInterval = 60;// 60 seconds
            ListenBacklog = 100;
            ReceiveBufferSize = DefaultReceiveBufferSize;
            SendingQueueSize = DefaultSendingQueueSize;
            ClearIdleSession = true;
            IdleSessionTimeOut = 300;//seconds
            ClearIdleSessionInterval = 120;//seconds
            SessionSnapshotInterval = 5;//seconds
        }

        #region IServerConfig Members

        /// <summary>
        /// Gets/sets the name of the server type of this appServer want to use.
        /// </summary>
        /// <value>
        /// The name of the server type.
        /// </value>
        public string ServerTypeName { get; set; }


        /// <summary>
        /// Gets/sets the type definition of the appserver.
        /// </summary>
        /// <value>
        /// The type of the server.
        /// </value>
        public string ServerType { get; set; }

        /// <summary>
        /// Gets/sets the Receive filter factory.
        /// </summary>
        public string ReceiveFilterFactory { get; set; }

        /// <summary>
        /// Gets/sets the ip.
        /// </summary>
        public string Ip { get; set; }

        /// <summary>
        /// Gets/sets the port.
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Gets/sets the options.
        /// </summary>
        public NameValueCollection Options { get; set; }

        /// <summary>
        /// Gets the option elements.
        /// </summary>
        public NameValueCollection OptionElements { get; set; }

        /// <summary>
        /// Gets/sets a value indicating whether this <see cref="IServerConfig"/> is disabled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if disabled; otherwise, <c>false</c>.
        /// </value>
        public bool Disabled { get; set; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets/sets the mode.
        /// </summary>
        public SocketMode Mode { get; set; }

        /// <summary>
        /// Gets/sets the send time out.
        /// </summary>
        public int SendTimeOut { get; set; }

        /// <summary>
        /// Gets the max connection number.
        /// </summary>
        public int MaxConnectionNumber { get; set; }

        /// <summary>
        /// Gets the size of the receive buffer.
        /// </summary>
        /// <value>
        /// The size of the receive buffer.
        /// </value>
        public int ReceiveBufferSize { get; set; }

        /// <summary>
        /// Gets the size of the send buffer.
        /// </summary>
        /// <value>
        /// The size of the send buffer.
        /// </value>
        public int SendBufferSize { get; set; }


        /// <summary>
        /// Gets a value indicating whether sending is in synchronous mode.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [sync send]; otherwise, <c>false</c>.
        /// </value>
        public bool SyncSend { get; set; }

        /// <summary>
        /// Gets/sets a value indicating whether clear idle session.
        /// </summary>
        /// <value>
        ///   <c>true</c> if clear idle session; otherwise, <c>false</c>.
        /// </value>
        public bool ClearIdleSession { get; set; }

        /// <summary>
        /// Gets/sets the clear idle session interval, in seconds.
        /// </summary>
        /// <value>
        /// The clear idle session interval.
        /// </value>
        public int ClearIdleSessionInterval { get; set; }

        /// <summary>
        /// Gets/sets the idle session timeout time length, in seconds.
        /// </summary>
        /// <value>
        /// The idle session time out.
        /// </value>
        public int IdleSessionTimeOut { get; set; }

        /// <summary>
        /// Gets/sets the length of the max request.
        /// </summary>
        /// <value>
        /// The length of the max request.
        /// </value>
        public int MaxRequestLength { get; set; }

        /// <summary>
        /// Gets/sets a value indicating whether [disable session snapshot].
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [disable session snapshot]; otherwise, <c>false</c>.
        /// </value>
        public bool DisableSessionSnapshot { get; set; }

        /// <summary>
        /// Gets/sets the interval to taking snapshot for all live sessions.
        /// </summary>
        public int SessionSnapshotInterval { get; set; }

        /// <summary>
        /// Gets/sets the connection filters used by this server instance.
        /// </summary>
        /// <value>
        /// The connection filter's name list, seperated by comma
        /// </value>
        public string ConnectionFilter { get; set; }

        /// <summary>
        /// Gets/sets the start keep alive time, in seconds
        /// </summary>
        public int KeepAliveTime { get; set; }

        /// <summary>
        /// Gets/sets the keep alive interval, in seconds.
        /// </summary>
        public int KeepAliveInterval { get; set; }

        /// <summary>
        /// Gets the backlog size of socket listening.
        /// </summary>
        public int ListenBacklog { get; set; }

        /// <summary>
        /// Gets/sets the startup order of the server instance.
        /// </summary>
        public int StartupOrder { get; set; }

        /// <summary>
        /// Gets the child config.
        /// </summary>
        /// <typeparam name="TConfig">The type of the config.</typeparam>
        /// <param name="childConfigName">Name of the child config.</param>
        /// <returns></returns>
        public virtual TConfig GetChildConfig<TConfig>(string childConfigName)
            where TConfig : ConfigurationElement, new()
        {
            return this.OptionElements.GetChildConfig<TConfig>(childConfigName);
        }

        /// <summary>
        /// Gets and sets the listeners' configuration.
        /// </summary>
        public IEnumerable<IListenerConfig> Listeners { get; set; }
         
        /// <summary>
        /// Gets/sets the size of the sending queue.
        /// </summary>
        /// <value>
        /// The size of the sending queue.
        /// </value>
        public int SendingQueueSize { get; set; }

        #endregion
    }
}