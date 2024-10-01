using PangyaAPI.SuperSocket.Interface;

namespace PangyaAPI.SuperSocket.SocketBase
{
    public class PangyaRequestInfo : IRequestInfo
    {
        public short PacketID => (short)_packet ?.Id;
        /// <summary>
        /// Get Read and Write Packet 
        /// </summary>
        public Packet _packet { get; set; }

        public PangyaRequestInfo()
        {
         }


        public PangyaRequestInfo Initialize(byte key, byte[] buff, int size)
        {
            try
            {
                _packet = new Packet(buff, key);                  
                return this;
            }
            catch
            {
                return null;
            }
        }
    }
}
