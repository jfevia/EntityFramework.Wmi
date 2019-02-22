using System.Data.Common;
using System.Data.Entity.Core.Common;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.WMI.EF6.Gen;
using System.Diagnostics;

namespace System.Data.WMI.EF6
{
    internal sealed class WMIProviderServices : DbProviderServices
    {
        internal static WMIProviderServices Instance = new WMIProviderServices();

        /// <summary>
        ///     Creates the database command definition.
        /// </summary>
        /// <param name="manifest">The manifest.</param>
        /// <param name="commandTree">The command tree.</param>
        /// <returns>The database command definition.</returns>
        protected override DbCommandDefinition CreateDbCommandDefinition(DbProviderManifest manifest, DbCommandTree commandTree)
        {
            var prototype = CreateCommand(manifest, commandTree);
            var result = CreateCommandDefinition(prototype);
            return result;
        }

        private DbCommand CreateCommand(DbProviderManifest manifest, DbCommandTree commandTree)
        {
            if (manifest == null)
                throw new ArgumentNullException(nameof(manifest));

            if (commandTree == null)
                throw new ArgumentNullException(nameof(commandTree));

            var command = new WMICommand();
            try
            {
                command.CommandText = SqlGenerator.GenerateSql((WMIProviderManifest) manifest, commandTree, out var columns, out var parameters, out var commandType);
                command.Columns = columns;
                command.CommandType = commandType;

                // Get the function (if any) implemented by the command tree since this influences our interpretation of parameters
                EdmFunction function = null;
                if (commandTree is DbFunctionCommandTree tree)
                    function = tree.EdmFunction;

                // Now make sure we populate the command's parameters from the CQT's parameters:
                foreach (var queryParameter in commandTree.Parameters)
                {
                    WMIParameter parameter;

                    // Use the corresponding function parameter TypeUsage where available (currently, the SSDL facets and
                    // type trump user-defined facets and type in the EntityCommand).
                    if (null != function && function.Parameters.TryGetValue(queryParameter.Key, false, out var functionParameter))
                        parameter = CreateSqlParameter((WMIProviderManifest) manifest, functionParameter.Name, functionParameter.TypeUsage, functionParameter.Mode, DBNull.Value);
                    else
                        parameter = CreateSqlParameter((WMIProviderManifest) manifest, queryParameter.Key, queryParameter.Value, ParameterMode.In, DBNull.Value);

                    command.Parameters.Add(parameter);
                }

                // Now add parameters added as part of SQL gen (note: this feature is only safe for DML SQL gen which
                // does not support user parameters, where there is no risk of name collision)
                if (null != parameters && 0 < parameters.Count)
                {
                    if (!(commandTree is DbInsertCommandTree) &&
                        !(commandTree is DbUpdateCommandTree) &&
                        !(commandTree is DbDeleteCommandTree))
                        throw new InvalidOperationException("SqlGenParametersNotPermitted");

                    foreach (var parameter in parameters) command.Parameters.Add(parameter);
                }

                return command;
            }
            catch
            {
                command.Dispose();
                throw;
            }
        }

        /// <summary>
        ///     Returns provider manifest token for a given connection.
        /// </summary>
        /// <param name="connection">Connection to find manifest token from.</param>
        /// <returns>
        ///     The provider manifest token for the specified connection.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     connection
        ///     or
        ///     ConnectionString
        /// </exception>
        protected override string GetDbProviderManifestToken(DbConnection connection)
        {
            if (connection == null)
                throw new ArgumentNullException(nameof(connection));

            if (string.IsNullOrEmpty(connection.ConnectionString))
                throw new ArgumentNullException(nameof(connection.ConnectionString));

            return connection.ConnectionString;
        }

        /// <summary>
        ///     Gets the database provider manifest.
        /// </summary>
        /// <param name="versionHint">The version hint.</param>
        /// <returns>The database provider manifest.</returns>
        protected override DbProviderManifest GetDbProviderManifest(string versionHint)
        {
            return new WMIProviderManifest();
        }

        /// <summary>
        ///     Creates a WMIParameter given a name, type, and direction
        /// </summary>
        internal static WMIParameter CreateSqlParameter(WMIProviderManifest manifest, string name, TypeUsage type, ParameterMode mode, object value)
        {
            if (MetadataHelpers.GetPrimitiveTypeKind(type) == PrimitiveTypeKind.Guid)
                type = TypeUsage.CreateStringTypeUsage(
                    PrimitiveType.GetEdmPrimitiveType(PrimitiveTypeKind.String),
                    false, true);

            var result = new WMIParameter(name, value);

            // .Direction
            var direction = MetadataHelpers.ParameterModeToParameterDirection(mode);
            if (result.Direction != direction)
                result.Direction = direction;

            // .Size and .DbType
            // output parameters are handled differently (we need to ensure there is space for return
            // values where the user has not given a specific Size/MaxLength)
            var isOutParam = mode != ParameterMode.In;
            var sqlDbType = GetSqlDbType(type, isOutParam, out var size);
            if (result.DbType != sqlDbType) result.DbType = sqlDbType;

            // Note that we overwrite 'facet' parameters where either the value is different or
            // there is an output parameter.
            if (size.HasValue && (isOutParam || result.Size != size.Value))
                result.Size = size.Value;

            // .IsNullable
            var isNullable = MetadataHelpers.IsNullable(type);
            if (isOutParam || isNullable != result.IsNullable)
                result.IsNullable = isNullable;

            return result;
        }

        /// <summary>
        ///     Determines DbType for the given primitive type. Extracts facet
        ///     information as well.
        /// </summary>
        private static DbType GetSqlDbType(TypeUsage type, bool isOutParam, out int? size)
        {
            // only supported for primitive type
            var primitiveTypeKind = MetadataHelpers.GetPrimitiveTypeKind(type);

            size = default(int?);

            switch (primitiveTypeKind)
            {
                case PrimitiveTypeKind.Binary:
                    // for output parameters, ensure there is space...
                    size = GetParameterSize(type, isOutParam);
                    return GetBinaryDbType(type);

                case PrimitiveTypeKind.Boolean:
                    return DbType.Boolean;

                case PrimitiveTypeKind.Byte:
                    return DbType.Byte;

                case PrimitiveTypeKind.Time:
                    return DbType.Time;

                case PrimitiveTypeKind.DateTimeOffset:
                    return DbType.DateTimeOffset;

                case PrimitiveTypeKind.DateTime:
                    return DbType.DateTime;

                case PrimitiveTypeKind.Decimal:
                    return DbType.Decimal;

                case PrimitiveTypeKind.Double:
                    return DbType.Double;

                case PrimitiveTypeKind.Guid:
                    return DbType.Guid;

                case PrimitiveTypeKind.Int16:
                    return DbType.Int16;

                case PrimitiveTypeKind.Int32:
                    return DbType.Int32;

                case PrimitiveTypeKind.Int64:
                    return DbType.Int64;

                case PrimitiveTypeKind.SByte:
                    return DbType.SByte;

                case PrimitiveTypeKind.Single:
                    return DbType.Single;

                case PrimitiveTypeKind.String:
                    size = GetParameterSize(type, isOutParam);
                    return GetStringDbType(type);

                default:
                    Debug.Fail("Unknown PrimitiveTypeKind " + primitiveTypeKind);
                    return DbType.Object;
            }
        }

        /// <summary>
        ///     Determines preferred value for SqlParameter.Size. Returns null
        ///     where there is no preference.
        /// </summary>
        private static int? GetParameterSize(TypeUsage type, bool isOutParam)
        {
            if (MetadataHelpers.TryGetMaxLength(type, out var maxLength))
                return maxLength;
            if (isOutParam)
                return int.MaxValue;
            return default(int?);
        }

        /// <summary>
        ///     Chooses the appropriate DbType for the given string type.
        /// </summary>
        private static DbType GetStringDbType(TypeUsage type)
        {
            Debug.Assert(type.EdmType.BuiltInTypeKind == BuiltInTypeKind.PrimitiveType &&
                         PrimitiveTypeKind.String == ((PrimitiveType) type.EdmType).PrimitiveTypeKind, "only valid for string type");

            DbType dbType;
            if (!MetadataHelpers.TryGetIsFixedLength(type, out var fixedLength))
                fixedLength = false;

            if (!MetadataHelpers.TryGetIsUnicode(type, out var unicode))
                unicode = true;

            if (fixedLength)
                dbType = unicode ? DbType.StringFixedLength : DbType.AnsiStringFixedLength;
            else
                dbType = unicode ? DbType.String : DbType.AnsiString;
            return dbType;
        }

        /// <summary>
        ///     Chooses the appropriate DbType for the given binary type.
        /// </summary>
        private static DbType GetBinaryDbType(TypeUsage type)
        {
            Debug.Assert(type.EdmType.BuiltInTypeKind == BuiltInTypeKind.PrimitiveType &&
                         PrimitiveTypeKind.Binary == ((PrimitiveType) type.EdmType).PrimitiveTypeKind, "only valid for binary type");

            return DbType.Binary;
        }
    }
}