using PangyaAPI.IFF.StructModels;
using System.Runtime.InteropServices;
namespace PangyaAPI.IFF.Models
{

    #region Struct Club.iff

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public class Club : IFFCommon
    {
        [field: MarshalAs(UnmanagedType.ByValTStr, SizeConst = 40)]
        public string MPet { get; set; }
        public ushort ClubType { get; set; }
        public ushort Power { get; set; }
        public ushort Control { get; set; }
        public ushort Impact { get; set; }
        public ushort Spin { get; set; }
        public ushort Curve { get; set; }

    }
    #endregion

}
