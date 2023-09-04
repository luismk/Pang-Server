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

        public void AddFixedString(string _string, int size)
        {

            AddUInt16((ushort)size);

            if (_string.size() >= size)
                AddPlain(_string.c_str(), size);
            else
            {

                AddPlain(_string.c_str(), _string.size());

                var diff = size - _string.size();

                if (diff > 0)
                    AddZeroByte(diff);
            }
        }


        public Object ReadObject(object obj)
        {
            foreach (var property in obj.GetType().GetProperties())
            {
                Type type = property.PropertyType;

                TypeCode typeCode = Type.GetTypeCode(type);
                switch (typeCode)
                {
                    case TypeCode.Empty:
                        break;
                    case TypeCode.Object:
                        {
                            if (type.Name == "Byte[]")
                            {
                                var obj_value = (byte[])obj;
                                for (int i = 0; i < obj_value.Length; i++)
                                {
                                    ReadByte(out byte _int8);
                                    obj_value[i] = _int8;
                                }
                                property.SetValue(obj, obj_value);
                            }
                            if (type.Name == "Long[]")
                            {
                                var obj_value = (long[])obj;
                                for (int i = 0; i < obj_value.Length; i++)
                                {
                                    ReadInt64(out long _int64);
                                    obj_value[i] = _int64;
                                }
                                property.SetValue(obj, obj_value);
                            }
                        }
                        break;
                    case TypeCode.DBNull:
                        break;
                    case TypeCode.Boolean:
                        {
                            ReadByte(out byte value);
                            property.SetValue(obj, value);
                        }
                        break;
                    case TypeCode.Char:
                        {
                         //   property.SetValue(obj, ReadChar());
                        }
                        break;
                    case TypeCode.SByte:
                        {
                           // property.SetValue(obj, ReadSByte());
                        }
                        break;
                    case TypeCode.Byte:
                        {
                            ReadByte(out byte value);
                            property.SetValue(obj, value);
                        }
                        break;
                    case TypeCode.Int16:
                        {
                            ReadInt16(out short value);
                            property.SetValue(obj, value);
                        }
                        break;
                    case TypeCode.UInt16:
                        {
                            property.SetValue(obj, ReadUInt16());
                        }
                        break;
                    case TypeCode.Int32:
                        {
                            ReadInt32(out int value);
                            property.SetValue(obj, value);
                        }
                        break;
                    case TypeCode.UInt32:
                        property.SetValue(obj, ReadUInt32());
                        break;
                    case TypeCode.Int64:
                        {
                            ReadInt64(out long value);
                            property.SetValue(obj, value);
                        }
                        break;
                    case TypeCode.UInt64:
                        {
                            property.SetValue(obj, ReadUInt64());
                        }
                        break;
                    case TypeCode.Single:
                        {
                            //property.SetValue(obj, ReadSingle());
                        }
                        break;
                    case TypeCode.Double:
                        {
                           // property.SetValue(obj, ReadDouble());
                        }
                        break;
                    case TypeCode.Decimal:
                        {
                            //property.SetValue(obj, ReadDecimal());
                        }
                        break;
                    case TypeCode.DateTime:
                        {
                          //  property.SetValue(obj, ReadDateTime());
                        }
                        break;
                    case TypeCode.String:
                        {
                            property.SetValue(obj, ReadString());
                        }
                        break;
                    default:
                        {
                            Console.WriteLine("Object Type Name: " + typeCode);
                        }
                        break;
                }
            }
            return obj;
        }

        public void Version_Decrypt(uint @packet_version)
        {
            Pang.Packet_Ver_Decrypt(ref @packet_version);
        }
    }
}
