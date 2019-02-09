using System.Data.Common;
using System.Management;

namespace System.Data.WMI
{
    public class WMITransactionStartedEventArgs : EventArgs
    {
    }

    public class WMIConnectionChangedEventArgs : EventArgs
    {
    }

    public sealed class WMIConnection : DbConnection
    {
        private ConnectionState _connectionState;
        private WMIConnectionStringBuilder _connectionStringBuilder;

        private ManagementScope _managementScope;

        /// <summary>
        ///     Initializes a new instance of the <see cref="WMIConnection" /> class.
        /// </summary>
        public WMIConnection()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="WMIConnection" /> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public WMIConnection(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public event EventHandler<WMIConnectionChangedEventArgs> Changed;
        public event EventHandler<WMITransactionStartedEventArgs> TransactionStarted;

        /// <summary>
        ///     Gets or sets the string used to open the connection.
        /// </summary>
        /// <exception cref="System.ArgumentException"></exception>
        /// <exception cref="System.InvalidOperationException"></exception>
        public override string ConnectionString
        {
            get => _connectionStringBuilder?.ConnectionString;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException($"{nameof(ConnectionString)} cannot be null", nameof(value));

                if (State != ConnectionState.Closed)
                    throw new InvalidOperationException($"Cannot change {nameof(ConnectionString)} because its state is {_connectionState}");

                _connectionStringBuilder = new WMIConnectionStringBuilder(value);
            }
        }

        public override string Database => _connectionStringBuilder.Namespace;

        /// <summary>
        ///     Gets a string that describes the state of the connection.
        /// </summary>
        public override ConnectionState State => _connectionState;

        /// <summary>
        ///     Gets the name of the database server to which to connect.
        /// </summary>
        public override string DataSource => _connectionStringBuilder.Computer;

        /// <summary>
        ///     Gets a string that represents the version of the server to which the object is connected.
        /// </summary>
        /// <exception cref="System.NotSupportedException"></exception>
        public override string ServerVersion => throw new NotSupportedException();

        /// <summary>
        ///     Starts a database transaction.
        /// </summary>
        /// <param name="isolationLevel">Specifies the isolation level for the transaction.</param>
        /// <returns>
        ///     An object representing the new transaction.
        /// </returns>
        /// <exception cref="System.NotSupportedException"></exception>
        protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel)
        {
            TransactionStarted?.Invoke(this, new WMITransactionStartedEventArgs());
            throw new NotSupportedException();
        }

        /// <summary>
        ///     Changes the current database for an open connection.
        /// </summary>
        /// <param name="databaseName">Specifies the name of the database for the connection to use.</param>
        /// <exception cref="System.NotSupportedException"></exception>
        public override void ChangeDatabase(string databaseName)
        {
            Changed?.Invoke(this, new WMIConnectionChangedEventArgs());
            throw new NotSupportedException();
        }

        /// <summary>
        ///     Closes the connection to the database. This is the preferred method of closing any open connection.
        /// </summary>
        public override void Close()
        {
            _managementScope = null;
            _connectionState = ConnectionState.Closed;
        }

        /// <summary>
        ///     Opens a database connection with the settings specified by the
        ///     <see cref="P:System.Data.Common.DbConnection.ConnectionString" />.
        /// </summary>
        public override void Open()
        {
            _managementScope.Connect();
            _connectionState = ConnectionState.Open;
        }

        /// <summary>
        ///     Creates and returns a <see cref="T:System.Data.Common.DbCommand" /> object associated with the current connection.
        /// </summary>
        /// <returns>
        ///     A <see cref="T:System.Data.Common.DbCommand" /> object.
        /// </returns>
        protected override DbCommand CreateDbCommand()
        {
            return new WMICommand(this);
        }
    }
}