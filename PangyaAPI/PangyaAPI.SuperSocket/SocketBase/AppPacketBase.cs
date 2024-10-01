using PangyaAPI.SuperSocket.Cryptor;
using PangyaAPI.SuperSocket.Ext;
using PangyaAPI.Utilities;
using PangyaAPI.Utilities.BinaryModels;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using static PangyaAPI.SuperSocket.Ext.Tools;
namespace PangyaAPI.SuperSocket.SocketBase
{

    public class OffsetIndex
    {
        public PangyaBinaryReader Read;
        public PangyaBinaryWriter Writer;
        public int IndexR;
        public int IndexW;
        public int Size;
        public int SizeAllocated;

        public void Clear()
        {
            Writer = new PangyaBinaryWriter();
            IndexR = 0;
            IndexW = 0;
            Size = 0;
        }

        public void ResetRead()
        {
            IndexR = 0;
        }

        public void ResetWrite()
        {
            IndexR = 0;
            IndexW = 0;
        }

        public void Reset()
        {
            ResetRead();
            ResetWrite();
        }
    }
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class PacketHead
    {
        public byte LowKey { get; set; }
        public ushort Size { get; set; }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class PacketHeadClient : PacketHead
    {
        public byte Seq;
    }

    public class AppPacketBase : IDisposable
    {

        #region Private Fields
        private readonly MemoryStream _stream;
        /// <summary>
        /// Leitor do packet
        /// </summary>
        private PangyaBinaryReader Reader;

        private PangyaBinaryWriter Reply = new PangyaBinaryWriter();

        /// <summary>
        /// Mensagem do Packet
        /// </summary>
        public byte[] Message { get; set; }

        private byte[] MessageCrypted { get; set; }
        #endregion

        #region Public Fields
        /// <summary>
        /// Id do Packet
        /// </summary>
        public short Id { get; set; }
        #endregion

        #region Constructor
        public AppPacketBase()
        {                           
        }
        public AppPacketBase(ushort ID)
        {
            Reply.WriteUInt16(ID);
        }

        public AppPacketBase(byte[] message, byte key)
        {
            Id = BitConverter.ToInt16(new byte[] { message[5], message[6] }, 0);

            MessageCrypted = new byte[message.Length];
            Buffer.BlockCopy(message, 0, MessageCrypted, 0, message.Length); //Copia mensagem recebida criptografada

            Message = PangyaAPI.Cryptor.HandlePacket.Pang.ClientDecrypt(message, key);

            _stream = new MemoryStream(Message);

            _stream.Seek(2, SeekOrigin.Current); //Seek Inicial
            Reader = new PangyaBinaryReader(_stream);
        }


        #region Methods Get
        public uint GetSize
        {
            get => Reader.Size;
        }
        public uint GetPos
        {
            get => Reader.GetPosition();
        }

        public double ReadDouble()
        {
            return Reader.ReadDouble();
        }

        public byte ReadUInt8()
        {
            return Reader.ReadByte();
        }

        public short ReadInt16()
        {
            return Reader.ReadInt16();
        }
        public ushort ReadUInt16()
        {
            return Reader.ReadUInt16();
        }



        public uint ReadUInt32()
        {
            return Reader.ReadUInt32();
        }
        public int ReadInt32()
        {
            return Reader.ReadInt32();
        }

        public ulong ReadUInt64()
        {
            return Reader.ReadUInt64();
        }

        public long ReadInt64()
        {
            return Reader.ReadInt64();
        }

        public float ReadSingle()
        {
            return Reader.ReadSingle();
        }

        public string ReadString()
        {
            return Reader.ReadPStr();
        }
        public void Skip(int count)
        {
            Reader.Skip(count);
        }


        public void Seek(int offset, int origin)
        {
            Reader.Seek(offset, origin);
        }

        public T Read<T>() where T : struct
        {
            return Reader.Read<T>();
        }
        public IEnumerable<uint> Read(uint count)
        {
            return Reader.Read(count);
        }
        public object Read(object value, int Count)
        {
            return Reader.Read(value, Count);
        }

        public object Read(object value)
        {
            return Reader.Read(value);
        }



        public string ReadPStr(uint Count)
        {
            var data = new byte[Count];
            //ler os dados
            Reader.BaseStream.Read(data, 0, (int)Count);
            var value = Encoding.ASCII.GetString(data);
            return value;
        }

        public bool ReadPStr(out string value, uint Count)
        {
            return Reader.ReadPStr(out value, Count);
        }
        public bool ReadPStr(out string value)
        {
            return Reader.ReadPStr(out value);
        }
        public string ReadPStr()
        {
            return Reader.ReadPStr();
        }
        public bool ReadDouble(out Double value)
        {
            return Reader.ReadDouble(out value);
        }
        public bool ReadBytes(out byte[] value)
        {
            return Reader.ReadBytes(out value);
        }

        public bool ReadBytes(out byte[] value, int len)
        {
            return Reader.ReadBytes(out value, len);
        } 
        public bool ReadByte(out byte value)
        {
            return Reader.ReadByte(out value);
        }
        public byte ReadByte()
        {
            return Reader.ReadByte();
        }
        public bool ReadInt16(out short value)
        {
            return Reader.ReadInt16(out value);
        }
        public bool ReadUInt16(out ushort value)
        {
            return Reader.ReadUInt16(out value);
        }

        public bool ReadUInt32(out uint value)
        {
            return Reader.ReadUInt32(out value);
        }

        public bool ReadInt32(out int value)
        {
            return Reader.ReadInt32(out value);
        }

        public bool ReadUInt64(out ulong value)
        {
            return Reader.ReadUInt64(out value);
        }

        public bool ReadInt64(out long value)
        {
            return Reader.ReadInt64(out value);
        }

        public bool ReadSingle(out float value)
        {
            return Reader.ReadSingle(out value);
        }


        public byte[] GetRemainingData
        {
            get => Reader.GetRemainingData();
        }
        public byte[] ReadBytes(int count)
        {
            return Reader.ReadBytes(count);
        }

        void AddPlain(byte[] strBytes, int Length)
        {
            Write(strBytes, Length);
        }


        public void AddString(string str)
        {
            byte[] strBytes = Encoding.UTF8.GetBytes(str);
            AddUInt16((ushort)strBytes.Length);
            AddPlain(strBytes, strBytes.Length);
        }                                

        public void AddBuffer(object value, int size)
        {
            try
            {
                byte[] arr = new byte[size];

                IntPtr ptr = Marshal.AllocHGlobal(size);
                Marshal.StructureToPtr(value, ptr, true);
                Marshal.Copy(ptr, arr, 0, size);
                Marshal.FreeHGlobal(ptr);
                AddPlain(arr, size);
                ptr = IntPtr.Zero;
            }
            catch (Exception ex)
            {
            }
        }
        public void AddBuffer(object value, object value_or)
        {
            try
            {
                int size = Marshal.SizeOf(value_or);
                byte[] arr = new byte[size];

                IntPtr ptr = Marshal.AllocHGlobal(size);
                Marshal.StructureToPtr(value, ptr, false);
                Marshal.Copy(ptr, arr, 0, size);
                Marshal.FreeHGlobal(ptr);
                AddPlain(arr, size);
            }
            catch (Exception ex)
            {
            }
        }
        public void AddBuffer(object value)
        {
            try
            {
                var size = Marshal.SizeOf(value);
                byte[] arr = new byte[size];
                IntPtr ptr = Marshal.AllocHGlobal(size);
                Marshal.StructureToPtr(value, ptr, false);
                Marshal.Copy(ptr, arr, 0, size);
                Marshal.FreeHGlobal(ptr);
                AddPlain(arr, size);
            }
            catch (Exception ex)
            {

            }
        }
        public void AddBuffer(byte[] buffer)
        {
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer), "Buffer cannot be null.");

            AddUInt32((uint)buffer.Length);
            AddPlain(buffer, buffer.Length);
        }

        public void ReadBuffer(byte[] @buffer, int len)
        {
            
            ReadBytes(out byte[] _buffer, len);

        }

        public void AddZeroByte(int size)
        {
            if (size <= 0)
                throw new ArgumentOutOfRangeException(nameof(size), "Size must be greater than zero.");

            byte[] zeros = new byte[size];
            AddPlain(zeros, size);
        }

        public void AddQWord(ulong qword)
        {
            byte[] bytes = BitConverter.GetBytes(qword);
            AddPlain(bytes, sizeof(ulong));
        }

        public void ReadQWord(out ulong qword)
        {
            qword = ReadUInt64(); 
        }

        public void AddDWord(uint dword)
        {
            byte[] bytes = BitConverter.GetBytes(dword);
            AddPlain(bytes, sizeof(uint));
        }

        public void ReadDWord(out uint dword)
        {                                      
            dword = ReadUInt32();
        }

        public void AddWord(ushort word)
        {
            byte[] bytes = BitConverter.GetBytes(word);
            AddPlain(bytes, sizeof(ushort));
        }

        public void ReadWord(out ushort word)
        {                                     
            word = ReadUInt16();
        }

        public void AddByte(byte b)
        {
            AddPlain(b.ConvertArray(), 1);
        }     

        public void AddInt64(long value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            AddPlain(bytes, sizeof(long));
        }
                 
        public void AddInt32(int value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            AddPlain(bytes, sizeof(int));
        }              

        public void AddFloat(float value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            AddPlain(bytes, sizeof(float));
        }

        public void ReadFloat(out float value)
        {                                            
            value = ReadSingle();      
        }

        public void AddDouble(double value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            AddPlain(bytes, sizeof(double));
        }
                         
        public void ReadString(out string str)
        {                    
            str = ReadPStr();
        }

        public void AddWString(string str)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(str);
            AddInt16((short)bytes.Length);
            AddPlain(bytes, bytes.Length);
        }
           
        public void AddUInt16(ushort value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            AddPlain(bytes, sizeof(ushort));
        }
        public void AddUInt32(uint value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            AddPlain(bytes, sizeof(uint));
        }

        public void AddInt16(short value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            AddPlain(bytes, sizeof(short));
        }                         

        public void AddUInt8(byte value)
        {
            AddPlain(new byte[] { value }, sizeof(byte));
        }

        public void ReadUInt8(out byte value) {
            value = ReadByte();
        }

        public void SetReader(PangyaBinaryReader read)
        {
            Reader = read;
        }

        #endregion

        #region Methods Writer

        public void Write(byte[] data)
        {
            try
            {
                Reply.Write(data);
            }
            catch
            {
            }
            return;
        }

        public void Write(byte[] data, int len)
        {
            try
            {
                Reply.Write(data, len);
            }
            catch
            {
            }
            return;
        }

        public void WriteStruct(object data)
        {
            try
            {
                Reply.WriteStruct(data);
            }
            catch
            {
            }
            return;
        }


        public void WriteStr(string message, int length)
        {

            try
            {
                if (message == null)
                {
                    message = string.Empty;
                }

                message = message.PadRight(length, (char)0x00);
                Reply.Write(message.Select(Convert.ToByte).ToArray());
            }
            catch
            {
            }
            return;
        }

        public bool WriteStr(string message)
        {
            try
            {
                WriteStr(message, message.Length);
            }
            catch
            {
                return false;
            }
            return true;

        }

        public void WritePStr(string value)
        {

            try
            {
                Reply.WritePStr(value);

            }
            catch
            {
                return;
            }
        }

        public void WriteZero(int count)
        {
            try
            {
                Reply.WriteZero(count);
            }
            catch
            {

            }

        }
        public void WriteUInt16(ushort value)
        {
            try
            {
                Reply.Write(value);
            }
            catch
            {

            }

        }

        public void WriteInt16(short value)
        {
            try
            {
                Reply.Write(value);
            }
            catch
            {

            }

        }
        public void WriteByte(byte value)
        {
            try
            {
                Reply.Write(value);
            }
            catch
            {

            }

        }

        public void WriteSingle(float value)
        {
            try
            {
                Reply.Write(value);
            }
            catch
            {

            }

        }

        public void WriteUInt32(uint value)
        {
            try
            {
                Reply.Write(value);
            }
            catch
            {

            }

        }

        public void WriteInt32(int value)
        {
            try
            {
                Reply.Write(value);
            }
            catch
            {

            }

        }

        public void WriteUInt64(ulong value)
        {
            try
            {
                Reply.Write(value);
            }
            catch
            {

            }

        }

        public void WriteInt64(long value)
        {
            try
            {
                Reply.Write(value);
            }
            catch
            {

            }

        }

        public void WriteDouble(double value)
        {
            try
            {
                Reply.Write(value);
            }
            catch
            {

            }

        }


        public void init_plain(ushort value)
        {
            Clear();
            WriteUInt16(value);
        }

        public byte[] GetBytes()
        {
            return Reply.GetBytes;
        }

        public void Clear()
        {
            Reply = new PangyaBinaryWriter();
        }

        #region IDisposable Support
        private bool disposedValue = false; // Para detectar chamadas redundantes

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (Reader != null)
                    {
                        Reader.Dispose();
                    }
                    else if (Reply != null)
                    {
                        Reply.Dispose();
                    }
                }
                disposedValue = true;
            }
        }

        ~AppPacketBase()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
        #endregion
        #endregion
    }
}
