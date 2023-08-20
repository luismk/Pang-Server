using PangyaAPI.SQL.Manager;
namespace PangyaAPI.SQL
{
    public enum type_SqlDbType
    {
        _default = -1,
        BigInt = 0,
        Binary = 1,
        Bit = 2,
        Char = 3,
        DateTime = 4,
        Decimal = 5,
        Float = 6,
        Image = 7,
        Int = 8,
        Money = 9,
        NChar = 10,
        NText = 11,
        NVarChar = 12,
        Real = 13,
        UniqueIdentifier = 14,
        SmallDateTime = 0xF,
        SmallInt = 0x10,
        SmallMoney = 17,
        Text = 18,
        Timestamp = 19,
        TinyInt = 20,
        VarBinary = 21,
        VarChar = 22,
        Variant = 23,
        Xml = 25,
        Udt = 29,
        Structured = 30,
        Date = 0x1F,
        Time = 0x20,
        DateTime2 = 33,
        DateTimeOffset = 34
    }

    public class pangya_base_db : System.IDisposable
	{
		public pangya_base_db()
		{
		}

		public void Dispose()
		{
		}
		public static bool compare(exec_query _query1, exec_query _query2)
		{
			return _query1.getQuery().CompareTo(_query2.getQuery()) == 0;
		}
	}
}