using System.Globalization;

namespace System.Data.WMI.EF6.Gen
{
    /// <summary>
    ///     TopClause represents the a TOP expression in a SqlSelectStatement.
    ///     It has a count property, which indicates how many TOP rows should be selected and a
    ///     boolean WithTies property.
    /// </summary>
    internal class TopClause : ISqlFragment
    {
        /// <summary>
        ///     Creates a TopClause with the given topCount and withTies.
        /// </summary>
        /// <param name="topCount"></param>
        /// <param name="withTies"></param>
        internal TopClause(ISqlFragment topCount, bool withTies)
        {
            TopCount = topCount;
            WithTies = withTies;
        }

        /// <summary>
        ///     Creates a TopClause with the given topCount and withTies.
        /// </summary>
        /// <param name="topCount"></param>
        /// <param name="withTies"></param>
        internal TopClause(int topCount, bool withTies)
        {
            var sqlBuilder = new SqlBuilder();
            sqlBuilder.Append(topCount.ToString(CultureInfo.InvariantCulture));
            TopCount = sqlBuilder;
            WithTies = withTies;
        }

        /// <summary>
        ///     Do we need to add a WITH_TIES to the top statement
        /// </summary>
        internal bool WithTies { get; }

        /// <summary>
        ///     How many top rows should be selected.
        /// </summary>
        internal ISqlFragment TopCount { get; }

        /// <summary>
        ///     Write out the TOP part of sql select statement
        ///     It basically writes LIMIT (X).
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="sqlGenerator"></param>
        public void WriteSql(SqlWriter writer, SqlGenerator sqlGenerator)
        {
            writer.Write(" LIMIT ");
            TopCount.WriteSql(writer, sqlGenerator);

            if (WithTies)
                throw new NotSupportedException("WITH TIES");

            //writer.Write(" ");

            //if (this.WithTies)
            //{
            //    writer.Write("WITH TIES ");
            //}
        }
    }
}