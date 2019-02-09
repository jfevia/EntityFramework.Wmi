using System.ComponentModel;
using System.Data.Common;
using System.Linq;

namespace System.Data.WMI
{
    /// <summary>
    ///     WMI implementation of DbDataAdapter.
    /// </summary>
    [DefaultEvent("RowUpdated")]
    [ToolboxItem("WMI.Designer.WMIDataAdapterToolboxItem, WMI.Designer, Version=" + WMIDesigner.Version + ", Culture=neutral, PublicKeyToken=db937bc2d44ff139")]
    [Designer("Microsoft.VSDesigner.Data.VS.SqlDataAdapterDesigner, Microsoft.VSDesigner, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
    public sealed class WMIDataAdapter : DbDataAdapter
    {
        private static readonly object UpdatingEventPh = new object();
        private static readonly object UpdatedEventPh = new object();

        private bool _disposed;
        private readonly bool _disposeSelect = true;

        /// <overloads>
        ///     This class is just a shell around the DbDataAdapter.  Nothing from
        ///     DbDataAdapter is overridden here, just a few constructors are defined.
        /// </overloads>
        /// <summary>
        ///     Default constructor.
        /// </summary>
        public WMIDataAdapter()
        {
        }

        /// <summary>
        ///     Constructs a data adapter using the specified select command.
        /// </summary>
        /// <param name="cmd">
        ///     The select command to associate with the adapter.
        /// </param>
        public WMIDataAdapter(WMICommand cmd)
        {
            SelectCommand = cmd;
            _disposeSelect = false;
        }

        /// <summary>
        ///     Constructs a data adapter with the supplied select command text and
        ///     associated with the specified connection.
        /// </summary>
        /// <param name="commandText">
        ///     The select command text to associate with the data adapter.
        /// </param>
        /// <param name="connection">
        ///     The connection to associate with the select command.
        /// </param>
        public WMIDataAdapter(string commandText, WMIConnection connection)
        {
            SelectCommand = new WMICommand(commandText, connection);
        }

        /// <summary>
        ///     Constructs a data adapter with the specified select command text,
        ///     and using the specified database connection string.
        /// </summary>
        /// <param name="commandText">
        ///     The select command text to use to construct a select command.
        /// </param>
        /// <param name="connectionString">
        ///     A connection string suitable for passing to a new WMIConnection,
        ///     which is associated with the select command.
        /// </param>
        public WMIDataAdapter(string commandText, string connectionString)
        {
            var cnn = new WMIConnection(connectionString);

            SelectCommand = new WMICommand(commandText, cnn);
        }

        /// <summary>
        ///     Row updated event handler
        /// </summary>
        public event EventHandler<RowUpdatedEventArgs> RowUpdated
        {
            add
            {
                CheckDisposed();
                Events.AddHandler(UpdatedEventPh, value);
            }
            remove
            {
                CheckDisposed();
                Events.RemoveHandler(UpdatedEventPh, value);
            }
        }

        /// <summary>
        ///     Row updating event handler
        /// </summary>
        public event EventHandler<RowUpdatingEventArgs> RowUpdating
        {
            add
            {
                CheckDisposed();

                var previous = (EventHandler<RowUpdatingEventArgs>) Events[UpdatingEventPh];
                if (previous != null && value.Target is DbCommandBuilder)
                {
                    var handler = (EventHandler<RowUpdatingEventArgs>) FindBuilder(previous);
                    if (handler != null) Events.RemoveHandler(UpdatingEventPh, handler);
                }

                Events.AddHandler(UpdatingEventPh, value);
            }
            remove
            {
                CheckDisposed();
                Events.RemoveHandler(UpdatingEventPh, value);
            }
        }

        /// <summary>
        ///     Gets/sets the select command for this DataAdapter
        /// </summary>
        [DefaultValue(null)]
        [Editor("Microsoft.VSDesigner.Data.Design.DBCommandEditor, Microsoft.VSDesigner, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
        public new WMICommand SelectCommand
        {
            get
            {
                CheckDisposed();
                return (WMICommand) base.SelectCommand;
            }
            set
            {
                CheckDisposed();
                base.SelectCommand = value;
            }
        }

        /// <summary>
        ///     Gets/sets the insert command for this DataAdapter
        /// </summary>
        [DefaultValue(null)]
        [Editor("Microsoft.VSDesigner.Data.Design.DBCommandEditor, Microsoft.VSDesigner, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
        public new WMICommand InsertCommand
        {
            get
            {
                CheckDisposed();
                return (WMICommand) base.InsertCommand;
            }
            set
            {
                CheckDisposed();
                base.InsertCommand = value;
            }
        }

        /// <summary>
        ///     Gets/sets the update command for this DataAdapter
        /// </summary>
        [DefaultValue(null)]
        [Editor("Microsoft.VSDesigner.Data.Design.DBCommandEditor, Microsoft.VSDesigner, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
        public new WMICommand UpdateCommand
        {
            get
            {
                CheckDisposed();
                return (WMICommand) base.UpdateCommand;
            }
            set
            {
                CheckDisposed();
                base.UpdateCommand = value;
            }
        }

        /// <summary>
        ///     Gets/sets the delete command for this DataAdapter
        /// </summary>
        [DefaultValue(null)]
        [Editor("Microsoft.VSDesigner.Data.Design.DBCommandEditor, Microsoft.VSDesigner, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
        public new WMICommand DeleteCommand
        {
            get
            {
                CheckDisposed();
                return (WMICommand) base.DeleteCommand;
            }
            set
            {
                CheckDisposed();
                base.DeleteCommand = value;
            }
        }

        private void CheckDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(typeof(WMIDataAdapter).Name);
        }

        /// <summary>
        ///     Cleans up resources (native and managed) associated with the current instance.
        /// </summary>
        /// <param name="disposing">
        ///     Zero when being disposed via garbage collection; otherwise, non-zero.
        /// </param>
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (_disposed)
                    return;

                if (!disposing)
                    return;

                // Dispose managed resources here
                if (_disposeSelect && SelectCommand != null)
                {
                    SelectCommand.Dispose();
                    SelectCommand = null;
                }

                if (InsertCommand != null)
                {
                    InsertCommand.Dispose();
                    InsertCommand = null;
                }

                if (UpdateCommand != null)
                {
                    UpdateCommand.Dispose();
                    UpdateCommand = null;
                }

                if (DeleteCommand != null)
                {
                    DeleteCommand.Dispose();
                    DeleteCommand = null;
                }
            }
            finally
            {
                base.Dispose(disposing);

                // Everything should be fully disposed at this point
                _disposed = true;
            }
        }

        internal static Delegate FindBuilder(MulticastDelegate mcd)
        {
            if (mcd == null)
                return null;

            var invocationList = mcd.GetInvocationList();
            return invocationList.FirstOrDefault(s => s.Target is DbCommandBuilder);
        }

        /// <summary>
        ///     Raised by the underlying DbDataAdapter when a row is being updated
        /// </summary>
        /// <param name="value">The event's specifics</param>
        protected override void OnRowUpdating(RowUpdatingEventArgs value)
        {
            if (Events[UpdatingEventPh] is EventHandler<RowUpdatingEventArgs> handler)
                handler(this, value);
        }

        /// <summary>
        ///     Raised by DbDataAdapter after a row is updated
        /// </summary>
        /// <param name="value">The event's specifics</param>
        protected override void OnRowUpdated(RowUpdatedEventArgs value)
        {
            if (Events[UpdatedEventPh] is EventHandler<RowUpdatedEventArgs> handler)
                handler(this, value);
        }
    }
}