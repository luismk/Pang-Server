using PangyaAPI.IFF.StructModels;
using System.Runtime.InteropServices;
namespace PangyaAPI.IFF.Models
{
    /// <summary>
    /// Is Struct file CadieMagicBoxRandom.iff
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public class CadieMagicBoxRandom
    {
        public uint Index { get; set; }
        public uint TypeID { get; set; }

        public uint Qty { get; set; }

        public uint Rate { get; set; }
    }
}
