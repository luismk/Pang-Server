namespace PangyaAPI.SuperSocket.Engine
{
    /// <summary>
    /// Server's state enum class
    /// </summary>
    public enum ServerState : int
    {
        /// <summary>
        /// Not initialized
        /// </summary>
        NotInitialized = ServerStateConst.NotInitialized,

        /// <summary>
        /// In initializing
        /// </summary>
        Initializing = ServerStateConst.Initializing,

        /// <summary>
        /// Has been initialized, but not started
        /// </summary>
        NotStarted = ServerStateConst.NotStarted,

        /// <summary>
        /// In starting
        /// </summary>
        Starting = ServerStateConst.Starting,

        /// <summary>
        /// In running
        /// </summary>
        Running = ServerStateConst.Running,

        /// <summary>
        /// In stopping
        /// </summary>
        Stopping = ServerStateConst.Stopping,

        /// <summary>
        /// In Restarting
        /// </summary>
        Restarting = ServerStateConst.Restarting,
    }

    internal class ServerStateConst
    {
        public const int NotInitialized = 0;

        public const int Initializing = 1;

        public const int NotStarted = 2;

        public const int Starting = 3;

        public const int Running = 4;

        public const int Stopping = 5;

        public const int Restarting = 6;
    }
}
