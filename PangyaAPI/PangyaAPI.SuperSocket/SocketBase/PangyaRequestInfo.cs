using PangyaAPI.SuperSocket.Interface;

namespace PangyaAPI.SuperSocket.SocketBase
{
    public class PangyaRequestInfo : IRequestInfo
    {
        public short PacketID => (short)_packet?.m_Tipo;
        /// <summary>
        /// Get Read and Write Packet 
        /// </summary>
        public Packet _packet { get; set; }

        public PangyaRequestInfo()
        {
            _packet = new Packet();
        }


        public PangyaRequestInfo Initialize(byte key, byte[] buff, int size)
        {
            try
            {
                _packet = new Packet();
                _packet.AddMaked(buff, size);//adiciona o pacote recebido
                _packet.UnMake(key);//decripta o pacote
                _packet.m_Tipo = _packet.ReadUInt16();//ler o pacote
                return this;
            }
            catch
            {
                return null;
            }
        }
    }
}
