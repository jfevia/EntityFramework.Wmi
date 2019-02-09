using System.Data.Common;
using System.Data.Entity.Core.Common;

namespace System.Data.WMI.EF6
{
    /// <summary>
    ///     WMI implementation of <see cref="DbProviderFactory" />.
    /// </summary>
    public sealed class WMIProviderFactory : DbProviderFactory, IServiceProvider, IDisposable
    {
        private bool _disposed;

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     Gets the service object of the specified type.
        /// </summary>
        /// <param name="serviceType">
        ///     An object that specifies the type of service object to get.
        /// </param>
        /// <returns>
        ///     A service object of type serviceType -OR- a null reference if
        ///     there is no service object of type serviceType.
        /// </returns>
        public object GetService(Type serviceType)
        {
            if (serviceType == typeof(DbProviderServices))
                return WMIProviderServices.Instance;

            return null;
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
        ///     Creates and returns a new <see cref="WMIConnectionStringBuilder" />
        ///     object.
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

        private void CheckDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(
                    typeof(WMIProviderFactory).Name);
            }
        }

        /// <summary>
        ///     Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing">
        ///     <span class="keyword">
        ///         <span class="languageSpecificText">
        ///             <span class="cs">true</span>
        ///             <span class="vb">True</span>
        ///             <span class="cpp">true</span>
        ///         </span>
        ///     </span>
        ///     <span class="nu">
        ///         <span class="keyword">true</span> (<span class="keyword">True</span> in Visual Basic)
        ///     </span>
        ///     to release both managed and unmanaged resources;
        ///     <span class="keyword">
        ///         <span class="languageSpecificText">
        ///             <span class="cs">false</span><span class="vb">False</span><span class="cpp">false</span>
        ///         </span>
        ///     </span>
        ///     <span class="nu"><span class="keyword">false</span> (<span class="keyword">False</span> in Visual Basic)</span> to
        ///     release only unmanaged resources.
        /// </param>
        private void Dispose(bool disposing)
        {
            if (!_disposed)
                _disposed = true;
        }

        /// <summary>
        ///     Finalizes an instance of the <see cref="WMIProviderFactory" /> class.
        /// </summary>
        ~WMIProviderFactory()
        {
            Dispose(false);
        }
    }
}