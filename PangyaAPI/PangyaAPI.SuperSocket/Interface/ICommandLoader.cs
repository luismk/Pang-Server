﻿using System.Collections.Generic;
using System.IO;
using System.Windows.Input;
using System;
using PangyaAPI.SuperSocket.SocketBase;

namespace PangyaAPI.SuperSocket.Interface
{
    /// <summary>
    /// the empty basic interface for command loader
    /// </summary>
    public interface ICommandLoader
    {

    }

    /// <summary>
    /// Command loader's interface
    /// </summary>
    public interface ICommandLoader<TCommand> : ICommandLoader
        where TCommand : ICommand
    {

        /// <summary>
        /// Initializes the command loader by the root config and the server instance.
        /// </summary>
        /// <param name="rootConfig">The root config.</param>
        /// <param name="appServer">The app server.</param>
        /// <returns></returns>
        bool Initialize(IRootConfig rootConfig, IAppServer appServer);

        /// <summary>
        /// Tries to load commands.
        /// </summary>
        /// <param name="commands">The commands.</param>
        /// <returns></returns>
        bool TryLoadCommands(out IEnumerable<TCommand> commands);

        /// <summary>
        /// Occurs when [updated].
        /// </summary>
        event EventHandler<CommandUpdateEventArgs<TCommand>> Updated;

        /// <summary>
        /// Occurs when [error].
        /// </summary>
        event EventHandler<ErrorEventArgs> Error;
    }
}