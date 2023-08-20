namespace PangyaAPI.SuperSocket.Interface
{
    /// <summary>
    /// Filter state enum
    /// </summary>
    public enum FilterState : byte
    {
        /// <summary>
        /// Normal state
        /// </summary>
        Normal = 0,
        /// <summary>
        /// Error state
        /// </summary>
        Error = 1,
        /// <summary>
        /// Error state packet
        /// </summary>
        Packet_Size_Error = 2
    }
}