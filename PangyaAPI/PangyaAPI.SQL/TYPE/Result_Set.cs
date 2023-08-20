using System;
using System.Data;
using System.Globalization;
using System.Diagnostics;
using System.Data.Common;

namespace PangyaAPI.SQL
{
    public class ctx_res
	{
        protected DataRow Row;
        public uint cols;
		public ctx_res next;
        public void SetRow(DataRow _row)
        { Row = _row; }
        public DataRow getRow { get => Row; set { Row = value; } }
        public object[] data { get => Row.ItemArray;}

        public ctx_res()
        {
            cols = 0;
            next = null;
        }
        public bool Compare(ctx_res b)
		{
			this.cols = b.cols;
			this.Row = b.Row;
			this.next = b.next;
			return true;
		}

		public bool IsNotNull(int idx)
		{
			if (Row.ItemArray[idx] == DBNull.Value)
				return false;

			return true;
		}


        /// <summary>
        /// Gets the value of the specified column as a Boolean.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>The value of the specified column.</returns>
        public bool GetBoolean(int i)
        {
            object value = GetValue(i);
            if (int.TryParse(value as string, out var result))
            {
                return Convert.ToBoolean(result);
            }
            return Convert.ToBoolean(value);
        }

       
        /// <summary>
        /// Gets the value of the specified column as a byte.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>The value of the specified column.</returns>
        public byte GetByte(int i)
        {
            var fieldValue = GetFieldValue(i, checkNull: true);
            try
            {
                if (fieldValue is byte UByte)
                {
                    return UByte;
                }
                if (fieldValue is short bit)
                {
                    return Convert.ToByte(bit);
                }
                if (fieldValue is Int32 Int)
                {
                    return Convert.ToByte(fieldValue);
                }
                if (fieldValue is UInt32 UInt)
                {
                    return Convert.ToByte(fieldValue);
                }
                if (fieldValue is UInt64 U64)
                {
                    return Convert.ToByte(fieldValue);
                }
                if (fieldValue is Int64 I64)
                {
                    return Convert.ToByte(fieldValue);
                }
                if (fieldValue is ushort UShort)
                {
                    return Convert.ToByte(fieldValue);
                }
                if (fieldValue is short Short)
                {
                    return Convert.ToByte(fieldValue);
                }
                else
                {
                    Console.WriteLine(fieldValue.GetType());
                }
                return (byte)ChangeType(fieldValue, i, typeof(byte));
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                Console.WriteLine("Error : " + msg + ", Nmb: " + new StackTrace(ex).GetFrame(0).GetFileLineNumber());
                return 0;
            }
        }


        /// <summary>
        /// Gets the value of the specified column as a sbyte.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>The value of the specified column.</returns>
        public sbyte GetSByte(int i)
        {
            var fieldValue = GetFieldValue(i, checkNull: true);
            try
            {
                if (fieldValue is sbyte s)
                {
                    return s;
                }
                if (fieldValue is short bit)
                {
                    return Convert.ToSByte(bit);
                }
                if (fieldValue is Int32 Int)
                {
                    return Convert.ToSByte(fieldValue);
                }
                if (fieldValue is UInt32 UInt)
                {
                    return Convert.ToSByte(fieldValue);
                }
                if (fieldValue is UInt64 U64)
                {
                    return Convert.ToSByte(fieldValue);
                }
                if (fieldValue is Int64 I64)
                {
                    return Convert.ToSByte(fieldValue);
                }
                if (fieldValue is ushort UShort)
                {
                    return Convert.ToSByte(fieldValue);
                }
                if (fieldValue is short Short)
                {
                    return Convert.ToSByte(fieldValue);
                }
                else
                {
                    Console.WriteLine(fieldValue.GetType());
                }
                return (sbyte)ChangeType(fieldValue, i, typeof(sbyte));
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                Console.WriteLine("Error : " + msg + ", Nmb: " + new StackTrace(ex).GetFrame(0).GetFileLineNumber());
                return 0;
            }
        }

        /// <summary>
        /// Reads a stream of bytes from the specified column offset into the buffer an array starting at the given buffer offset.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <param name="fieldOffset">The index within the field from which to begin the read operation.</param>
        /// <param name="buffer">The buffer into which to read the stream of bytes.</param>
        /// <param name="bufferoffset">The index for buffer to begin the read operation.</param>
        /// <param name="length">The maximum length to copy into the buffer.</param>
        /// <returns>The actual number of bytes read.</returns>
        private object ChangeType(object Value, int fieldIndex, Type newType)
        {
            return Convert.ChangeType(Value, newType, CultureInfo.InvariantCulture);
        }

       
        /// <summary>
        /// Gets the value of the specified column as a single character.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>The value of the specified column.</returns>
        public char GetChar(int i)
        {
            return GetString(i)[0];
        }
        
        /// <summary>
        ///  Gets the value of the specified column as a <see cref="T:.Row.Types.DateTime" /> object.
        /// </summary>
        /// <remarks>
        ///  <para>No conversions are performed; therefore, the data retrieved must already be a <see cref="T:System.DateTime" /> object.</para>
        ///  <para>Call IsDBNull to check for null values before calling this method.</para>
        /// </remarks>
        /// <param name="column">The zero-based column ordinal.</param>
        /// <returns>The value of the specified column.</returns>
        public DateTime GetDateTime(int column)
        {
            var fieldValue = GetFieldValue(column, checkNull: true);
            if (fieldValue.ToString() =="0")
            {
                fieldValue = DateTime.MinValue;
            }
            try
            {
                if (fieldValue is DateTime UByte)
                {
                    return UByte;
                }
                
                return (DateTime)ChangeType(fieldValue, column, typeof(DateTime));
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                Console.WriteLine("Error : " + msg + ", Nmb: " + new StackTrace(ex).GetFrame(0).GetFileLineNumber());
                return DateTime.MinValue;
            }
        }

       
        /// <summary>
        /// Gets the value of the specified column as a <see cref="T:.Row.Types.Decimal" />.
        /// </summary>
        /// <param name="i">The index of the colum.</param>
        /// <returns>The value of the specified column as a <see cref="T:.Row.Types.Decimal" />.</returns>
        public Decimal GetDecimal(int i)
        {
            return (Decimal)(object)GetFieldValue(i, checkNull: false);
        }

        /// <summary>Gets the value of the specified column as a double-precision floating point number.</summary>
        /// <remarks>
        ///  <para>No conversions are performed; therefore, the data retrieved must already be a <see cref="T:System.Double" /> object.</para>
        ///  <para>Call <see cref="M:.Row.Client.RowReader.IsDBNull(System.Int32)" /> to check for null values before calling this method.</para>
        /// </remarks>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>The value of the specified column.</returns>
        public double GetDouble(int i)
        {
            var fieldValue = GetFieldValue(i, checkNull: true);
            if (fieldValue is Double Double)
            {
                return Double;
            }
            return Convert.ToDouble(fieldValue);
        }
        /// <summary>
        ///  Gets the value of the specified column as a single-precision floating point number.
        /// </summary>
        /// <remarks>
        ///  <para> No conversions are performed; therefore, the data retrieved must already be a <see cref="T:System.Single" /> object.</para>
        ///  <para> Call <see cref="M:.Row.Client.RowReader.IsDBNull(System.Int32)" /> to check for null values before calling this method.</para>
        /// </remarks>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>The value of the specified column.</returns>
        public float GetFloat(int i)
        {
            var fieldValue = GetFieldValue(i, checkNull: true);
            if (fieldValue is Single Single)
            {
                return Single;
            }
            return Convert.ToSingle(fieldValue);
        }

      
        /// <summary>
        /// Gets the value of the specified column as a globally-unique identifier(GUID).
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>The value of the specified column.</returns>
        public Guid GetGuid(int i)
        {
            object value = GetValue(i);
            if (value is Guid)
            {
                return (Guid)value;
            }
            if (value is string)
            {
                return new Guid(value as string);
            }
            if (value is byte[])
            {
                byte[] array = (byte[])value;
                if (array.Length == 16)
                {
                    return new Guid(array);
                }
            }
            return Guid.Empty;
        }
        /// <summary>Gets the value of the specified column as a 16-bit signed integer.</summary>
        /// <remarks>
        ///  <para>No conversions are performed; therefore, the data retrieved must already be a <see cref="T:System.Int16" /> value.</para>
        ///  <para>Call <see cref="M:.Row.Client.RowReader.IsDBNull(System.Int32)" /> to check for null values before calling this method.</para>
        /// </remarks>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>The value of the specified column.</returns>
        public short GetInt16(int i)
        {
            var fieldValue = GetFieldValue(i, checkNull: true);
            try
            {
                if (fieldValue is short Short)
                {
                    return Convert.ToInt16(fieldValue);
                }

                if (fieldValue is sbyte s)
                {
                    return Convert.ToInt16(fieldValue);
                }
                if (fieldValue is byte bit)
                {
                    return Convert.ToInt16(fieldValue);
                }
                if (fieldValue is Int32 Int)
                {
                    return Convert.ToInt16(fieldValue);
                }
                if (fieldValue is UInt32 UInt)
                {
                    return Convert.ToInt16(fieldValue);
                }
                if (fieldValue is UInt64 U64)
                {
                    return Convert.ToInt16(fieldValue);
                }
                if (fieldValue is Int64 I64)
                {
                    return Convert.ToInt16(fieldValue);
                }
                if (fieldValue is ushort UShort)
                {
                    return Convert.ToInt16(fieldValue);
                }
                else
                {
                    Console.WriteLine(fieldValue.GetType());
                }
                return (short)ChangeType(fieldValue, i, typeof(short));
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                Console.WriteLine("Error : " + msg + ", Nmb: " + new StackTrace(ex).GetFrame(0).GetFileLineNumber());
                return 0;
            }
        }

        /// <summary>Gets the value of the specified column as a 32-bit signed integer.</summary>
        /// <remarks>
        ///  <para>No conversions are performed; therefore, the data retrieved must already be a <see cref="T:System.Int32" /> value.</para>
        ///  <para>Call <see cref="M:.Row.Client.RowReader.IsDBNull(System.Int32)" /> to check for null values before calling this method.</para>
        /// </remarks>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>The value of the specified column.</returns>
        public int GetInt32(int i)
        {
            var fieldValue = GetFieldValue(i, checkNull: true);
            try
            {
                if (fieldValue is Int32 Int)
                {
                    return Int;
                }
                if (fieldValue is UInt32 UInt)
                {
                    return Convert.ToInt32(fieldValue);
                }
                if (fieldValue is UInt64 U64)
                {
                    return Convert.ToInt32(fieldValue);
                }
                if (fieldValue is Int64 I64)
                {
                    return Convert.ToInt32(fieldValue);
                }
                if (fieldValue is ushort UShort)
                {
                    return Convert.ToInt32(fieldValue);
                }
                if (fieldValue is short Short)
                {
                    return Convert.ToInt32(fieldValue);
                }
                else
                {
                    Console.WriteLine(fieldValue.GetType());
                }
                return (int)ChangeType(fieldValue, i, typeof(int));
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                Console.WriteLine("Error : " + msg + ", Nmb: " + new StackTrace(ex).GetFrame(0).GetFileLineNumber());
                return 0;
            }
        }


        /// <summary>Gets the value of the specified column as a 64-bit signed integer.</summary>
        /// <remarks>
        ///  <para>No conversions are performed; therefore, the data retrieved must already be a <see cref="T:System.Int64" /> value.</para>
        ///  <para>Call <see cref="M:.Row.Client.RowReader.IsDBNull(System.Int32)" /> to check for null values before calling this method.</para>
        /// </remarks>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>The value of the specified column.</returns>
        public long GetInt64(int i)
        {
            var fieldValue = GetFieldValue(i, checkNull: true);
            if (fieldValue is Int64 Int)
            {
                return Int;
            }
            return (long)ChangeType(fieldValue, i, typeof(long));
        }

      
        public string GetString(int i)
        {
            var fieldValue = GetFieldValue(i, checkNull: true);
           
            return fieldValue.ToString();
        }

      
        public TimeSpan GetTimeSpan(int column)
        {
            return ((TimeSpan)(object)GetFieldValue(column, checkNull: true));
        }

        /// <summary>
        /// Gets the value of the specified column in its native format.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>The value of the specified column.</returns>
        public object GetValue(int i)
        {
            
            var fieldValue = GetFieldValue(i, checkNull: false);
            
            
            return fieldValue;
        }

        /// <summary>
        /// Gets all attribute columns in the collection for the current row.
        /// </summary>
        /// <param name="values">An array of <see cref="T:System.Object" /> into which to copy the attribute columns.</param>
        /// <returns>The number of instances of <see cref="T:System.Object" /> in the array.</returns>
        public int GetValues(object[] values)
        {
            int num = Math.Min(values.Length, Row.ItemArray.Length);
            for (int i = 0; i < num; i++)
            {
                values[i] = GetValue(i);
            }
            return num;
        }

      
        public ushort GetUInt16(int column)
        {
            var fieldValue = GetFieldValue(column, checkNull: true);
            if (fieldValue is UInt16 UInt)
            {
                return UInt;
            }
            return (ushort)ChangeType(fieldValue, column, typeof(ushort));
        }

       
        /// <summary>Gets the value of the specified column as a 32-bit unsigned integer.</summary>
        /// <remarks>
        ///  <para>No conversions are performed; therefore, the data retrieved must already be a <see cref="T:System.UInt32" /> value.</para>
        ///  <para>Call <see cref="M:.Row.Client.RowReader.IsDBNull(System.Int32)" /> to check for null values before calling this method.</para>
        /// </remarks>
        /// <param name="column">The zero-based column ordinal.</param>
        /// <returns>The value of the specified column.</returns>
        public uint GetUInt32(int column)
        {
            var fieldValue = GetFieldValue(column, checkNull: true);
            try
            {
                if (fieldValue is UInt32 UInt)
                {
                    return UInt;
                }
                if (fieldValue is short Short)
                {
                    return Convert.ToUInt32(fieldValue);
                }
                if (fieldValue is Int32 Int)
                {
                    return Convert.ToUInt32(fieldValue);
                }
                if (fieldValue is UInt64 U64)
                {
                    return Convert.ToUInt32(fieldValue);
                }
                if (fieldValue is Int64 I64)
                {
                    return Convert.ToUInt32(fieldValue);
                }
                if (fieldValue is ushort UShort)
                {
                    return Convert.ToUInt32(fieldValue);
                }
                else
                {
                    Console.WriteLine(fieldValue.GetType());
                }
                return (uint)ChangeType(fieldValue, column, typeof(uint));
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                Console.WriteLine("Error : " + msg + ", Nmb: " + new StackTrace(ex).GetFrame(0).GetFileLineNumber());
                return 0;
            }
        }
        /// <summary>Gets the value of the specified column as a 64-bit unsigned integer.</summary>
        /// <remarks>
        ///  <para>No conversions are performed; therefore, the data retrieved must already be a <see cref="T:System.UInt64" /> value.</para>
        ///  <para>Call <see cref="M:.Row.Client.RowReader.IsDBNull(System.Int32)" /> to check for null values before calling this method.</para>
        /// </remarks>
        /// <param name="column">The zero-based column ordinal.</param>
        /// <returns>The value of the specified column.</returns>
        public ulong GetUInt64(int column)
        {
            var fieldValue = GetFieldValue(column, checkNull: true);
            if (fieldValue is UInt64 UInt)
            {
                return UInt;
            }
            return (ulong)ChangeType(fieldValue, column, typeof(ulong));
        }
        private object GetFieldValue(int index, bool checkNull)
        {
            var obj = Row.ItemArray[index];
            foreach (var property in obj.GetType().GetProperties())
            {
                Type type = property.PropertyType;

                TypeCode typeCode = Type.GetTypeCode(type);
                switch (typeCode)
                {
                    case TypeCode.Empty:
                        break;
                    case TypeCode.Object:
                        break;
                    case TypeCode.DBNull:
                        break;
                    case TypeCode.Boolean:
                    case TypeCode.Char:
                    case TypeCode.SByte:
                    case TypeCode.Byte:
                    case TypeCode.Int16:
                    case TypeCode.UInt16:
                    case TypeCode.Int32:
                    case TypeCode.UInt32:
                    case TypeCode.Int64:
                    case TypeCode.UInt64:
                    case TypeCode.Single:
                    case TypeCode.Double:
                    case TypeCode.Decimal:
                    case TypeCode.DateTime:
                    case TypeCode.String:
                        return property.GetValue(obj);
                    //case TypeCode.Boolean:
                    //    {

                    //    }
                    //    break;
                    //case TypeCode.Char:
                    //    {

                    //    }
                    //    break;
                    //case TypeCode.SByte:
                    //    {

                    //    }
                    //    break;
                    //case TypeCode.Byte:
                    //    {

                    //    }
                    //    break;
                    //case TypeCode.Int16:
                    //    {

                    //    }
                    //    break;
                    //case TypeCode.UInt16:
                    //    {

                    //    }
                    //    break;
                    //case TypeCode.Int32:
                    //    {

                    //    }
                    //    break;
                    //case TypeCode.UInt32:
                    //    {

                    //    }
                    //    break;
                    //case TypeCode.Int64:
                    //    {

                    //    }
                    //    break;
                    //case TypeCode.UInt64:
                    //    {

                    //    }
                    //    break;
                    //case TypeCode.Single:
                    //    {

                    //    }
                    //    break;
                    //case TypeCode.Double:
                    //    {

                    //    }
                    //    break;
                    //case TypeCode.Decimal:
                    //    {

                    //    }
                    //    break;
                    //case TypeCode.DateTime:
                    //    {

                    //    }
                    //    break;
                    //case TypeCode.String:
                    //    {

                    //    }
                    //    break; 
                    default:
                        {
                            Console.WriteLine("Object Type Name: " + typeCode);
                        }
                        break;
                }
            }
            return obj;
        }
    }

    public class Result_Set : System.IDisposable
	{
		public enum STATE_TYPE : uint
		{
			HAVE_DATA,
			_NO_DATA,
			_UPDATE_OR_DELETE,
			_ERROR
		}


		public Result_Set(uint _state)
		{
			this.m_state = _state;
			this.m_lines_affected = -1;
			this.m_data = null;
			this.m_curr_data = null;
			this.m_lines = 0;
			this.m_cols = 0;
		}

		public Result_Set(uint _state,
			uint _cols,
			int _lines_affected)
		{
			this.m_state = _state;
			this.m_lines_affected = _lines_affected;
			this.m_data = null;
			this.m_curr_data = null;
			this.m_lines = 0;
			this.m_cols = _cols;
		}

		public void Dispose()
		{
			destroy();
		}

		public void destroy()
		{
			if (m_data != null)
			{
				ctx_res pNext = null;
				uint i = 0;

				while ((pNext.Compare(m_data)))
				{
					if (m_data.getRow != null)
					{
						for (i = 0; i < m_data.cols; ++i)
						{
							if (m_data.getRow.ItemArray[i] != null)
							{
								m_data.getRow.ItemArray[i] = null;
							}
						}
						m_data.getRow = null;
					}
					m_data = pNext.next;

					pNext = null;
				}
			}

			m_data = null;
		}

		public uint reserve_cols(uint _cols)
		{
			if (_cols > 0)
			{
				addLineData();

				m_cols = m_curr_data.cols = _cols;
			}

			return _cols;
		}

		public ctx_res getFirstLine()
		{
			return m_data;
		}

		public void setLinesAffected(int _lines_affected)
		{
			m_lines_affected = _lines_affected;
		}

		public object getColAt(uint _index)
		{
			if (m_curr_data == null)
			{
				throw new System.Exception("Nao tem nenhum dados reservado, reserve primeiro.");
			}

			if ((int)_index < 0 || _index >= m_curr_data.cols)
			{
				throw new System.Exception("Index out of range.");
			}

			return m_curr_data.getRow[(int)_index];
		}

		public uint getNumLines()
		{
			return m_lines;
		}

		public uint getNumCols()
		{
			return m_cols;
		}

		public void setState(uint _state)
		{
			m_state = _state;
		}

		public uint getState()
		{
			return m_state;
		}

		public void addLine()
		{
			if (m_state == (uint)STATE_TYPE.HAVE_DATA)
			{
				reserve_cols(m_cols);
			}
		}

		protected ctx_res addLineData()
		{

			if (m_data == null)
			{
				m_data = new ctx_res();

				m_curr_data = m_data;

				// Init Dados
				m_curr_data.next = null;
				m_curr_data.cols = 0;
				m_curr_data.getRow = null;
			}
			else
			{
				m_curr_data.next = new ctx_res();

				m_curr_data = m_curr_data.next;

				// Init Dados
				m_curr_data.next = null;
				m_curr_data.cols = 0;
				m_curr_data.getRow = null;
			}

			++m_lines;

			return m_curr_data;
		}

        internal void setRow(DataRow _data)
        {
            if (m_curr_data == null)
            {
                throw new System.Exception("Nao tem nenhum dados reservado, reserve primeiro.");
            }

            m_curr_data.SetRow(_data);
        }

        protected uint m_state;
		protected int m_lines_affected;

		protected uint m_lines;
		protected uint m_cols;
		protected ctx_res m_data;
		protected ctx_res m_curr_data;
	}
}

