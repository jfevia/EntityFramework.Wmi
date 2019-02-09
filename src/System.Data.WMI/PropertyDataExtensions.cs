using System.Collections.Generic;
using System.Management;

namespace System.Data.WMI
{
    internal static class PropertyDataExtensions
    {
        private static readonly Dictionary<CimType, Type> CimTypeToSystemType = new Dictionary<CimType, Type>
        {
            {CimType.Boolean, typeof(bool)},
            {CimType.Char16, typeof(string)},
            {CimType.DateTime, typeof(DateTime)},
            {CimType.Object, typeof(object)},
            {CimType.Real32, typeof(decimal)},
            {CimType.Real64, typeof(decimal)},
            {CimType.Reference, typeof(object)},
            {CimType.SInt16, typeof(short)},
            {CimType.SInt32, typeof(int)},
            {CimType.SInt8, typeof(sbyte)},
            {CimType.String, typeof(string)},
            {CimType.UInt8, typeof(byte)},
            {CimType.UInt16, typeof(ushort)},
            {CimType.UInt32, typeof(uint)},
            {CimType.UInt64, typeof(ulong)}
        };

        /// <summary>
        ///     Converts the <paramref name="propertyData" /> to its equivalent <see cref="Type" />.
        /// </summary>
        /// <param name="propertyData">The property data.</param>
        /// <returns>The <see cref="Type" />.</returns>
        public static Type ToSystemType(this PropertyData propertyData)
        {
            var type = CimTypeToSystemType[propertyData.Type];
            if (propertyData.IsArray)
                type = type.MakeArrayType();
            return type;
        }
    }
}