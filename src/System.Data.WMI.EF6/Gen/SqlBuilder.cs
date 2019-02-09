using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Diagnostics;

namespace System.Data.WMI.EF6.Gen
{
    /// <summary>
    ///     This class is like StringBuilder.  While traversing the tree for the first time,
    ///     we do not know all the strings that need to be appended e.g. things that need to be
    ///     renamed, nested select statements etc.  So, we use a builder that can collect
    ///     all kinds of sql fragments.
    /// </summary>
    internal sealed class SqlBuilder : ISqlFragment
    {
        private List<object> _sqlFragments;

        private List<object> SqlFragments => _sqlFragments ?? (_sqlFragments = new List<object>());

        /// <summary>
        ///     Whether the builder is empty.  This is used by the <see cref="SqlGenerator.Visit(DbProjectExpression)" />
        ///     to determine whether a sql statement can be reused.
        /// </summary>
        public bool IsEmpty => null == _sqlFragments || 0 == _sqlFragments.Count;

        /// <summary>
        ///     We delegate the writing of the fragment to the appropriate type.
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="sqlGenerator"></param>
        public void WriteSql(SqlWriter writer, SqlGenerator sqlGenerator)
        {
            if (null == _sqlFragments)
                return;

            foreach (var o in _sqlFragments)
                switch (o)
                {
                    case string str:
                        writer.Write(str);
                        break;
                    case ISqlFragment sqlFragment:
                        sqlFragment.WriteSql(writer, sqlGenerator);
                        break;
                    case char _:
                        writer.Write((char) o);
                        break;
                    default:
                        throw new InvalidOperationException();
                }
        }


        /// <summary>
        ///     Add an object to the list - we do not verify that it is a proper sql fragment
        ///     since this is an internal method.
        /// </summary>
        /// <param name="s"></param>
        public void Append(object s)
        {
            Debug.Assert(s != null);
            SqlFragments.Add(s);
        }

        /// <summary>
        ///     This is to pretty print the SQL.  The writer <see cref="SqlWriter.Write" />
        ///     needs to know about new lines so that it can add the right amount of
        ///     indentation at the beginning of lines.
        /// </summary>
        public void AppendLine()
        {
            SqlFragments.Add("\r\n");
        }
    }
}