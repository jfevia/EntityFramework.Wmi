using System.Collections.Generic;
using System.Data.Common;

namespace System.Data.WMI
{
    public sealed class WMICommand : DbCommand, ICloneable
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="WMICommand" /> class.
        /// </summary>
        /// <param name="connection">The connection.</param>
        public WMICommand(WMIConnection connection)
            : this(null, connection)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="WMICommand" /> class.
        /// </summary>
        public WMICommand()
            : this(null, null)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="WMICommand" /> class.
        /// </summary>
        /// <param name="commandText">The command text.</param>
        public WMICommand(string commandText)
            : this(commandText, null)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="WMICommand" /> class.
        /// </summary>
        /// <param name="commandText">The command text.</param>
        /// <param name="connection">The connection.</param>
        public WMICommand(string commandText, WMIConnection connection)
        {
            CommandText = commandText;
            DbConnection = connection;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="WMICommand" /> class.
        /// </summary>
        /// <param name="source">The source.</param>
        private WMICommand(WMICommand source)
            : this(source.CommandText, (WMIConnection) source.Connection)
        {
            CommandTimeout = source.CommandTimeout;
            DesignTimeVisible = source.DesignTimeVisible;
            UpdatedRowSource = source.UpdatedRowSource;

            if (source.InternalParameters != null)
                foreach (WMIParameter param in source.InternalParameters)
                    Parameters.Add(param.Clone());

            if (source.Columns != null)
                Columns = new List<string>(source.Columns);
        }

        /// <summary>
        ///     Gets the internal parameters.
        /// </summary>
        /// <value>
        ///     The internal parameters.
        /// </value>
        internal DbParameterCollection InternalParameters => Parameters;

        /// <summary>
        ///     Gets or sets the text command to run against the data source.
        /// </summary>
        public override string CommandText { get; set; }

        /// <summary>
        ///     Gets or sets the wait time before terminating the attempt to execute a command and generating an error.
        /// </summary>
        public override int CommandTimeout { get; set; }

        /// <summary>
        ///     Indicates or specifies how the <see cref="P:System.Data.Common.DbCommand.CommandText" /> property is interpreted.
        /// </summary>
        public override CommandType CommandType { get; set; }

        /// <summary>
        ///     Gets or sets how command results are applied to the <see cref="T:System.Data.DataRow" /> when used by the Update
        ///     method of a <see cref="T:System.Data.Common.DbDataAdapter" />.
        /// </summary>
        public override UpdateRowSource UpdatedRowSource { get; set; }

        /// <summary>
        ///     Gets or sets the <see cref="T:System.Data.Common.DbConnection" /> used by this
        ///     <see cref="T:System.Data.Common.DbCommand" />.
        /// </summary>
        protected override DbConnection DbConnection { get; set; }

        /// <summary>
        ///     Gets the collection of <see cref="T:System.Data.Common.DbParameter" /> objects.
        /// </summary>
        protected override DbParameterCollection DbParameterCollection { get; }

        /// <summary>
        ///     Gets or sets the <see cref="P:System.Data.Common.DbCommand.DbTransaction" /> within which this
        ///     <see cref="T:System.Data.Common.DbCommand" /> object executes.
        /// </summary>
        protected override DbTransaction DbTransaction { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether the command object should be visible in a customized interface control.
        /// </summary>
        public override bool DesignTimeVisible { get; set; }

        /// <summary>
        ///     Gets or sets the columns.
        /// </summary>
        /// <value>
        ///     The columns.
        /// </value>
        public IList<string> Columns { get; set; }

        /// <summary>
        ///     Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        ///     A new object that is a copy of this instance.
        /// </returns>
        public object Clone()
        {
            return new WMICommand(this);
        }

        public override void Prepare()
        {
            throw new NotImplementedException();
        }

        public override void Cancel()
        {
            throw new NotImplementedException();
        }

        protected override DbParameter CreateDbParameter()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Executes the command text against the connection.
        /// </summary>
        /// <param name="behavior">
        ///     An instance of <see cref="T:System.Data.CommandBehavior" />.
        /// </param>
        /// <returns>
        ///     A task representing the operation.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///     DbConnection
        /// </exception>
        /// <exception cref="System.InvalidOperationException">
        ///     The connection is not open
        /// </exception>
        protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior)
        {
            if (DbConnection == null)
                throw new ArgumentNullException(nameof(DbConnection));

            if (DbConnection.State != ConnectionState.Open)
                throw new InvalidOperationException("The connection is not open");

            return new WMIDataReader(this, behavior);
        }

        /// <summary>
        ///     Executes a SQL statement against a connection object.
        /// </summary>
        /// <returns>
        ///     The number of rows affected.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///     DbConnection
        /// </exception>
        /// <exception cref="System.InvalidOperationException">
        ///     The connection is not open
        /// </exception>
        public override int ExecuteNonQuery()
        {
            return ExecuteNonQuery(CommandBehavior.Default);
        }

        /// <summary>
        ///     Executes a SQL statement against a connection object with the specified behavior.
        /// </summary>
        /// <param name="behavior">The behavior.</param>
        /// <returns>
        ///     The number of rows affected.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">DbConnection</exception>
        /// <exception cref="System.InvalidOperationException">The connection is not open</exception>
        public int ExecuteNonQuery(CommandBehavior behavior)
        {
            using (var reader = ExecuteReader(behavior))
            {
                while (reader.NextResult())
                {
                    // Nothing
                }

                return reader.RecordsAffected;
            }
        }

        /// <summary>
        ///     Executes the query and returns the first column of the first row in the result set returned by the query. All other
        ///     columns and rows are ignored.
        /// </summary>
        /// <returns>
        ///     The first column of the first row in the result set.
        /// </returns>
        public override object ExecuteScalar()
        {
            return ExecuteScalar(CommandBehavior.Default);
        }

        /// <summary>
        ///     Executes the query with the specified behavior and returns the first column of the first row in the result set
        ///     returned by the query. All other columns and rows are ignored.
        /// </summary>
        /// <param name="behavior">The behavior.</param>
        /// <returns>
        ///     The first column of the first row in the result set.
        /// </returns>
        public object ExecuteScalar(CommandBehavior behavior)
        {
            using (var reader = ExecuteReader(behavior))
            {
                if (reader.Read() && reader.FieldCount > 0)
                    return reader[0];
            }

            return null;
        }
    }
}