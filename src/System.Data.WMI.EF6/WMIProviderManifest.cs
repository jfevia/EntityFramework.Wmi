using System.Data.Entity.Core;
using System.Data.Entity.Core.Common;
using System.Data.Entity.Core.Metadata.Edm;
using System.Reflection;
using System.Xml;

namespace System.Data.WMI.EF6
{
    internal sealed class WMIProviderManifest : DbXmlEnabledProviderManifest
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="WMIProviderManifest" /> class.
        /// </summary>
        public WMIProviderManifest()
            : base(GetProviderManifest())
        {
        }

        private static XmlReader GetProviderManifest()
        {
            return GetXmlResource("System.Data.WMI.WMIProviderServices.ProviderManifest.xml");
        }

        /// <summary>
        ///     Returns manifest information for the provider
        /// </summary>
        /// <param name="informationType">The name of the information to be retrieved.</param>
        /// <returns>An XmlReader at the beginning of the information requested.</returns>
        protected override XmlReader GetDbInformation(string informationType)
        {
            if (informationType == StoreSchemaDefinition)
                return GetStoreSchemaDescription();

            if (informationType == StoreSchemaMapping)
                return GetStoreSchemaMapping();

            if (informationType == ConceptualSchemaDefinition)
                return null;

            throw new ProviderIncompatibleException($"WMI does not support this information type '{informationType}'.");
        }

        /// <summary>
        ///     This method takes a type and a set of facets and returns the best mapped equivalent type
        ///     in EDM.
        /// </summary>
        /// <param name="storeType">A TypeUsage encapsulating a store type and a set of facets</param>
        /// <returns>A TypeUsage encapsulating an EDM type and a set of facets</returns>
        public override TypeUsage GetEdmType(TypeUsage storeType)
        {
            if (storeType == null) throw new ArgumentNullException(nameof(storeType));

            var storeTypeName = storeType.EdmType.Name.ToLowerInvariant();
            if (StoreTypeNameToEdmPrimitiveType.TryGetValue(storeTypeName, out var edmPrimitiveType) == false)
                throw new ArgumentException($"WMI does not support the type '{storeTypeName}'.");

            int maxLength;
            var isUnicode = true;
            bool isFixedLen;
            bool isUnbounded;

            PrimitiveTypeKind newPrimitiveTypeKind;

            switch (storeTypeName)
            {
                case "tinyint":
                case "smallint":
                case "integer":
                case "bit":
                case "uniqueidentifier":
                case "int":
                case "float":
                case "real":
                    return TypeUsage.CreateDefaultTypeUsage(edmPrimitiveType);

                case "varchar":
                    newPrimitiveTypeKind = PrimitiveTypeKind.String;
                    isUnbounded = !TypeHelpers.TryGetMaxLength(storeType, out maxLength);
                    isUnicode = false;
                    isFixedLen = false;
                    break;
                case "char":
                    newPrimitiveTypeKind = PrimitiveTypeKind.String;
                    isUnbounded = !TypeHelpers.TryGetMaxLength(storeType, out maxLength);
                    isUnicode = false;
                    isFixedLen = true;
                    break;
                case "nvarchar":
                    newPrimitiveTypeKind = PrimitiveTypeKind.String;
                    isUnbounded = !TypeHelpers.TryGetMaxLength(storeType, out maxLength);
                    isUnicode = true;
                    isFixedLen = false;
                    break;
                case "nchar":
                    newPrimitiveTypeKind = PrimitiveTypeKind.String;
                    isUnbounded = !TypeHelpers.TryGetMaxLength(storeType, out maxLength);
                    isUnicode = true;
                    isFixedLen = true;
                    break;
                case "blob":
                    newPrimitiveTypeKind = PrimitiveTypeKind.Binary;
                    isUnbounded = !TypeHelpers.TryGetMaxLength(storeType, out maxLength);
                    isFixedLen = false;
                    break;
                case "decimal":
                    if (TypeHelpers.TryGetPrecision(storeType, out var precision) && TypeHelpers.TryGetScale(storeType, out var scale))
                        return TypeUsage.CreateDecimalTypeUsage(edmPrimitiveType, precision, scale);
                    return TypeUsage.CreateDecimalTypeUsage(edmPrimitiveType);
                case "datetime":
                    return TypeUsage.CreateDateTimeTypeUsage(edmPrimitiveType, null);
                default:
                    throw new NotSupportedException($"WMI does not support the type '{storeTypeName}'.");
            }

            switch (newPrimitiveTypeKind)
            {
                case PrimitiveTypeKind.String:
                    if (!isUnbounded)
                        return TypeUsage.CreateStringTypeUsage(edmPrimitiveType, isUnicode, isFixedLen, maxLength);
                    else
                        return TypeUsage.CreateStringTypeUsage(edmPrimitiveType, isUnicode, isFixedLen);
                case PrimitiveTypeKind.Binary:
                    if (!isUnbounded)
                        return TypeUsage.CreateBinaryTypeUsage(edmPrimitiveType, isFixedLen, maxLength);
                    else
                        return TypeUsage.CreateBinaryTypeUsage(edmPrimitiveType, isFixedLen);
                default:
                    throw new NotSupportedException($"WMI does not support the type '{storeTypeName}'.");
            }
        }

        /// <summary>
        ///     This method takes a type and a set of facets and returns the best mapped equivalent type
        /// </summary>
        /// <param name="edmType">A TypeUsage encapsulating an EDM type and a set of facets</param>
        /// <returns>A TypeUsage encapsulating a store type and a set of facets</returns>
        public override TypeUsage GetStoreType(TypeUsage edmType)
        {
            if (edmType == null)
                throw new ArgumentNullException(nameof(edmType));

            if (!(edmType.EdmType is PrimitiveType primitiveType))
                throw new ArgumentException($"WMI does not support the type '{edmType}'.");

            var facets = edmType.Facets;

            switch (primitiveType.PrimitiveTypeKind)
            {
                case PrimitiveTypeKind.Boolean:
                    return TypeUsage.CreateDefaultTypeUsage(StoreTypeNameToStorePrimitiveType["bit"]);
                case PrimitiveTypeKind.Byte:
                    return TypeUsage.CreateDefaultTypeUsage(StoreTypeNameToStorePrimitiveType["tinyint"]);
                case PrimitiveTypeKind.Int16:
                    return TypeUsage.CreateDefaultTypeUsage(StoreTypeNameToStorePrimitiveType["smallint"]);
                case PrimitiveTypeKind.Int32:
                    return TypeUsage.CreateDefaultTypeUsage(StoreTypeNameToStorePrimitiveType["int"]);
                case PrimitiveTypeKind.Int64:
                    return TypeUsage.CreateDefaultTypeUsage(StoreTypeNameToStorePrimitiveType["integer"]);
                case PrimitiveTypeKind.Guid:
                    return TypeUsage.CreateDefaultTypeUsage(StoreTypeNameToStorePrimitiveType["uniqueidentifier"]);
                case PrimitiveTypeKind.Double:
                    return TypeUsage.CreateDefaultTypeUsage(StoreTypeNameToStorePrimitiveType["float"]);
                case PrimitiveTypeKind.Single:
                    return TypeUsage.CreateDefaultTypeUsage(StoreTypeNameToStorePrimitiveType["real"]);
                case PrimitiveTypeKind.Decimal:
                {
                    // This applies to decimal, numeric, smallmoney, money
                    if (!TypeHelpers.TryGetPrecision(edmType, out var precision))
                        precision = 18;

                    if (!TypeHelpers.TryGetScale(edmType, out var scale))
                        scale = 0;

                    return TypeUsage.CreateDecimalTypeUsage(StoreTypeNameToStorePrimitiveType["decimal"], precision, scale);
                }
                case PrimitiveTypeKind.Binary:
                {
                    // This applies to binary, varbinary, varbinary(max), image, timestamp, rowversion
                    var isFixedLength = null != facets["FixedLength"].Value && (bool) facets["FixedLength"].Value;
                    var f = facets["MaxLength"];

                    var isMaxLength = f.IsUnbounded || null == f.Value;
                    var maxLength = !isMaxLength ? (int) f.Value : int.MinValue;

                    TypeUsage tu;
                    if (isFixedLength)
                        tu = TypeUsage.CreateBinaryTypeUsage(StoreTypeNameToStorePrimitiveType["blob"], true, maxLength);
                    else
                    {
                        if (isMaxLength)
                            tu = TypeUsage.CreateBinaryTypeUsage(StoreTypeNameToStorePrimitiveType["blob"], false);
                        else
                            tu = TypeUsage.CreateBinaryTypeUsage(StoreTypeNameToStorePrimitiveType["blob"], false, maxLength);
                    }

                    return tu;
                }
                case PrimitiveTypeKind.String:
                {
                    // This applies to char, nchar, varchar, nvarchar, varchar(max), nvarchar(max), ntext, text
                    var isUnicode = null == facets["Unicode"].Value || (bool) facets["Unicode"].Value;
                    var isFixedLength = null != facets["FixedLength"].Value && (bool) facets["FixedLength"].Value;
                    var facet = facets["MaxLength"];

                    // maxlen is true if facet value is unbounded, the value is bigger than the limited string sizes *or* the facet
                    // value is null. this is needed since functions still have maxlength facet value as null
                    var isMaxLength = facet.IsUnbounded || null == facet.Value;
                    var maxLength = !isMaxLength ? (int) facet.Value : int.MinValue;

                    TypeUsage typeUsage;

                    if (isUnicode)
                    {
                        if (isFixedLength)
                            typeUsage = TypeUsage.CreateStringTypeUsage(StoreTypeNameToStorePrimitiveType["nchar"], true, true, maxLength);
                        else
                        {
                            if (isMaxLength)
                                typeUsage = TypeUsage.CreateStringTypeUsage(StoreTypeNameToStorePrimitiveType["nvarchar"], true, false);
                            else
                                typeUsage = TypeUsage.CreateStringTypeUsage(StoreTypeNameToStorePrimitiveType["nvarchar"], true, false, maxLength);
                        }
                    }
                    else
                    {
                        if (isFixedLength)
                            typeUsage = TypeUsage.CreateStringTypeUsage(StoreTypeNameToStorePrimitiveType["char"], false, true, maxLength);
                        else
                        {
                            if (isMaxLength)
                                typeUsage = TypeUsage.CreateStringTypeUsage(StoreTypeNameToStorePrimitiveType["varchar"], false, false);
                            else
                                typeUsage = TypeUsage.CreateStringTypeUsage(StoreTypeNameToStorePrimitiveType["varchar"], false, false, maxLength);
                        }
                    }

                    return typeUsage;
                }
                case PrimitiveTypeKind.DateTime:
                    // This applies to datetime, smalldatetime
                    return TypeUsage.CreateDefaultTypeUsage(StoreTypeNameToStorePrimitiveType["datetime"]);
                default:
                    throw new NotSupportedException($"There is no store type corresponding to the EDM type '{edmType}' of primitive type '{primitiveType.PrimitiveTypeKind}'.");
            }
        }

        private XmlReader GetStoreSchemaMapping()
        {
            return GetXmlResource("System.Data.WMI.WMIProviderServices.StoreSchemaMapping.msl");
        }

        private XmlReader GetStoreSchemaDescription()
        {
            return GetXmlResource("System.Data.WMI.WMIProviderServices.StoreSchemaDefinition.ssdl");
        }

        internal static XmlReader GetXmlResource(string resourceName)
        {
            var executingAssembly = Assembly.GetExecutingAssembly();
            var stream = executingAssembly.GetManifestResourceStream(resourceName);
            return XmlReader.Create(stream);
        }

        private static class TypeHelpers
        {
            public static bool TryGetPrecision(TypeUsage typeUsage, out byte precision)
            {
                precision = 0;
                if (!typeUsage.Facets.TryGetValue("Precision", false, out var facet))
                    return false;

                if (facet.IsUnbounded || facet.Value == null)
                    return false;

                precision = (byte) facet.Value;
                return true;
            }

            public static bool TryGetMaxLength(TypeUsage typeUsage, out int maxLength)
            {
                maxLength = 0;
                if (!typeUsage.Facets.TryGetValue("MaxLength", false, out var facet))
                    return false;

                if (facet.IsUnbounded || facet.Value == null)
                    return false;

                maxLength = (int) facet.Value;
                return true;
            }

            public static bool TryGetScale(TypeUsage typeUsage, out byte scale)
            {
                scale = 0;
                if (!typeUsage.Facets.TryGetValue("Scale", false, out var facet))
                    return false;

                if (facet.IsUnbounded || facet.Value == null)
                    return false;

                scale = (byte) facet.Value;
                return true;
            }
        }
    }
}