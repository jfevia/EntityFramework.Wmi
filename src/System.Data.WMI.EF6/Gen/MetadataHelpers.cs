using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Diagnostics;

namespace System.Data.WMI.EF6.Gen
{
    /// <summary>
    ///     A set of static helpers for type metadata
    /// </summary>
    internal static class MetadataHelpers
    {
        // Returns ParameterDirection corresponding to given ParameterMode
        internal static ParameterDirection ParameterModeToParameterDirection(ParameterMode mode)
        {
            switch (mode)
            {
                case ParameterMode.In:
                    return ParameterDirection.Input;

                case ParameterMode.InOut:
                    return ParameterDirection.InputOutput;

                case ParameterMode.Out:
                    return ParameterDirection.Output;

                case ParameterMode.ReturnValue:
                    return ParameterDirection.ReturnValue;

                default:
                    Debug.Fail("unrecognized mode " + mode);
                    return default(ParameterDirection);
            }
        }

        /// <summary>
        ///     Cast the EdmType of the given type usage to the given TEdmType
        /// </summary>
        /// <typeparam name="TEdmType"></typeparam>
        /// <param name="typeUsage"></param>
        /// <returns></returns>
        internal static TEdmType GetEdmType<TEdmType>(TypeUsage typeUsage)
            where TEdmType : EdmType
        {
            return (TEdmType) typeUsage.EdmType;
        }

        /// <summary>
        ///     Gets the TypeUsage of the element if the given type is a collection type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        internal static TypeUsage GetElementTypeUsage(TypeUsage type)
        {
            if (IsCollectionType(type))
                return ((CollectionType) type.EdmType).TypeUsage;

            return null;
        }

        /// <summary>
        ///     Retrieves the properties of in the EdmType underlying the input type usage,
        ///     if that EdmType is a structured type (EntityType, RowType).
        /// </summary>
        /// <param name="typeUsage"></param>
        /// <returns></returns>
        internal static IList<EdmProperty> GetProperties(TypeUsage typeUsage)
        {
            return GetProperties(typeUsage.EdmType);
        }

        /// <summary>
        ///     Retrieves the properties of the given EdmType, if it is
        ///     a structured type (EntityType, RowType).
        /// </summary>
        /// <param name="edmType"></param>
        /// <returns></returns>
        internal static IList<EdmProperty> GetProperties(EdmType edmType)
        {
            switch (edmType.BuiltInTypeKind)
            {
                case BuiltInTypeKind.ComplexType:
                    return ((ComplexType) edmType).Properties;
                case BuiltInTypeKind.EntityType:
                    return ((EntityType) edmType).Properties;
                case BuiltInTypeKind.RowType:
                    return ((RowType) edmType).Properties;
                default:
                    return new List<EdmProperty>();
            }
        }

        /// <summary>
        ///     Is the given type usage over a collection type
        /// </summary>
        /// <param name="typeUsage"></param>
        /// <returns></returns>
        internal static bool IsCollectionType(TypeUsage typeUsage)
        {
            return IsCollectionType(typeUsage.EdmType);
        }

        /// <summary>
        ///     Is the given type a collection type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        internal static bool IsCollectionType(EdmType type)
        {
            return BuiltInTypeKind.CollectionType == type.BuiltInTypeKind;
        }

        /// <summary>
        ///     Is the given type usage over a primitive type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        internal static bool IsPrimitiveType(TypeUsage type)
        {
            return IsPrimitiveType(type.EdmType);
        }

        /// <summary>
        ///     Is the given type a primitive type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        internal static bool IsPrimitiveType(EdmType type)
        {
            return BuiltInTypeKind.PrimitiveType == type.BuiltInTypeKind;
        }

        /// <summary>
        ///     Is the given type usage over a row type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        internal static bool IsRowType(TypeUsage type)
        {
            return IsRowType(type.EdmType);
        }

        /// <summary>
        ///     Is the given type a row type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        internal static bool IsRowType(EdmType type)
        {
            return BuiltInTypeKind.RowType == type.BuiltInTypeKind;
        }

        /// <summary>
        ///     Gets the type of the given type usage if it is a primitive type
        /// </summary>
        /// <param name="type"></param>
        /// <param name="typeKind"></param>
        /// <returns></returns>
        internal static bool TryGetPrimitiveTypeKind(TypeUsage type, out PrimitiveTypeKind typeKind)
        {
            if (type?.EdmType != null && type.EdmType.BuiltInTypeKind == BuiltInTypeKind.PrimitiveType)
            {
                typeKind = ((PrimitiveType) type.EdmType).PrimitiveTypeKind;
                return true;
            }

            typeKind = default(PrimitiveTypeKind);
            return false;
        }

        internal static PrimitiveTypeKind GetPrimitiveTypeKind(TypeUsage type)
        {
            if (!TryGetPrimitiveTypeKind(type, out var returnValue))
            {
                Debug.Assert(false, "Cannot create parameter of non-primitive type");
                throw new NotSupportedException("Cannot create parameter of non-primitive type");
            }

            return returnValue;
        }

        /// <summary>
        ///     Gets the value for the metadata property with the given name
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        internal static T TryGetValueForMetadataProperty<T>(MetadataItem item, string propertyName)
        {
            if (!item.MetadataProperties.TryGetValue(propertyName, true, out var property)) return default(T);

            return (T) property.Value;
        }

        internal static bool IsPrimitiveType(TypeUsage type, PrimitiveTypeKind primitiveType)
        {
            if (TryGetPrimitiveTypeKind(type, out var typeKind)) return typeKind == primitiveType;
            return false;
        }

        internal static DbType GetDbType(PrimitiveTypeKind primitiveType)
        {
            switch (primitiveType)
            {
                case PrimitiveTypeKind.Binary: return DbType.Binary;
                case PrimitiveTypeKind.Boolean: return DbType.Boolean;
                case PrimitiveTypeKind.Byte: return DbType.Byte;
                case PrimitiveTypeKind.DateTime: return DbType.DateTime;
                case PrimitiveTypeKind.Decimal: return DbType.Decimal;
                case PrimitiveTypeKind.Double: return DbType.Double;
                case PrimitiveTypeKind.Single: return DbType.Single;
                case PrimitiveTypeKind.Guid: return DbType.Guid;
                case PrimitiveTypeKind.Int16: return DbType.Int16;
                case PrimitiveTypeKind.Int32: return DbType.Int32;
                case PrimitiveTypeKind.Int64: return DbType.Int64;
                //case PrimitiveTypeKind.Money: return DbType.Decimal;
                case PrimitiveTypeKind.SByte: return DbType.SByte;
                case PrimitiveTypeKind.String: return DbType.String;
                //case PrimitiveTypeKind.UInt16: return DbType.UInt16;
                //case PrimitiveTypeKind.UInt32: return DbType.UInt32;
                //case PrimitiveTypeKind.UInt64: return DbType.UInt64;
                //case PrimitiveTypeKind.Xml: return DbType.Xml;
                default:
                    Debug.Fail("unknown PrimitiveTypeKind" + primitiveType);
                    throw new InvalidOperationException($"Unknown PrimitiveTypeKind {primitiveType}");
            }
        }

        internal static readonly int UnicodeStringMaxMaxLength = int.MaxValue;
        internal static readonly int AsciiStringMaxMaxLength = int.MaxValue;
        internal static readonly int BinaryMaxMaxLength = int.MaxValue;

        /// <summary>
        ///     Name of the MaxLength Facet
        /// </summary>
        public static readonly string MaxLengthFacetName = "MaxLength";

        /// <summary>
        ///     Name of the Unicode Facet
        /// </summary>
        public static readonly string UnicodeFacetName = "Unicode";

        /// <summary>
        ///     Name of the FixedLength Facet
        /// </summary>
        public static readonly string FixedLengthFacetName = "FixedLength";

        /// <summary>
        ///     Name of the PreserveSeconds Facet
        /// </summary>
        public static readonly string PreserveSecondsFacetName = "PreserveSeconds";

        /// <summary>
        ///     Name of the Precision Facet
        /// </summary>
        public static readonly string PrecisionFacetName = "Precision";

        /// <summary>
        ///     Name of the Scale Facet
        /// </summary>
        public static readonly string ScaleFacetName = "Scale";

        /// <summary>
        ///     Name of the DefaultValue Facet
        /// </summary>
        public static readonly string DefaultValueFacetName = "DefaultValue";

        /// <summary>
        ///     Name of the Nullable Facet
        /// </summary>
        internal const string NullableFacetName = "Nullable";

        /// <summary>
        ///     Get the value specified on the given type usage for the given facet name.
        ///     If the faces does not have a value specified or that value is null returns
        ///     the default value for that facet.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <param name="facetName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        internal static T GetFacetValueOrDefault<T>(TypeUsage type, string facetName, T defaultValue)
        {
            //Get the value for the facet, if any
            if (type.Facets.TryGetValue(facetName, false, out var facet) && facet.Value != null && !facet.IsUnbounded)
                return (T) facet.Value;
            return defaultValue;
        }

        internal static bool IsFacetValueConstant(TypeUsage type, string facetName)
        {
            return GetFacet(((PrimitiveType) type.EdmType).FacetDescriptions, facetName).IsConstant;
        }

        private static FacetDescription GetFacet(IEnumerable<FacetDescription> facetCollection, string facetName)
        {
            foreach (var facetDescription in facetCollection)
                if (facetDescription.FacetName == facetName)
                    return facetDescription;

            return null;
        }

        /// <summary>
        ///     Given a facet name and an EdmType, tries to get that facet's description.
        /// </summary>
        /// <param name="edmType"></param>
        /// <param name="facetName"></param>
        /// <param name="facetDescription"></param>
        /// <returns></returns>
        internal static bool TryGetTypeFacetDescriptionByName(EdmType edmType, string facetName, out FacetDescription facetDescription)
        {
            facetDescription = null;
            if (IsPrimitiveType(edmType))
            {
                var primitiveType = (PrimitiveType) edmType;
                foreach (var fd in primitiveType.FacetDescriptions)
                    if (facetName.Equals(fd.FacetName, StringComparison.OrdinalIgnoreCase))
                    {
                        facetDescription = fd;
                        return true;
                    }
            }

            return false;
        }

        internal static bool IsNullable(TypeUsage type)
        {
            if (type.Facets.TryGetValue(NullableFacetName, false, out var nullableFacet)) return (bool) nullableFacet.Value;
            return false;
        }

        internal static bool TryGetMaxLength(TypeUsage type, out int maxLength)
        {
            if (!IsPrimitiveType(type, PrimitiveTypeKind.String) &&
                !IsPrimitiveType(type, PrimitiveTypeKind.Binary))
            {
                maxLength = 0;
                return false;
            }

            // Binary and String FixedLength facets share the same name
            return TryGetIntFacetValue(type, MaxLengthFacetName, out maxLength);
        }

        internal static bool TryGetIntFacetValue(TypeUsage type, string facetName, out int intValue)
        {
            intValue = 0;

            if (type.Facets.TryGetValue(facetName, false, out var intFacet) && intFacet.Value != null && !intFacet.IsUnbounded)
            {
                intValue = (int) intFacet.Value;
                return true;
            }

            return false;
        }

        internal static bool TryGetIsFixedLength(TypeUsage type, out bool isFixedLength)
        {
            if (!IsPrimitiveType(type, PrimitiveTypeKind.String) &&
                !IsPrimitiveType(type, PrimitiveTypeKind.Binary))
            {
                isFixedLength = false;
                return false;
            }

            // Binary and String MaxLength facets share the same name
            return TryGetBooleanFacetValue(type, FixedLengthFacetName, out isFixedLength);
        }

        internal static bool TryGetBooleanFacetValue(TypeUsage type, string facetName, out bool boolValue)
        {
            boolValue = false;
            if (type.Facets.TryGetValue(facetName, false, out var boolFacet) && boolFacet.Value != null)
            {
                boolValue = (bool) boolFacet.Value;
                return true;
            }

            return false;
        }

        internal static bool TryGetIsUnicode(TypeUsage type, out bool isUnicode)
        {
            if (!IsPrimitiveType(type, PrimitiveTypeKind.String))
            {
                isUnicode = false;
                return false;
            }

            return TryGetBooleanFacetValue(type, UnicodeFacetName, out isUnicode);
        }
    }
}