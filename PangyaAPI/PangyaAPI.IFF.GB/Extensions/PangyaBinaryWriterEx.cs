using PangyaAPI.IFF.StructModels;
using PangyaAPI.Utilities.BinaryModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static PangyaAPI.Utilities.Tools;
namespace PangyaAPI.IFF
{
    public static class PangyaBinary
    {
        /// <summary>
        /// Versão 2.1
        /// </summary>
        /// <param name="reader">class de escrita</param>
        /// <param name="value">Part.iff(Part.cs) por exemplo</param>
        /// <param name="name">nome por exemplo, voce pode escrever em japones sem o (?????)</param>
        /// <param name="sSize">tamanho por exemplo, é 64 quanto é IffCommon</param>
        /// <param name="local">local por exemplo o local é 8 ou 4, dependendo da posição</param>
        public static void Write(this PangyaBinaryWriter reader, object value, string name, int sSize = 64, int local = 8)
        {
            int size = Marshal.SizeOf(value);
            byte[] arr = new byte[size];
            byte[] name_bytes = new byte[sSize];
            var encoded = Encoding.GetEncoding("Shift_JIS").GetBytes(name);
            Buffer.BlockCopy(encoded, 0, name_bytes, 0, encoded.Length > sSize ? sSize : encoded.Length);//maximo é 64

            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(value, ptr, true);
            Marshal.Copy(ptr, arr, 0, size);
            Marshal.FreeHGlobal(ptr);
            Buffer.BlockCopy(name_bytes, 0, arr, local, name_bytes.Length);
            reader.Write(arr, arr.Length);
        }

        public static void ReadIff(this PangyaBinaryReader reader, ref object value)
        {
            if (value is IFFCommon)
            {

            }
            else
            {
                var count = Marshal.SizeOf(value);

                byte[] recordData = reader.ReadBytes(count);

                if (recordData.Length != count)
                {
                    throw new Exception(
                        $"The record length ({recordData.Length}) mismatches the length of the passed structure ({count})");
                }


                IntPtr ptr = Marshal.AllocHGlobal(count);

                Marshal.Copy(recordData, 0, ptr, count);

                value = Marshal.PtrToStructure(ptr, value.GetType());
                Marshal.FreeHGlobal(ptr);
            }
        }
    }
}
