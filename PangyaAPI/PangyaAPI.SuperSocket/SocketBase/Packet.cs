using PangyaAPI.Cryptor.HandlePacket;
using PangyaAPI.SuperSocket.Cryptor;
using PangyaAPI.Utilities;
using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace PangyaAPI.SuperSocket.SocketBase
{
    public class Packet : AppPacketBase
    {
        public Packet()
        {
        }

        public Packet(ushort ID) : base(ID)
        {
        }

        public Packet(byte[] message, byte key) : base(message, key)
        {
        }
        public void AddFixedString(string value, int len)
        {
            WriteStr(value, len);
        }
        public void Version_Decrypt(uint @packet_version)
        {
            Pang.Packet_Ver_Decrypt(ref @packet_version);
        }
    }
}
