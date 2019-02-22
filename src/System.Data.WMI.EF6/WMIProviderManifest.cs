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

            switch (storeTypeName)
            {
                case "boolean":
                case "sint8":
                case "sint16":
                case "sint32":
                case "sint64":
                case "uint8":
                case "uint16":
                case "uint32":
                case "uint64":
                case "real32":
                case "real64":
                    return TypeUsage.CreateDefaultTypeUsage(edmPrimitiveType);

                case "string":
                case "char16":
                    return TypeUsage.CreateStringTypeUsage(edmPrimitiveType, true, false, int.MaxValue);

                case "datetime":
                    return TypeUsage.CreateDateTimeTypeUsage(edmPrimitiveType, null);
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

            switch (primitiveType.PrimitiveTypeKind)
            {
                case PrimitiveTypeKind.Boolean:
                    return TypeUsage.CreateDefaultTypeUsage(StoreTypeNameToStorePrimitiveType["boolean"]);
                case PrimitiveTypeKind.SByte:
                    return TypeUsage.CreateDefaultTypeUsage(StoreTypeNameToStorePrimitiveType["sint8"]);
                case PrimitiveTypeKind.Int16:
                    return TypeUsage.CreateDefaultTypeUsage(StoreTypeNameToStorePrimitiveType["sint16"]);
                case PrimitiveTypeKind.Int32:
                    return TypeUsage.CreateDefaultTypeUsage(StoreTypeNameToStorePrimitiveType["sint32"]);
                case PrimitiveTypeKind.Int64:
                    return TypeUsage.CreateDefaultTypeUsage(StoreTypeNameToStorePrimitiveType["sint64"]);
                case PrimitiveTypeKind.Double:
                    return TypeUsage.CreateDefaultTypeUsage(StoreTypeNameToStorePrimitiveType["real64"]);
                case PrimitiveTypeKind.Single:
                    return TypeUsage.CreateDefaultTypeUsage(StoreTypeNameToStorePrimitiveType["real32"]);
                case PrimitiveTypeKind.Byte:
                    return TypeUsage.CreateDefaultTypeUsage(StoreTypeNameToStorePrimitiveType["uint8"]);
                case PrimitiveTypeKind.Decimal:
                    return TypeUsage.CreateDefaultTypeUsage(StoreTypeNameToStorePrimitiveType["uint64"]);
                case PrimitiveTypeKind.String:
                    return TypeUsage.CreateStringTypeUsage(StoreTypeNameToStorePrimitiveType["string"], true, false, int.MaxValue);
                case PrimitiveTypeKind.DateTime:
                    return TypeUsage.CreateDefaultTypeUsage(StoreTypeNameToStorePrimitiveType["datetime"]);
                //case PrimitiveTypeKind.UInt16:
                //  return TypeUsage.CreateDefaultTypeUsage(StoreTypeNameToStorePrimitiveType["uint16"]);
                //case PrimitiveTypeKind.UInt32:
                //  return TypeUsage.CreateDefaultTypeUsage(StoreTypeNameToStorePrimitiveType["uint32"]);
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
            return XmlReader.Create(stream ?? throw new InvalidOperationException("Stream cannot be null"));
        }
    }
}