using PangyaAPI.IFF.Handle;
using PangyaAPI.SQL.DATA.TYPE;
using PangyaAPI.SuperSocket.Engine;
using PangyaAPI.Utilities;
using System.Collections.Generic;

namespace PangyaAPI.SuperSocket.Interface
{
    /// <summary>
    /// An item can be started and stopped
    /// </summary>
    public interface IWorkItemBase
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the server's config.
        /// </summary>
        /// <value>
        /// The server's config.
        /// </value>
        IServerConfig Config { get; }


        /// <summary>
        /// Starts this server instance.
        /// </summary>
        /// <returns>return true if start successfull, else false</returns>
        bool Start();

        /// <summary>
        /// Stops this server instance.
        /// </summary>
        bool Stop();

        /// <summary>
        /// Restart this server instance.
        /// </summary>
        bool Restart();

        /// <summary>
        /// Gets the total session count.
        /// </summary>
        int SessionCount { get; }
        IFFHandle IFF { get; set; }
        IniHandle Ini { get; set; }
        ServerInfoEx m_si { get; set; }
        List<TableMac> ListBlockMac { get; set; }
    }


    /// <summary>
    /// An item can be started and stopped
    /// </summary>
    public interface IWorkItem : IWorkItemBase
    {
       
        /// <summary>
        /// Gets the current state of the work item.
        /// </summary>
        /// <value>
        /// The state.
        /// </value>
        ServerState State { get; }
    }
}