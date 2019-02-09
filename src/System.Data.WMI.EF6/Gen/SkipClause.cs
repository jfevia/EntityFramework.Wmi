using System.Globalization;

namespace System.Data.WMI.EF6.Gen
{
    /// <summary>
    ///     SkipClause represents the a SKIP expression in a SqlSelectStatement.
    ///     It has a count property, which indicates how many rows should be skipped.
    /// </summary>
    internal class SkipClause : ISqlFragment
    {
        /// <summary>
        ///     Creates a SkipClause with the given skipCount.
        /// </summary>
        /// <param name="skipCount"></param>
        internal SkipClause(ISqlFragment skipCount)
        {
            SkipCount = skipCount;
        }

        /// <summary>
        ///     Creates a SkipClause with the given skipCount.
        /// </summary>
        /// <param name="skipCount"></param>
        internal SkipClause(int skipCount)
        {
            var sqlBuilder = new SqlBuilder();
            sqlBuilder.Append(skipCount.ToString(CultureInfo.InvariantCulture));
            SkipCount = sqlBuilder;
        }

        /// <summary>
        ///     How many rows should be skipped.
        /// </summary>
        internal ISqlFragment SkipCount { get; }

        /// <summary>
        ///     Write out the SKIP part of sql select statement
        ///     It basically writes OFFSET (X).
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="sqlGenerator"></param>
        public void WriteSql(SqlWriter writer, SqlGenerator sqlGenerator)
        {
            writer.Write(" OFFSET ");
            SkipCount.WriteSql(writer, sqlGenerator);
        }
    }
}