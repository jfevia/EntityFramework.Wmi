using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Management;

namespace System.Data.WMI
{
    public sealed class WMIDataReader : DbDataReader
    {
        private readonly CommandBehavior _behavior;
        private WMICommand _command;
        private bool _isClosed;
        private bool _isQueryExecuted;
        private ManagementObject _managementObject;
        private ManagementObjectCollection _managementObjectCollection;
        private ManagementObjectSearcher _managementObjectSearcher;
        private PropertyData[] _properties;
        private Dictionary<string, PropertyData> _propertyByName;
        private List<string> _propertyNames;
        private int _rowCount;
        private ManagementBaseObject[] _rows;

        /// <summary>
        ///     Initializes a new instance of the <see cref="WMIDataReader" /> class.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="behavior">The behavior.</param>
        public WMIDataReader(WMICommand command, CommandBehavior behavior)
        {
            _command = command ?? throw new ArgumentNullException(nameof(command));
            _behavior = behavior;

            RecordsAffected = -1;
        }

        /// <summary>
        ///     Gets the number of columns in the current row.
        /// </summary>
        public override int FieldCount => _managementObject.Properties.Count;

        /// <summary>
        ///     Gets the <see cref="object" /> with the specified ordinal.
        /// </summary>
        /// <param name="ordinal">
        ///     The ordinal.
        /// </param>
        /// <value>
        ///     The <see cref="object" />.
        /// </value>
        /// <returns>
        ///     The object.
        /// </returns>
        public override object this[int ordinal] => GetValue(ordinal);

        /// <summary>
        ///     Gets the <see cref="object" /> with the specified name.
        /// </summary>
        /// <param name="name">
        ///     The name.
        /// </param>
        /// <value>
        ///     The <see cref="object" />.
        /// </value>
        /// <returns>
        ///     The object.
        /// </returns>
        public override object this[string name] => GetValue(GetOrdinal(name));

        /// <summary>
        ///     Gets the number of rows changed, inserted, or deleted by execution of the SQL statement.
        /// </summary>
        public override int RecordsAffected { get; }

        /// <summary>
        ///     Gets a value that indicates whether this <see cref="T:System.Data.Common.DbDataReader" /> contains one or more
        ///     rows.
        /// </summary>
        public override bool HasRows => _managementObject.Properties.Count > 0;

        /// <summary>
        ///     Gets a value indicating whether the <see cref="T:System.Data.Common.DbDataReader" /> is closed.
        /// </summary>
        public override bool IsClosed => _isClosed;

        /// <summary>
        ///     Gets a value indicating the depth of nesting for the current row.
        /// </summary>
        public override int Depth => 0;

        /// <summary>
        ///     Returns a <see cref="T:System.Data.DataTable"></see> that describes the column metadata of the
        ///     <see cref="T:System.Data.Common.DbDataReader"></see>.
        /// </summary>
        /// <returns>
        ///     A <see cref="T:System.Data.DataTable"></see> that describes the column metadata.
        /// </returns>
        public override DataTable GetSchemaTable()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Gets the value of the specified column as a Boolean.
        /// </summary>
        /// <param name="ordinal">
        ///     The zero-based column ordinal.
        /// </param>
        /// <returns>
        ///     The value of the specified column.
        /// </returns>
        public override bool GetBoolean(int ordinal)
        {
            return Convert.ToBoolean(GetValue(ordinal));
        }

        /// <summary>
        ///     Gets the value of the specified column as a byte.
        /// </summary>
        /// <param name="ordinal">
        ///     The zero-based column ordinal.
        /// </param>
        /// <returns>
        ///     The value of the specified column.
        /// </returns>
        public override byte GetByte(int ordinal)
        {
            return Convert.ToByte(GetValue(ordinal));
        }

        /// <summary>
        ///     Reads a stream of bytes from the specified column, starting at location indicated by <paramref name="dataOffset" />
        ///     , into the buffer, starting at the location indicated by <paramref name="bufferOffset" />.
        /// </summary>
        /// <param name="ordinal">
        ///     The zero-based column ordinal.
        /// </param>
        /// <param name="dataOffset">
        ///     The index within the row from which to begin the read operation.
        /// </param>
        /// <param name="buffer">
        ///     The buffer into which to copy the data.
        /// </param>
        /// <param name="bufferOffset">
        ///     The index with the buffer to which the data will be copied.
        /// </param>
        /// <param name="length">
        ///     The maximum number of characters to read.
        /// </param>
        /// <returns>
        ///     The actual number of bytes read.
        /// </returns>
        /// <exception cref="System.NotSupportedException"></exception>
        public override long GetBytes(int ordinal, long dataOffset, byte[] buffer, int bufferOffset, int length)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        ///     Gets the value of the specified column as a single character.
        /// </summary>
        /// <param name="ordinal">
        ///     The zero-based column ordinal.
        /// </param>
        /// <returns>
        ///     The value of the specified column.
        /// </returns>
        public override char GetChar(int ordinal)
        {
            return Convert.ToChar(GetValue(ordinal));
        }

        /// <summary>
        ///     Reads a stream of characters from the specified column, starting at location indicated by
        ///     <paramref name="dataOffset" />, into the buffer, starting at the location indicated by
        ///     <paramref name="bufferOffset" />.
        /// </summary>
        /// <param name="ordinal">
        ///     The zero-based column ordinal.
        /// </param>
        /// <param name="dataOffset">
        ///     The index within the row from which to begin the read operation.
        /// </param>
        /// <param name="buffer">
        ///     The buffer into which to copy the data.
        /// </param>
        /// <param name="bufferOffset">
        ///     The index with the buffer to which the data will be copied.
        /// </param>
        /// <param name="length">
        ///     The maximum number of characters to read.
        /// </param>
        /// <returns>
        ///     The actual number of characters read.
        /// </returns>
        /// <exception cref="System.NotSupportedException"></exception>
        public override long GetChars(int ordinal, long dataOffset, char[] buffer, int bufferOffset, int length)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        ///     Gets name of the data type of the specified column.
        /// </summary>
        /// <param name="ordinal">The zero-based column ordinal.</param>
        /// <returns>
        ///     A string representing the name of the data type.
        /// </returns>
        public override string GetDataTypeName(int ordinal)
        {
            return GetPropertyDataFromOrdinal(ordinal).Type.ToString();
        }

        /// <summary>
        ///     Gets the value of the specified column as a <see cref="T:System.DateTime" /> object.
        /// </summary>
        /// <param name="ordinal">
        ///     The zero-based column ordinal.
        /// </param>
        /// <returns>
        ///     The value of the specified column.
        /// </returns>
        public override DateTime GetDateTime(int ordinal)
        {
            var value = GetString(ordinal);
            return ManagementDateTimeConverter.ToDateTime(value);
        }

        /// <summary>
        ///     Gets the value of the specified column as a <see cref="T:System.Decimal" /> object.
        /// </summary>
        /// <param name="ordinal">
        ///     The zero-based column ordinal.
        /// </param>
        /// <returns>
        ///     The value of the specified column.
        /// </returns>
        public override decimal GetDecimal(int ordinal)
        {
            return Convert.ToDecimal(GetValue(ordinal));
        }

        /// <summary>
        ///     Gets the value of the specified column as a double-precision floating point number.
        /// </summary>
        /// <param name="ordinal">
        ///     The zero-based column ordinal.
        /// </param>
        /// <returns>
        ///     The value of the specified column.
        /// </returns>
        public override double GetDouble(int ordinal)
        {
            return Convert.ToDouble(GetValue(ordinal));
        }

        /// <summary>
        ///     Gets the data type of the specified column.
        /// </summary>
        /// <param name="ordinal">The zero-based column ordinal.</param>
        /// <returns>
        ///     The data type of the specified column.
        /// </returns>
        public override Type GetFieldType(int ordinal)
        {
            var propertyData = GetPropertyDataFromOrdinal(ordinal);
            return propertyData.ToSystemType();
        }

        /// <summary>
        ///     Gets the value of the specified column as a single-precision floating point number.
        /// </summary>
        /// <param name="ordinal">
        ///     The zero-based column ordinal.
        /// </param>
        /// <returns>
        ///     The value of the specified column.
        /// </returns>
        public override float GetFloat(int ordinal)
        {
            return Convert.ToSingle(GetValue(ordinal));
        }

        /// <summary>
        ///     Gets the value of the specified column as a globally-unique identifier (GUID).
        /// </summary>
        /// <param name="ordinal">
        ///     The zero-based column ordinal.
        /// </param>
        /// <returns>
        ///     The value of the specified column.
        /// </returns>
        public override Guid GetGuid(int ordinal)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        ///     Gets the value of the specified column as a 16-bit signed integer.
        /// </summary>
        /// <param name="ordinal">
        ///     The zero-based column ordinal.
        /// </param>
        /// <returns>
        ///     The value of the specified column.
        /// </returns>
        public override short GetInt16(int ordinal)
        {
            return Convert.ToInt16(GetValue(ordinal));
        }

        /// <summary>
        ///     Gets the value of the specified column as a 32-bit signed integer.
        /// </summary>
        /// <param name="ordinal">
        ///     The zero-based column ordinal.
        /// </param>
        /// <returns>
        ///     The value of the specified column.
        /// </returns>
        public override int GetInt32(int ordinal)
        {
            return Convert.ToInt32(GetValue(ordinal));
        }

        /// <summary>
        ///     Gets the value of the specified column as a 64-bit signed integer.
        /// </summary>
        /// <param name="ordinal">
        ///     The zero-based column ordinal.
        /// </param>
        /// <returns>
        ///     The value of the specified column.
        /// </returns>
        public override long GetInt64(int ordinal)
        {
            return Convert.ToInt64(GetValue(ordinal));
        }

        /// <summary>
        ///     Gets the name of the column, given the zero-based column ordinal.
        /// </summary>
        /// <param name="ordinal">
        ///     The zero-based column ordinal.
        /// </param>
        /// <returns>
        ///     The name of the specified column.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        ///     Could not find property name from ordinal value
        /// </exception>
        public override string GetName(int ordinal)
        {
            return _command.Columns[ordinal];
        }

        /// <summary>
        ///     Gets the property data from ordinal.
        /// </summary>
        /// <param name="ordinal">The zero-based column ordinal.</param>
        /// <returns>The property data</returns>
        /// <exception cref="System.InvalidOperationException">Could not find property from ordinal value</exception>
        private PropertyData GetPropertyDataFromOrdinal(int ordinal)
        {
            var propertyName = _propertyNames[ordinal];
            return _propertyByName[propertyName];
        }

        /// <summary>
        ///     Gets the column ordinal given the name of the column.
        /// </summary>
        /// <param name="name">
        ///     The name of the column.
        /// </param>
        /// <returns>
        ///     The zero-based column ordinal.
        /// </returns>
        public override int GetOrdinal(string name)
        {
            return _propertyNames.IndexOf(name);
        }

        /// <summary>
        ///     Gets the value of the specified column as an instance of <see cref="T:System.String" />.
        /// </summary>
        /// <param name="ordinal">
        ///     The zero-based column ordinal.
        /// </param>
        /// <returns>
        ///     The value of the specified column.
        /// </returns>
        public override string GetString(int ordinal)
        {
            return Convert.ToString(GetValue(ordinal));
        }

        /// <summary>
        ///     Gets the value of the specified column as an instance of <see cref="T:System.Object" />.
        /// </summary>
        /// <param name="ordinal">
        ///     The zero-based column ordinal.
        /// </param>
        /// <returns>
        ///     The value of the specified column.
        /// </returns>
        public override object GetValue(int ordinal)
        {
            var propertyName = GetName(ordinal);
            return _managementObject.GetPropertyValue(propertyName);
        }

        /// <summary>
        ///     Populates an array of objects with the column values of the current row.
        /// </summary>
        /// <param name="values">
        ///     An array of <see cref="T:System.Object" /> into which to copy the attribute columns.
        /// </param>
        /// <returns>
        ///     The number of instances of <see cref="T:System.Object" /> in the array.
        /// </returns>
        /// <exception cref="System.NotSupportedException"></exception>
        public override int GetValues(object[] values)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        ///     Gets a value that indicates whether the column contains nonexistent or missing values.
        /// </summary>
        /// <param name="ordinal">
        ///     The zero-based column ordinal.
        /// </param>
        /// <returns>
        ///     true if the specified column is equivalent to <see cref="T:System.DBNull" />; otherwise false.
        /// </returns>
        public override bool IsDBNull(int ordinal)
        {
            return GetValue(ordinal) == null;
        }

        /// <summary>
        ///     Closes the <see cref="T:System.Data.Common.DbDataReader"></see> object.
        /// </summary>
        public override void Close()
        {
            _command = null;
        }

        /// <summary>
        ///     Advances the reader to the next result when reading the results of a batch of statements.
        /// </summary>
        /// <returns>
        ///     true if there are more result sets; otherwise false.
        /// </returns>
        /// <exception cref="NotSupportedException"></exception>
        public override bool NextResult()
        {
            throw new NotSupportedException();
        }

        /// <summary>
        ///     Advances the reader to the next record in a result set.
        /// </summary>
        /// <returns>
        ///     true if there are more rows; otherwise false.
        /// </returns>
        public override bool Read()
        {
            if (!_isQueryExecuted)
            {
                if (!(_command.Connection is WMIConnection connection))
                    throw new InvalidOperationException("Connection is not of type WMIConnection");

                _managementObjectSearcher = new ManagementObjectSearcher(connection.Path, _command.CommandText);
                _managementObjectCollection = _managementObjectSearcher.Get();

                _rows = new ManagementBaseObject[_managementObjectCollection.Count];
                _managementObjectCollection.CopyTo(_rows, 0);

                _isQueryExecuted = true;
            }

            var enumerator = GetEnumerator();
            if (enumerator.MoveNext())
            {
                _managementObject = (ManagementObject) enumerator.Current;
                if (_managementObject != null)
                {
                    _properties = new PropertyData[_managementObject.Properties.Count];
                    _managementObject.Properties.CopyTo(_properties, 0);

                    _propertyByName = _properties.ToDictionary(s => s.Name);
                    _propertyNames = _propertyByName.Keys.OrderBy(s => s).ToList();
                }

                _rowCount++;
            }
            else
                _rowCount = _managementObjectCollection.Count + 1;

            if (_rowCount > _managementObjectCollection.Count)
                _isClosed = true;

            return !_isClosed;
        }

        /// <summary>
        ///     Returns an <see cref="T:System.Collections.IEnumerator" /> that can be used to iterate through the rows in the data
        ///     reader.
        /// </summary>
        /// <returns>
        ///     An <see cref="T:System.Collections.IEnumerator" /> that can be used to iterate through the rows in the data reader.
        /// </returns>
        public override IEnumerator GetEnumerator()
        {
            return _managementObjectCollection.GetEnumerator();
        }
    }
}