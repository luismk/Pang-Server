using System;
using System.Runtime.InteropServices;
using System.Text;

namespace PangyaAPI.IFF.Models
{

    #region Struct Desc.iff
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public class Desc
    {
        public uint TypeID { get; set; }
        [field: MarshalAs(UnmanagedType.ByValArray, SizeConst = 512)]
        byte[] DescriptionInBytes { get; set; }//4 start position
        public string Description 
        {
            get
            {
              return  Encoding.GetEncoding("Shift_JIS").GetString(DescriptionInBytes).Replace("\0", "");
            }
            set 
            {

                DescriptionInBytes = new byte[512];
                Buffer.BlockCopy(Encoding.GetEncoding("Shift_JIS").GetBytes(value), 0, DescriptionInBytes, 0, Math.Min(value.Length, 64));
            }
        }

    }
    #endregion

}
