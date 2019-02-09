using System.Collections.Generic;

namespace System.Data.WMI
{
    internal static class TypeExtensions
    {
        private static readonly Dictionary<Type, DbType> DbTypeByTypeCode = new Dictionary<Type, DbType>
        {
            {typeof(object), DbType.Binary},
            {typeof(DBNull), DbType.Object},
            {typeof(bool), DbType.Boolean},
            {typeof(char), DbType.SByte},
            {typeof(sbyte), DbType.SByte},
            {typeof(byte), DbType.Byte},
            {typeof(short), DbType.Int16},
            {typeof(ushort), DbType.UInt16},
            {typeof(int), DbType.Int32},
            {typeof(uint), DbType.UInt32},
            {typeof(long), DbType.Int64},
            {typeof(ulong), DbType.UInt64},
            {typeof(float), DbType.Single},
            {typeof(double), DbType.Double},
            {typeof(decimal), DbType.Decimal},
            {typeof(DateTime), DbType.DateTime},
            {typeof(string), DbType.String},
            {typeof(byte[]), DbType.Binary},
            {typeof(Guid), DbType.Guid}
        };

        /// <summary>
        ///     Converts the <paramref name="type" /> to its equivalent <see cref="DbType" />.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The <see cref="DbType" />.</returns>
        public static DbType ToDbType(this Type type)
        {
            return DbTypeByTypeCode[type];
        }
    }
}