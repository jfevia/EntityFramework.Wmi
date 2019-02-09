using System.ComponentModel;
using System.Data.Common;

namespace System.Data.WMI
{
    public sealed class WMIParameter : DbParameter
    {
        /// <summary>
        ///     This value represents an "unknown" <see cref="DbType" />.
        /// </summary>
        private const DbType UnknownDbType = (DbType) (-1);

        private DbType _dbType;
        private object _objValue;

        /// <summary>
        ///     Constructor used when creating for use with a specific command.
        /// </summary>
        /// <param name="command">
        ///     The command associated with this parameter.
        /// </param>
        public WMIParameter(IDbCommand command)
            : this()
        {
            Command = command;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="WMIParameter" /> class.
        /// </summary>
        public WMIParameter()
            : this(null, UnknownDbType, 0, null, DataRowVersion.Current)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="WMIParameter" /> class.
        /// </summary>
        /// <param name="parameterName">Name of the parameter.</param>
        public WMIParameter(string parameterName)
            : this(parameterName, UnknownDbType, 0, null, DataRowVersion.Current)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="WMIParameter" /> class.
        /// </summary>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="value">The initial value of the parameter.</param>
        public WMIParameter(string parameterName, object value)
            : this(parameterName, UnknownDbType, 0, null, DataRowVersion.Current)
        {
            Value = value;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="WMIParameter" /> class.
        /// </summary>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="dbType">The data type of the parameter.</param>
        public WMIParameter(string parameterName, DbType dbType)
            : this(parameterName, dbType, 0, null, DataRowVersion.Current)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="WMIParameter" /> class.
        /// </summary>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="dbType">The data type.</param>
        /// <param name="sourceColumn">The source column.</param>
        public WMIParameter(string parameterName, DbType dbType, string sourceColumn)
            : this(parameterName, dbType, 0, sourceColumn, DataRowVersion.Current)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="WMIParameter" /> class.
        /// </summary>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="dbType">The data type.</param>
        /// <param name="sourceColumn">The source column.</param>
        /// <param name="rowVersion">The source version information.</param>
        public WMIParameter(string parameterName, DbType dbType, string sourceColumn, DataRowVersion rowVersion)
            : this(parameterName, dbType, 0, sourceColumn, rowVersion)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="WMIParameter" /> class.
        /// </summary>
        /// <param name="dbType">The data type of the parameter.</param>
        public WMIParameter(DbType dbType)
            : this(null, dbType, 0, null, DataRowVersion.Current)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="WMIParameter" /> class.
        /// </summary>
        /// <param name="dbType">The data type of the parameter.</param>
        /// <param name="value">The initial value of the parameter.</param>
        public WMIParameter(DbType dbType, object value)
            : this(null, dbType, 0, null, DataRowVersion.Current)
        {
            Value = value;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="WMIParameter" /> class.
        /// </summary>
        /// <param name="dbType">The data type of the parameter.</param>
        /// <param name="sourceColumn">The source column.</param>
        public WMIParameter(DbType dbType, string sourceColumn)
            : this(null, dbType, 0, sourceColumn, DataRowVersion.Current)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="WMIParameter" /> class.
        /// </summary>
        /// <param name="dbType">The data type.</param>
        /// <param name="sourceColumn">The source column.</param>
        /// <param name="rowVersion">The row version information.</param>
        public WMIParameter(DbType dbType, string sourceColumn, DataRowVersion rowVersion)
            : this(null, dbType, 0, sourceColumn, rowVersion)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="WMIParameter" /> class.
        /// </summary>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="parameterType">The data type.</param>
        /// <param name="parameterSize">The size of the parameter.</param>
        public WMIParameter(string parameterName, DbType parameterType, int parameterSize)
            : this(parameterName, parameterType, parameterSize, null, DataRowVersion.Current)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="WMIParameter" /> class.
        /// </summary>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="parameterType">The data type.</param>
        /// <param name="parameterSize">The size of the parameter.</param>
        /// <param name="sourceColumn">The source column.</param>
        public WMIParameter(string parameterName, DbType parameterType, int parameterSize, string sourceColumn)
            : this(parameterName, parameterType, parameterSize, sourceColumn, DataRowVersion.Current)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="WMIParameter" /> class.
        /// </summary>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="parameterType">Type of the parameter.</param>
        /// <param name="parameterSize">Size of the parameter.</param>
        /// <param name="sourceColumn">The source column.</param>
        /// <param name="sourceVersion">The source version.</param>
        public WMIParameter(string parameterName, DbType parameterType, int parameterSize, string sourceColumn, DataRowVersion sourceVersion)
        {
            ParameterName = parameterName;
            _dbType = parameterType;
            SourceColumn = sourceColumn;
            SourceVersion = sourceVersion;
            Size = parameterSize;
            IsNullable = true;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="WMIParameter" /> class.
        /// </summary>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="parameterType">The data type.</param>
        /// <param name="parameterSize">The size of the parameter.</param>
        /// <param name="direction">Only input parameters are supported in WMI.</param>
        /// <param name="isNullable">Ignored.</param>
        /// <param name="sourceColumn">The source column.</param>
        /// <param name="rowVersion">The row version information.</param>
        /// <param name="value">The initial value to assign the parameter.</param>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public WMIParameter(string parameterName, DbType parameterType, int parameterSize, ParameterDirection direction, bool isNullable, string sourceColumn, DataRowVersion rowVersion, object value)
            : this(parameterName, parameterType, parameterSize, sourceColumn, rowVersion)
        {
            Direction = direction;
            IsNullable = isNullable;
            Value = value;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="WMIParameter" /> class.
        /// </summary>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="parameterType">The data type.</param>
        /// <param name="parameterSize">The size of the parameter.</param>
        /// <param name="direction">Only input parameters are supported in WMI.</param>
        /// <param name="sourceColumn">The source column.</param>
        /// <param name="rowVersion">The row version information.</param>
        /// <param name="sourceColumnNullMapping">Whether or not this parameter is for comparing NULL's.</param>
        /// <param name="value">The initial value to assign the parameter.</param>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public WMIParameter(string parameterName, DbType parameterType, int parameterSize, ParameterDirection direction, string sourceColumn, DataRowVersion rowVersion, bool sourceColumnNullMapping, object value)
            : this(parameterName, parameterType, parameterSize, sourceColumn, rowVersion)
        {
            Direction = direction;
            SourceColumnNullMapping = sourceColumnNullMapping;
            Value = value;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="WMIParameter" /> class.
        /// </summary>
        /// <param name="parameterType">The data type.</param>
        /// <param name="parameterSize">The size of the parameter.</param>
        public WMIParameter(DbType parameterType, int parameterSize)
            : this(null, parameterType, parameterSize, null, DataRowVersion.Current)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="WMIParameter" /> class.
        /// </summary>
        /// <param name="parameterType">The data type.</param>
        /// <param name="parameterSize">The size of the parameter.</param>
        /// <param name="sourceColumn">The source column.</param>
        public WMIParameter(DbType parameterType, int parameterSize, string sourceColumn)
            : this(null, parameterType, parameterSize, sourceColumn, DataRowVersion.Current)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="WMIParameter" /> class.
        /// </summary>
        /// <param name="parameterType">The data type.</param>
        /// <param name="parameterSize">The size of the parameter.</param>
        /// <param name="sourceColumn">The source column.</param>
        /// <param name="rowVersion">The row version information.</param>
        public WMIParameter(DbType parameterType, int parameterSize, string sourceColumn, DataRowVersion rowVersion)
            : this(null, parameterType, parameterSize, sourceColumn, rowVersion)
        {
        }

        /// <summary>
        ///     The command associated with this parameter.
        /// </summary>
        public IDbCommand Command { get; set; }

        /// <summary>
        ///     Gets or sets a value that indicates whether the parameter accepts null values.
        /// </summary>
        public override bool IsNullable { get; set; }

        /// <summary>
        ///     Gets or sets the <see cref="T:System.Data.DbType" /> of the parameter.
        /// </summary>
        [DbProviderSpecificTypeProperty(true)]
        [RefreshProperties(RefreshProperties.All)]
        public override DbType DbType
        {
            get
            {
                if (_dbType == UnknownDbType)
                {
                    if (_objValue != null && _objValue != DBNull.Value)
                        return _objValue.GetType().ToDbType();
                    return DbType.String; // Unassigned default value is String
                }

                return _dbType;
            }
            set => _dbType = value;
        }

        /// <summary>
        ///     Gets or sets a value that indicates whether the parameter is input-only, output-only, bidirectional, or a stored
        ///     procedure return value parameter.
        /// </summary>
        /// <exception cref="System.NotSupportedException"></exception>
        public override ParameterDirection Direction
        {
            get => ParameterDirection.Input;
            set
            {
                if (value != ParameterDirection.Input)
                    throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Gets or sets the name of the <see cref="T:System.Data.Common.DbParameter"/>.
        /// </summary>
        public override string ParameterName { get; set; }

        /// <summary>
        /// Gets or sets the maximum size, in bytes, of the data within the column.
        /// </summary>
        [DefaultValue(0)]
        public override int Size { get; set; }

        /// <summary>
        ///     Gets/sets the source column
        /// </summary>
        public override string SourceColumn { get; set; }

        /// <summary>
        ///     Used by DbCommandBuilder to determine the mapping for nullable fields
        /// </summary>
        public override bool SourceColumnNullMapping { get; set; }

        /// <summary>
        ///     Gets and sets the row version
        /// </summary>
        public override DataRowVersion SourceVersion { get; set; }

        /// <summary>
        ///     Gets and sets the parameter value. If no data type was specified, the data type will assume the type from the value
        ///     given.
        /// </summary>
        [TypeConverter(typeof(StringConverter))]
        [RefreshProperties(RefreshProperties.All)]
        public override object Value
        {
            get => _objValue;
            set
            {
                _objValue = value;

                // If the DbType has never been assigned, try to glean one from the value's data type
                if (_dbType == UnknownDbType && _objValue != null && _objValue != DBNull.Value)
                    _dbType = _objValue.GetType().ToDbType();
            }
        }

        /// <summary>
        ///     The database type name associated with this parameter, if any.
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        ///     Resets the DbType of the parameter so it can be inferred from the value
        /// </summary>
        public override void ResetDbType()
        {
            _dbType = UnknownDbType;
        }
    }
}