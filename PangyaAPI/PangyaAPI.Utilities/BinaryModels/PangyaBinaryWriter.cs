using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Xml.Linq;

namespace PangyaAPI.Utilities.BinaryModels
{
    public class PangyaBinaryWriter : IDisposable
    {
        protected Stream OutStream;
        protected bool leaveOpen;
        protected byte[] _buffer = new byte[16];
        protected Encoding Encoding { get; private set; }
        public int Size
        {
            get
            {
                return CreateBytes().Count();
            }
        }
        public Stream BaseStream
        {
            get
            {
                Flush();
                return OutStream;
            }
        }
        public byte[] GetBytes
        {
            get
            {
                return CreateBytes();
            }
        }

        public PangyaBinaryWriter()
        {
            Encoding = Encoding.UTF7;
            this.OutStream = new MemoryStream();
        }

        public PangyaBinaryWriter(Stream output) : this(output, Encoding.Default, false)
        {
        }

        public PangyaBinaryWriter(Stream output, Encoding encoding) : this(output, encoding, false)
        {
        }

        public PangyaBinaryWriter(Stream output, Encoding encoding, bool leaveOpen)
        {
            if (output == null)
                throw new ArgumentNullException(nameof(output));

            if (encoding == null)
                throw new ArgumentNullException(nameof(encoding));

            this.OutStream = output;
            this.Encoding = encoding;
            this.leaveOpen = leaveOpen;
        }
        // Clears all buffers for this writer and causes any buffered data to be
        // written to the underlying device. 
        public void Flush()
        {
            OutStream.Flush();
        }
        public long Seek(int offset, SeekOrigin origin)
        {
            return OutStream.Seek(offset, origin);
        }

        // Writes a boolean to this stream. A single byte is written to the stream
        // with the value 0 representing false or the value 1 representing true.
        // 
        public void Write(bool value)
        {
            _buffer[0] = (byte)(value ? 1 : 0);
            OutStream.Write(_buffer, 0, 1);
        }

        // Writes a byte to this stream. The current position of the stream is
        // advanced by one.
        // 
        public void Write(byte value)
        {
            OutStream.WriteByte(value);
        }

        // Writes a signed byte to this stream. The current position of the stream 
        // is advanced by one.
        // 

        public void Write(sbyte value)
        {
            OutStream.WriteByte((byte)value);
        }

        // Writes a byte array to this stream.
        // 
        // This default implementation calls the Write(Object, int, int)
        // method to write the byte array.
        // 
        public void Write(byte[] buffer)
        {
            if (buffer == null)
                throw new ArgumentNullException("buffer");
            Contract.EndContractBlock();
            OutStream.Write(buffer, 0, buffer.Length);
        }

        // Writes a section of a byte array to this stream.
        //
        // This default implementation calls the Write(Object, int, int)
        // method to write the byte array.
        // 
        public void Write(byte[] buffer, int index, int count)
        {
            OutStream.Write(buffer, index, count);
        }


        // Writes a character to this stream. The current position of the stream is
        // advanced by two.
        // Note this method cannot handle surrogates properly in UTF-8.
        // 
        [System.Security.SecuritySafeCritical]  // auto-generated
        public unsafe void Write(char ch)
        {
            if (Char.IsSurrogate(ch))
                throw new Exception("Arg_SurrogatesNotAllowedAsSingleChar");
            Contract.EndContractBlock();

            byte[] charBytes = Encoding.GetBytes(new char[] { ch });
            WriteBytes(charBytes, charBytes.Length);

        }

        // Writes a character array to this stream.
        // 
        // This default implementation calls the Write(Object, int, int)
        // method to write the character array.
        // 
        public void Write(char[] chars)
        {
            if (chars == null)
                throw new ArgumentNullException("chars");
            Contract.EndContractBlock();

            byte[] bytes = Encoding.GetBytes(chars, 0, chars.Length);
            OutStream.Write(bytes, 0, bytes.Length);
        }

        // Writes a section of a character array to this stream.
        //
        // This default implementation calls the Write(Object, int, int)
        // method to write the character array.
        // 
        public void Write(char[] chars, int index, int count)
        {
            byte[] bytes = Encoding.GetBytes(chars, index, count);
            OutStream.Write(bytes, 0, bytes.Length);
        }


        // Writes a double to this stream. The current position of the stream is
        // advanced by eight.
        // 
        [System.Security.SecuritySafeCritical]  // auto-generated
        public unsafe void Write(double value)
        {
            ulong TmpValue = *(ulong*)&value;
            _buffer[0] = (byte)TmpValue;
            _buffer[1] = (byte)(TmpValue >> 8);
            _buffer[2] = (byte)(TmpValue >> 16);
            _buffer[3] = (byte)(TmpValue >> 24);
            _buffer[4] = (byte)(TmpValue >> 32);
            _buffer[5] = (byte)(TmpValue >> 40);
            _buffer[6] = (byte)(TmpValue >> 48);
            _buffer[7] = (byte)(TmpValue >> 56);
            OutStream.Write(_buffer, 0, 8);
        }

        public void Write(decimal value)
        {
            int[] bits = decimal.GetBits(value);
            byte[] bytes = new byte[bits.Length * 4];

            for (int i = 0; i < bits.Length; i++)
            {
                byte[] intBytes = BitConverter.GetBytes(bits[i]);
                Buffer.BlockCopy(intBytes, 0, bytes, i * 4, 4);
            }
            OutStream.Write(bytes, 0, 16);
        }

        // Writes a two-byte signed integer to this stream. The current position of
        // the stream is advanced by two.
        // 
        public void Write(short value)
        {
            _buffer[0] = (byte)value;
            _buffer[1] = (byte)(value >> 8);
            OutStream.Write(_buffer, 0, 2);
        }

        // Writes a two-byte unsigned integer to this stream. The current position
        // of the stream is advanced by two.
        // 

        public void Write(ushort value)
        {
            _buffer[0] = (byte)value;
            _buffer[1] = (byte)(value >> 8);
            OutStream.Write(_buffer, 0, 2);
        }

        // Writes a four-byte signed integer to this stream. The current position
        // of the stream is advanced by four.
        // 
        public void Write(int value)
        {
            _buffer[0] = (byte)value;
            _buffer[1] = (byte)(value >> 8);
            _buffer[2] = (byte)(value >> 16);
            _buffer[3] = (byte)(value >> 24);
            OutStream.Write(_buffer, 0, 4);
        }

        // Writes a four-byte unsigned integer to this stream. The current position
        // of the stream is advanced by four.
        // 

        public void Write(uint value)
        {
            _buffer[0] = (byte)value;
            _buffer[1] = (byte)(value >> 8);
            _buffer[2] = (byte)(value >> 16);
            _buffer[3] = (byte)(value >> 24);
            OutStream.Write(_buffer, 0, 4);
        }

        // Writes an eight-byte signed integer to this stream. The current position
        // of the stream is advanced by eight.
        // 
        public void Write(long value)
        {
            _buffer[0] = (byte)value;
            _buffer[1] = (byte)(value >> 8);
            _buffer[2] = (byte)(value >> 16);
            _buffer[3] = (byte)(value >> 24);
            _buffer[4] = (byte)(value >> 32);
            _buffer[5] = (byte)(value >> 40);
            _buffer[6] = (byte)(value >> 48);
            _buffer[7] = (byte)(value >> 56);
            OutStream.Write(_buffer, 0, 8);
        }

        // Writes an eight-byte unsigned integer to this stream. The current 
        // position of the stream is advanced by eight.
        // 

        public void Write(ulong value)
        {
            _buffer[0] = (byte)value;
            _buffer[1] = (byte)(value >> 8);
            _buffer[2] = (byte)(value >> 16);
            _buffer[3] = (byte)(value >> 24);
            _buffer[4] = (byte)(value >> 32);
            _buffer[5] = (byte)(value >> 40);
            _buffer[6] = (byte)(value >> 48);
            _buffer[7] = (byte)(value >> 56);
            OutStream.Write(_buffer, 0, 8);
        }

        // Writes a float to this stream. The current position of the stream is
        // advanced by four.
        // 
        [System.Security.SecuritySafeCritical]  // auto-generated
        public unsafe void Write(float value)
        {
            uint TmpValue = *(uint*)&value;
            _buffer[0] = (byte)TmpValue;
            _buffer[1] = (byte)(TmpValue >> 8);
            _buffer[2] = (byte)(TmpValue >> 16);
            _buffer[3] = (byte)(TmpValue >> 24);
            OutStream.Write(_buffer, 0, 4);
        }


        // Writes a length-prefixed string to this stream in the BinaryWriter's
        // current Encoding. This method first writes the length of the string as 
        // a four-byte unsigned integer, and then writes that many characters 
        // to the stream.
        //
        public PangyaBinaryWriter(short ID)
        {
            this.OutStream = new MemoryStream();
            Write(ID);
        }

        public void Skip(int count)
        {
            Seek(count, 1);
        }

        public void Seek(long offset, int origin)
        {
            OutStream.Seek(offset, (SeekOrigin)origin);
        }


        public bool WriteStr(string message, int length)
        {

            try
            {
                if (message == null)
                {
                    message = string.Empty;
                }

                var ret = new byte[length];
                Encoding.GetBytes(message).Take(length).ToArray().CopyTo(ret, 0);

                Write(ret);
            }
            catch
            {
                return false;
            }
            return true;
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

        public bool WritePStr(string data)
        {
            if (data == null) data = "";
            try
            {
                var encoded = Encoding.GetBytes(data);
                var length = encoded.Length;
                if (length >= ushort.MaxValue)
                {
                    return false;
                }
                Write((short)length);
                Write(encoded);
            }
            catch
            {
                return false;
            }
            return true;
        }


        public bool WriteBytes(byte[] message, int length)
        {
            try
            {
                if (message == null)
                    message = new byte[length];

                var result = new byte[length];

                Buffer.BlockCopy(message, 0, result, 0, length);

                Write(result);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool Write(byte[] message, int length)
        {
            try
            {
                if (message == null)
                    message = new byte[length];

                var result = new byte[length];

                Buffer.BlockCopy(message, 0, result, 0, message.Length);

                Write(result);
            }
            catch
            {
                return false;
            }
            return true;
        }
        public bool WriteZero(int Lenght)
        {
            try
            {
                Write(new byte[Lenght]);
            }
            catch
            {
                return false;
            }
            return true;
        }
        public bool WriteUInt16(ushort value)
        {
            try
            {
                Write(value);
            }
            catch
            {
                return false;
            }
            return true;
        }
        public bool WriteUInt16(int value)
        {
            try
            {
                Write((ushort)value);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool WriteUInt16(uint value)
        {
            try
            {
                Write((ushort)value);
            }
            catch
            {
                return false;
            }
            return true;
        }


        public bool WriteByte(byte value)
        {
            try
            {
                Write(value);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool WriteByte(int value)
        {
            try
            {
                Write(Convert.ToByte(value));
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool WriteSingle(float value)
        {
            try
            {
                Write(value);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool WriteUInt32(uint value)
        {
            try
            {
                Write(value);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool WriteInt32(int value)
        {
            try
            {
                Write(value);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool WriteUInt64(ulong value)
        {
            try
            {
                Write(value);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool WriteInt64(long value)
        {
            try
            {
                Write(value);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool WriteDouble(double value)
        {
            try
            {
                Write(value);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool WriteStruct(object value)
        {
            try
            {
                int size = Marshal.SizeOf(value);
                byte[] arr = new byte[size];

                IntPtr ptr = Marshal.AllocHGlobal(size);
                Marshal.StructureToPtr(value, ptr, true);
                Marshal.Copy(ptr, arr, 0, size);
                Marshal.FreeHGlobal(ptr);
                Write(arr);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool WriteStruct(object value, object value_ori)
        {
            try
            {
                int size = Marshal.SizeOf(value_ori);
                byte[] arr = new byte[size];

                IntPtr ptr = Marshal.AllocHGlobal(size);
                Marshal.StructureToPtr(value, ptr, false);
                Marshal.Copy(ptr, arr, 0, size);
                Marshal.FreeHGlobal(ptr);
                Write(arr);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool WriteStruct(object value, int size)
        {
            try
            {
                byte[] arr = new byte[size];

                IntPtr ptr = Marshal.AllocHGlobal(size);
                Marshal.StructureToPtr(value, ptr, false);
                Marshal.Copy(ptr, arr, 0, size);
                Marshal.FreeHGlobal(ptr);
                Write(arr);
            }
            catch
            {
                return false;
            }
            return true;
        }
        public bool WriteHexArray(string _value)
        {
            try
            {
                _value = _value.Replace(" ", "");
                int _size = _value.Length / 2;
                byte[] _result = new byte[_size];
                for (int ii = 0; ii < _size; ii++)
                    WriteByte(Convert.ToByte(_value.Substring(ii * 2, 2), 16));
            }
            catch
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// Write Pangya Time
        /// </summary>
        /// <returns></returns>
        public bool WriteTime(DateTime? date)
        {
            try
            {
                if (date.HasValue == false || date?.Ticks == 0)
                {
                    Write(new byte[16]);
                    return true;
                }
                WriteUInt16((ushort)date?.Year);
                WriteUInt16((ushort)date?.Month);
                WriteUInt16(Convert.ToUInt16(date?.DayOfWeek));
                WriteUInt16((ushort)date?.Day);
                WriteUInt16((ushort)date?.Hour);
                WriteUInt16((ushort)date?.Minute);
                WriteUInt16((ushort)date?.Second);
                WriteUInt16((ushort)date?.Millisecond);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Write Pangya Time
        /// </summary>
        /// <returns></returns>
        public bool WriteTime()
        {
            DateTime date = DateTime.Now;
            try
            {
                WriteUInt16((ushort)date.Year);
                WriteUInt16((ushort)date.Month);
                WriteUInt16((ushort)date.DayOfWeek);
                WriteUInt16((ushort)date.Day);
                WriteUInt16((ushort)date.Hour);
                WriteUInt16((ushort)date.Minute);
                WriteUInt16((ushort)date.Second);
                WriteUInt16((ushort)date.Millisecond);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void WriteObject(object _obj)
        {
            foreach (var property in _obj.GetType().GetProperties())
            {
                Type type = property.PropertyType;

                TypeCode typeCode = Type.GetTypeCode(type);
                var obj = property.GetValue(_obj);
                switch (typeCode)
                {
                    case TypeCode.Empty:
                        break;
                    case TypeCode.Object:
                        break;
                    case TypeCode.DBNull:
                        break;
                    case TypeCode.Boolean:
                        {
                            Write((bool)obj);
                        }
                        break;
                    case TypeCode.Char:
                        {
                            Write((char)obj);
                        }
                        break;
                    case TypeCode.SByte:
                        {
                            Write((sbyte)obj);
                        }
                        break;

                    case TypeCode.Byte:
                        {
                            Write((byte)obj);
                        }
                        break;
                    case TypeCode.Int16:
                        {
                            Write((short)obj);
                        }
                        break;
                    case TypeCode.UInt16:
                        {
                            WriteUInt16((UInt16)obj);
                        }
                        break;
                    case TypeCode.Int32:
                        {
                            WriteInt32((int)obj);
                        }
                        break;
                    case TypeCode.UInt32:
                        WriteUInt32((UInt32)obj);
                        break;
                    case TypeCode.Int64:
                        {
                            WriteInt64((long)obj);
                        }
                        break;
                    case TypeCode.UInt64:
                        {
                            WriteUInt64((ulong)obj);
                        }
                        break;
                    case TypeCode.Single:
                        {
                            WriteSingle((Single)obj);
                        }
                        break;
                    case TypeCode.Double:
                        {
                            WriteDouble((Double)obj);
                        }
                        break;
                    case TypeCode.Decimal:
                        {
                            Write((decimal)obj);
                        }
                        break;
                    case TypeCode.DateTime:
                        {
                            WriteTime((DateTime)obj);
                        }
                        break;
                    case TypeCode.String:
                        {
                            WritePStr((string)obj);
                        }
                        break;
                    default:
                        {
                            Console.WriteLine("Object Type Name: " + typeCode);
                        }
                        break;
                }
            }
            // return obj;
        }
        public void SaveWrite(string name)
        {
            File.WriteAllBytes(name, GetBytes);
        }
        /// <summary>
        /// GetBytes Written in Binary
        /// </summary>
        /// <returns>Array Of Bytes</returns>
        protected byte[] CreateBytes()
        {
            if (OutStream is MemoryStream stream)
                return ((MemoryStream)OutStream).ToArray();


            using (var memoryStream = new MemoryStream())
            {
                memoryStream.GetBuffer();
                OutStream.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }

        public void Clear()
        {
            OutStream = new MemoryStream();
        }

        public void Dispose()
        {
            if (!leaveOpen)
                OutStream.Dispose();
        }

        public void Close()
        {
            leaveOpen = true;
            Dispose();
        }

        public void WriteFile(string filePath)
        {
            if (File.Exists(filePath))
            {
              //  File.WriteAllBytes(filePath, GetBytes);
            }
            else
            {
                File.WriteAllBytes(filePath, GetBytes);
            }
        }
    }

}
