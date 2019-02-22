using System.Data.Common;

namespace System.Data.WMI
{
    /// <summary>
    ///     WMI implementation of <see cref="DbProviderFactory" />.
    /// </summary>
    public sealed class WMIFactory : DbProviderFactory, IDisposable
    {
        /// <summary>
        ///     Static instance member which returns an instanced <see cref="WMIFactory" /> class.
        /// </summary>
        public static readonly WMIFactory Instance = new WMIFactory();

        private bool _disposed;

        /// <summary>
        ///     Cleans up resources (native and managed) associated with the current instance.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void CheckDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(typeof(WMIFactory).Name);
        }

        /// <summary>
        ///     Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing">
        ///     <see langword="true" /> to release both managed and unmanaged resources;
        ///     <see langword="false" /> to release only unmanaged resources.
        /// </param>
        private void Dispose(bool disposing)
        {
            if (!_disposed)
                _disposed = true;
        }

        /// <summary>
        ///     Cleans up resources associated with the current instance.
        /// </summary>
        ~WMIFactory()
        {
            Dispose(false);
        }

        /// <summary>
        ///     Creates and returns a new <see cref="WMICommand" /> object.
        /// </summary>
        /// <returns>The new object.</returns>
        public override DbCommand CreateCommand()
        {
            CheckDisposed();
            return new WMICommand();
        }

        /// <summary>
        ///     Creates and returns a new <see cref="WMICommandBuilder" /> object.
        /// </summary>
        /// <returns>The new object.</returns>
        public override DbCommandBuilder CreateCommandBuilder()
        {
            CheckDisposed();
            return new WMICommandBuilder();
        }

        /// <summary>
        ///     Creates and returns a new <see cref="WMIConnection" /> object.
        /// </summary>
        /// <returns>The new object.</returns>
        public override DbConnection CreateConnection()
        {
            CheckDisposed();
            return new WMIConnection();
        }

        /// <summary>
        ///     Creates and returns a new <see cref="WMIConnectionStringBuilder" /> object.
        /// </summary>
        /// <returns>The new object.</returns>
        public override DbConnectionStringBuilder CreateConnectionStringBuilder()
        {
            CheckDisposed();
            return new WMIConnectionStringBuilder();
        }

        /// <summary>
        ///     Creates and returns a new <see cref="WMIDataAdapter" /> object.
        /// </summary>
        /// <returns>The new object.</returns>
        public override DbDataAdapter CreateDataAdapter()
        {
            CheckDisposed();
            return new WMIDataAdapter();
        }

        /// <summary>
        ///     Creates and returns a new <see cref="WMIParameter" /> object.
        /// </summary>
        /// <returns>The new object.</returns>
        public override DbParameter CreateParameter()
        {
            CheckDisposed();
            return new WMIParameter();
        }
    }
}