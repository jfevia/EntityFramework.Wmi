using System.ComponentModel;
using System.Data.Common;
using System.Globalization;

namespace System.Data.WMI
{
    public sealed class WMICommandBuilder : DbCommandBuilder
    {
        private bool _disposed;

        /// <summary>
        ///     Initializes a new instance of the <see cref="WMICommandBuilder" /> class.
        /// </summary>
        public WMICommandBuilder()
            : this(null)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="WMICommandBuilder" /> class.
        /// </summary>
        /// <param name="adapter">The adapter.</param>
        public WMICommandBuilder(WMIDataAdapter adapter)
        {
            QuotePrefix = "[";
            QuoteSuffix = "]";
            DataAdapter = adapter;
        }

        /// <summary>
        ///     Sets or gets the <see cref="T:System.Data.Common.CatalogLocation"></see> for an instance of the
        ///     <see cref="T:System.Data.Common.DbCommandBuilder"></see> class.
        /// </summary>
        [Browsable(false)]
        public override CatalogLocation CatalogLocation
        {
            get
            {
                CheckDisposed();
                return base.CatalogLocation;
            }
            set
            {
                CheckDisposed();
                base.CatalogLocation = value;
            }
        }

        /// <summary>
        ///     Sets or gets a string used as the catalog separator for an instance of the
        ///     <see cref="T:System.Data.Common.DbCommandBuilder"></see> class.
        /// </summary>
        [Browsable(false)]
        public override string CatalogSeparator
        {
            get
            {
                CheckDisposed();
                return base.CatalogSeparator;
            }
            set
            {
                CheckDisposed();
                base.CatalogSeparator = value;
            }
        }

        /// <summary>
        ///     Gets or sets the beginning character or characters to use when specifying database objects (for example, tables or
        ///     columns) whose names contain characters such as spaces or reserved tokens.
        /// </summary>
        [Browsable(false)]
        [DefaultValue("[")]
        public override string QuotePrefix
        {
            get
            {
                CheckDisposed();
                return base.QuotePrefix;
            }
            set
            {
                CheckDisposed();
                base.QuotePrefix = value;
            }
        }

        /// <summary>
        ///     Gets or sets the ending character or characters to use when specifying database objects (for example, tables or
        ///     columns) whose names contain characters such as spaces or reserved tokens.
        /// </summary>
        [Browsable(false)]
        public override string QuoteSuffix
        {
            get
            {
                CheckDisposed();
                return base.QuoteSuffix;
            }
            set
            {
                CheckDisposed();
                base.QuoteSuffix = value;
            }
        }

        /// <summary>
        ///     Gets or sets the character to be used for the separator between the schema identifier and any other identifiers.
        /// </summary>
        [Browsable(false)]
        public override string SchemaSeparator
        {
            get
            {
                CheckDisposed();
                return base.SchemaSeparator;
            }
            set
            {
                CheckDisposed();
                base.SchemaSeparator = value;
            }
        }

        private void CheckDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(typeof(WMICommandBuilder).Name);
        }

        /// <summary>
        ///     Releases the unmanaged resources used by the <see cref="T:System.Data.Common.DbCommandBuilder"></see> and
        ///     optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">
        ///     true to release both managed and unmanaged resources; false to release only unmanaged
        ///     resources.
        /// </param>
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (!_disposed)
                {
                    //if (disposing)
                    //{
                    //    ////////////////////////////////////
                    //    // dispose managed resources here...
                    //    ////////////////////////////////////
                    //}

                    //////////////////////////////////////
                    // release unmanaged resources here...
                    //////////////////////////////////////
                }
            }
            finally
            {
                base.Dispose(disposing);

                // Everything should be fully disposed at this point
                _disposed = true;
            }
        }

        /// <summary>
        ///     Allows the provider implementation of the <see cref="T:System.Data.Common.DbCommandBuilder"></see> class to handle
        ///     additional parameter properties.
        /// </summary>
        /// <param name="parameter">
        ///     A <see cref="T:System.Data.Common.DbParameter"></see> to which the additional modifications are
        ///     applied.
        /// </param>
        /// <param name="row">
        ///     The <see cref="T:System.Data.DataRow"></see> from the schema table provided by
        ///     <see cref="M:System.Data.Common.DbDataReader.GetSchemaTable"></see>.
        /// </param>
        /// <param name="statementType">The type of command being generated; INSERT, UPDATE or DELETE.</param>
        /// <param name="whereClause">
        ///     true if the parameter is part of the update or delete WHERE clause, false if it is part of
        ///     the insert or update values.
        /// </param>
        protected override void ApplyParameterInfo(DbParameter parameter, DataRow row, StatementType statementType, bool whereClause)
        {
            var param = (WMIParameter) parameter;
            param.DbType = (DbType) row[SchemaTableColumn.ProviderType];
        }

        /// <summary>
        ///     Returns the full parameter name, given the partial parameter name.
        /// </summary>
        /// <param name="parameterName">The partial name of the parameter.</param>
        /// <returns>
        ///     The full parameter name corresponding to the partial parameter name requested.
        /// </returns>
        protected override string GetParameterName(string parameterName)
        {
            return string.Format(CultureInfo.InvariantCulture, "@{0}", parameterName);
        }

        /// <summary>
        ///     Returns the name of the specified parameter. Use when building a custom command builder.
        /// </summary>
        /// <param name="parameterOrdinal">The number to be included as part of the parameter's name..</param>
        /// <returns>
        ///     The name of the parameter with the specified number appended as part of the parameter name.
        /// </returns>
        protected override string GetParameterName(int parameterOrdinal)
        {
            return string.Format(CultureInfo.InvariantCulture, "@param{0}", parameterOrdinal);
        }

        /// <summary>
        ///     Returns the placeholder for the parameter in the associated SQL statement.
        /// </summary>
        /// <param name="parameterOrdinal">The number to be included as part of the parameter's name.</param>
        /// <returns>
        ///     The name of the parameter with the specified number appended.
        /// </returns>
        protected override string GetParameterPlaceholder(int parameterOrdinal)
        {
            return GetParameterName(parameterOrdinal);
        }

        /// <summary>
        ///     Registers the <see cref="T:System.Data.Common.DbCommandBuilder"></see> to handle the
        ///     <see cref="E:System.Data.OleDb.OleDbDataAdapter.RowUpdating"></see> event for a
        ///     <see cref="T:System.Data.Common.DbDataAdapter"></see>.
        /// </summary>
        /// <param name="adapter">The <see cref="T:System.Data.Common.DbDataAdapter"></see> to be used for the update.</param>
        protected override void SetRowUpdatingHandler(DbDataAdapter adapter)
        {
            if (adapter == DataAdapter)
                ((WMIDataAdapter) adapter).RowUpdating -= WMICommandBuilder_RowUpdating;
            else
                ((WMIDataAdapter) adapter).RowUpdating += WMICommandBuilder_RowUpdating;
        }

        /// <summary>
        ///     Handles the RowUpdating event of the WMICommandBuilder control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RowUpdatingEventArgs" /> instance containing the event data.</param>
        private void WMICommandBuilder_RowUpdating(object sender, RowUpdatingEventArgs e)
        {
            RowUpdatingHandler(e);
        }

        /// <summary>
        ///     Given an unquoted identifier in the correct catalog case, returns the correct quoted form of that identifier,
        ///     including properly escaping any embedded quotes in the identifier.
        /// </summary>
        /// <param name="unquotedIdentifier">The original unquoted identifier.</param>
        /// <returns>
        ///     The quoted version of the identifier. Embedded quotes within the identifier are properly escaped.
        /// </returns>
        public override string QuoteIdentifier(string unquotedIdentifier)
        {
            CheckDisposed();

            if (string.IsNullOrEmpty(QuotePrefix)
                || string.IsNullOrEmpty(QuoteSuffix)
                || string.IsNullOrEmpty(unquotedIdentifier))
                return unquotedIdentifier;

            return $"{QuotePrefix}{unquotedIdentifier.Replace(QuoteSuffix, $"{QuoteSuffix}{QuoteSuffix}")}{QuoteSuffix}";
        }

        /// <summary>
        ///     Given a quoted identifier, returns the correct unquoted form of that identifier, including properly un-escaping any
        ///     embedded quotes in the identifier.
        /// </summary>
        /// <param name="quotedIdentifier">The identifier that will have its embedded quotes removed.</param>
        /// <returns>
        ///     The unquoted identifier, with embedded quotes properly un-escaped.
        /// </returns>
        public override string UnquoteIdentifier(string quotedIdentifier)
        {
            CheckDisposed();

            if (string.IsNullOrEmpty(QuotePrefix)
                || string.IsNullOrEmpty(QuoteSuffix)
                || string.IsNullOrEmpty(quotedIdentifier))
                return quotedIdentifier;

            if (quotedIdentifier.StartsWith(QuotePrefix, StringComparison.OrdinalIgnoreCase) == false
                || quotedIdentifier.EndsWith(QuoteSuffix, StringComparison.OrdinalIgnoreCase) == false)
                return quotedIdentifier;

            return quotedIdentifier.Substring(QuotePrefix.Length, quotedIdentifier.Length - (QuotePrefix.Length + QuoteSuffix.Length)).Replace($"{QuoteSuffix}{QuoteSuffix}", QuoteSuffix);
        }

        /// <summary>
        ///     Returns the schema table for the <see cref="T:System.Data.Common.DbCommandBuilder"></see>.
        /// </summary>
        /// <param name="sourceCommand">
        ///     The <see cref="T:System.Data.Common.DbCommand"></see> for which to retrieve the
        ///     corresponding schema table.
        /// </param>
        /// <returns>
        ///     A <see cref="T:System.Data.DataTable"></see> that represents the schema for the specific
        ///     <see cref="T:System.Data.Common.DbCommand"></see>.
        /// </returns>
        protected override DataTable GetSchemaTable(DbCommand sourceCommand)
        {
            using (IDataReader reader = sourceCommand.ExecuteReader(CommandBehavior.KeyInfo | CommandBehavior.SchemaOnly))
            {
                var schema = reader.GetSchemaTable();

                // If the query contains a primary key, turn off the IsUnique property
                // for all the non-key columns
                if (HasSchemaPrimaryKey(schema))
                    ResetIsUniqueSchemaColumn(schema);

                // if table has no primary key we use unique columns as a fall back
                return schema;
            }
        }

        private bool HasSchemaPrimaryKey(DataTable schema)
        {
            var isKeyColumn = schema.Columns[SchemaTableColumn.IsKey];

            foreach (DataRow schemaRow in schema.Rows)
                if ((bool) schemaRow[isKeyColumn])
                    return true;

            return false;
        }

        private void ResetIsUniqueSchemaColumn(DataTable schema)
        {
            var isUniqueColumn = schema.Columns[SchemaTableColumn.IsUnique];
            var isKeyColumn = schema.Columns[SchemaTableColumn.IsKey];

            foreach (DataRow schemaRow in schema.Rows)
                if ((bool) schemaRow[isKeyColumn] == false)
                    schemaRow[isUniqueColumn] = false;

            schema.AcceptChanges();
        }
    }
}