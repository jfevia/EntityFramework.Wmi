using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime;
using System.Text;

namespace System.Data.WMI.EF6.Gen
{
    internal static class StringUtil
    {
        // Fields
        private const string DefaultDelimiter = ", ";

        // Methods
        internal static string BuildDelimitedList<T>(IEnumerable<T> values, ToStringConverter<T> converter, string delimiter)
        {
            if (values == null)
                return string.Empty;

            if (converter == null)
                converter = InvariantConvertToString;

            if (delimiter == null)
                delimiter = ", ";

            var builder = new StringBuilder();
            var flag = true;
            foreach (var local in values)
            {
                if (flag)
                    flag = false;
                else
                    builder.Append(delimiter);
                builder.Append(converter(local));
            }

            return builder.ToString();
        }

        internal static string FormatIndex(string arrayVarName, int index)
        {
            var builder = new StringBuilder(arrayVarName.Length + 10 + 2);
            return builder.Append(arrayVarName).Append('[').Append(index).Append(']').ToString();
        }

        internal static string FormatInvariant(string format, params object[] args)
        {
            return string.Format(CultureInfo.InvariantCulture, format, args);
        }

        internal static StringBuilder FormatStringBuilder(StringBuilder builder, string format, params object[] args)
        {
            builder.AppendFormat(CultureInfo.InvariantCulture, format, args);
            return builder;
        }

        internal static StringBuilder IndentNewLine(StringBuilder builder, int indent)
        {
            builder.AppendLine();
            for (var i = 0; i < indent; i++) builder.Append("    ");
            return builder;
        }

        private static string InvariantConvertToString<T>(T value)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}", new object[] {value});
        }

		[TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        internal static bool IsNullOrEmptyOrWhiteSpace(string value)
        {
            return IsNullOrEmptyOrWhiteSpace(value, 0);
        }

        internal static bool IsNullOrEmptyOrWhiteSpace(string value, int offset)
        {
            if (value != null)
                for (var i = offset; i < value.Length; i++)
                    if (!char.IsWhiteSpace(value[i]))
                        return false;
            return true;
        }

        internal static bool IsNullOrEmptyOrWhiteSpace(string value, int offset, int length)
        {
            if (value != null)
            {
                length = Math.Min(value.Length, length);
                for (var i = offset; i < length; i++)
                    if (!char.IsWhiteSpace(value[i]))
                        return false;
            }

            return true;
        }

        internal static string MembersToCommaSeparatedString(IEnumerable members)
        {
            var builder = new StringBuilder();
            builder.Append("{");
            ToCommaSeparatedString(builder, members);
            builder.Append("}");
            return builder.ToString();
        }

        internal static string ToCommaSeparatedString(IEnumerable list)
        {
            return ToSeparatedString(list, ", ", string.Empty);
        }

        internal static void ToCommaSeparatedString(StringBuilder builder, IEnumerable list)
        {
            ToSeparatedStringPrivate(builder, list, ", ", string.Empty, false);
        }

        internal static string ToCommaSeparatedStringSorted(IEnumerable list)
        {
            return ToSeparatedStringSorted(list, ", ", string.Empty);
        }

        internal static void ToCommaSeparatedStringSorted(StringBuilder builder, IEnumerable list)
        {
            ToSeparatedStringPrivate(builder, list, ", ", string.Empty, true);
        }

        internal static string ToSeparatedString(IEnumerable list, string separator, string nullValue)
        {
            var stringBuilder = new StringBuilder();
            ToSeparatedString(stringBuilder, list, separator, nullValue);
            return stringBuilder.ToString();
        }

        internal static void ToSeparatedString(StringBuilder builder, IEnumerable list, string separator)
        {
            ToSeparatedStringPrivate(builder, list, separator, string.Empty, false);
        }

		[TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        internal static void ToSeparatedString(StringBuilder stringBuilder, IEnumerable list, string separator, string nullValue)
        {
            ToSeparatedStringPrivate(stringBuilder, list, separator, nullValue, false);
        }

        private static void ToSeparatedStringPrivate(StringBuilder stringBuilder, IEnumerable list, string separator, string nullValue, bool toSort)
        {
            if (list != null)
            {
                var flag = true;
                var list2 = new List<string>();
                foreach (var obj2 in list)
                {
                    string str;
                    if (obj2 == null)
                        str = nullValue;
                    else
                        str = FormatInvariant("{0}", obj2);
                    list2.Add(str);
                }

                if (toSort) list2.Sort(StringComparer.Ordinal);
                foreach (var str2 in list2)
                {
                    if (!flag) stringBuilder.Append(separator);
                    stringBuilder.Append(str2);
                    flag = false;
                }
            }
        }

        internal static string ToSeparatedStringSorted(IEnumerable list, string separator, string nullValue)
        {
            var stringBuilder = new StringBuilder();
            ToSeparatedStringPrivate(stringBuilder, list, separator, nullValue, true);
            return stringBuilder.ToString();
        }

        internal static void ToSeparatedStringSorted(StringBuilder builder, IEnumerable list, string separator)
        {
            ToSeparatedStringPrivate(builder, list, separator, string.Empty, true);
        }

        // Nested Types
        internal delegate string ToStringConverter<T>(T value);
    }
}