using PangyaAPI.Cryptor.HandlePacket;

namespace PangyaAPI.SuperSocket.SocketBase
{
    public class WSABuf
    {
        public uint Length { get; set; }
        public byte[] Buffer { get; set; }
        public byte[] Buffin()
        {
            var new_data = new byte[Length];
            System.Buffer.BlockCopy(Buffer, 0, new_data, 0, (int)Length);
            return new_data;
        }

        public byte[] Encrypt_Buff(byte _key)
        {
           return Buffin().ServerEncrypt(_key);
        }
    }
}