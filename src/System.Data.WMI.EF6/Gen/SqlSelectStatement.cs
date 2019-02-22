using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Diagnostics;
using System.Globalization;

namespace System.Data.WMI.EF6.Gen
{
    /// <summary>
    ///     A SqlSelectStatement represents a canonical SQL SELECT statement.
    ///     It has fields for the 5 main clauses
    ///     <list type="number">
    ///         <item>SELECT</item>
    ///         <item>FROM</item>
    ///         <item>WHERE</item>
    ///         <item>GROUP BY</item>
    ///         <item>ORDER BY</item>
    ///     </list>
    ///     We do not have HAVING, since it does not correspond to anything in the DbCommandTree.
    ///     Each of the fields is a SqlBuilder, so we can keep appending SQL strings
    ///     or other fragments to build up the clause.
    ///     We have a IsDistinct property to indicate that we want distinct columns.
    ///     This is given out of band, since the input expression to the select clause
    ///     may already have some columns projected out, and we use append-only SqlBuilders.
    ///     The DISTINCT is inserted when we finally write the object into a string.
    ///     Also, we have a Top property, which is non-null if the number of results should
    ///     be limited to certain number. It is given out of band for the same reasons as DISTINCT.
    ///     The FromExtents contains the list of inputs in use for the select statement.
    ///     There is usually just one element in this - Select statements for joins may
    ///     temporarily have more than one.
    ///     If the select statement is created by a Join node, we maintain a list of
    ///     all the extents that have been flattened in the join in AllJoinExtents
    ///     <example>
    ///         in J(j1= J(a,b), c)
    ///         FromExtents has 2 nodes JoinSymbol(name=j1, ...) and Symbol(name=c)
    ///         AllJoinExtents has 3 nodes Symbol(name=a), Symbol(name=b), Symbol(name=c)
    ///     </example>
    ///     If any expression in the non-FROM clause refers to an extent in a higher scope,
    ///     we add that extent to the OuterExtents list.  This list denotes the list
    ///     of extent aliases that may collide with the aliases used in this select statement.
    ///     It is set by <see cref="SqlGenerator.Visit(DbVariableReferenceExpression)" />.
    ///     An extent is an outer extent if it is not one of the FromExtents.
    /// </summary>
    internal sealed class SqlSelectStatement : ISqlFragment
    {
        private List<Symbol> _fromExtents;

        private SqlBuilder _groupBy;

        //indicates whether it is the top most select statement,
        // if not Order By should be omitted unless there is a corresponding TOP

        private SqlBuilder _orderBy;

        private Dictionary<Symbol, bool> _outerExtents;

        private SkipClause _skip;

        private TopClause _top;


        private SqlBuilder _where;

        /// <summary>
        ///     Do we need to add a DISTINCT at the beginning of the SELECT
        /// </summary>
        internal bool IsDistinct { get; set; }

        internal List<Symbol> AllJoinExtents
        {
            get;
            // We have a setter as well, even though this is a list,
            // since we use this field only in special cases.
            set;
        }

        internal List<Symbol> FromExtents => _fromExtents ?? (_fromExtents = new List<Symbol>());

        internal Dictionary<Symbol, bool> OuterExtents => _outerExtents ?? (_outerExtents = new Dictionary<Symbol, bool>());

        internal TopClause Top
        {
            get => _top;
            set
            {
                Debug.Assert(_top == null, "SqlSelectStatement.Top has already been set");
                _top = value;
            }
        }

        internal SkipClause Skip
        {
            get => _skip;
            set
            {
                Debug.Assert(_skip == null, "SqlSelectStatement.Skip has already been set");
                _skip = value;
            }
        }

        internal SqlBuilder Select { get; } = new SqlBuilder();

        internal SqlBuilder From { get; } = new SqlBuilder();

        internal SqlBuilder Where => _where ?? (_where = new SqlBuilder());

        internal SqlBuilder GroupBy => _groupBy ?? (_groupBy = new SqlBuilder());

        public SqlBuilder OrderBy => _orderBy ?? (_orderBy = new SqlBuilder());

        internal bool IsTopMost { get; set; }

        /// <summary>
        ///     Write out a SQL select statement as a string.
        ///     We have to
        ///     <list type="number">
        ///         <item>
        ///             Check whether the aliases extents we use in this statement have
        ///             to be renamed.
        ///             We first create a list of all the aliases used by the outer extents.
        ///             For each of the FromExtents( or AllJoinExtents if it is non-null),
        ///             rename it if it collides with the previous list.
        ///         </item>
        ///         <item>Write each of the clauses (if it exists) as a string</item>
        ///     </list>
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="sqlGenerator"></param>
        public void WriteSql(SqlWriter writer, SqlGenerator sqlGenerator)
        {
            // Create a list of the aliases used by the outer extents
            // JoinSymbols have to be treated specially.
            List<string> outerExtentAliases = null;
            if (null != _outerExtents && 0 < _outerExtents.Count)
                foreach (var outerExtent in _outerExtents.Keys)
                    if (outerExtent is JoinSymbol joinSymbol)
                        foreach (var symbol in joinSymbol.FlattenedExtentList)
                        {
                            if (null == outerExtentAliases)
                                outerExtentAliases = new List<string>();

                            outerExtentAliases.Add(symbol.NewName);
                        }
                    else
                    {
                        if (null == outerExtentAliases)
                            outerExtentAliases = new List<string>();

                        outerExtentAliases.Add(outerExtent.NewName);
                    }

            // An then rename each of the FromExtents we have
            // If AllJoinExtents is non-null - it has precedence.
            // The new name is derived from the old name - we append an increasing int.
            var extentList = AllJoinExtents ?? _fromExtents;
            if (null != extentList)
                foreach (var fromAlias in extentList)
                {
                    if (null != outerExtentAliases && outerExtentAliases.Contains(fromAlias.Name))
                    {
                        var i = sqlGenerator.AllExtentNames[fromAlias.Name];
                        string newName;
                        do
                        {
                            ++i;
                            newName = fromAlias.Name + i.ToString(CultureInfo.InvariantCulture);
                        } while (sqlGenerator.AllExtentNames.ContainsKey(newName));

                        sqlGenerator.AllExtentNames[fromAlias.Name] = i;
                        fromAlias.NewName = newName;

                        // Add extent to list of known names (although i is always incrementing, "prefix11" can
                        // eventually collide with "prefix1" when it is extended)
                        sqlGenerator.AllExtentNames[newName] = 0;
                    }

                    // Add the current alias to the list, so that the extents
                    // that follow do not collide with me.
                    if (null == outerExtentAliases)
                        outerExtentAliases = new List<string>();

                    outerExtentAliases.Add(fromAlias.NewName);
                }

            // Increase the indent, so that the Sql statement is nested by one tab.
            writer.Indent += 1; // ++ can be confusing in this context

            writer.Write("SELECT ");
            if (IsDistinct)
                writer.Write("DISTINCT ");

            if (null == Select || Select.IsEmpty)
                throw new InvalidOperationException("Invalid statement");

            Select.WriteSql(writer, sqlGenerator);

            writer.WriteLine();
            writer.Write("FROM ");
            From.WriteSql(writer, sqlGenerator);

            if (null != _where && !Where.IsEmpty)
            {
                writer.WriteLine();
                writer.Write("WHERE ");
                Where.WriteSql(writer, sqlGenerator);
            }

            if (null != _groupBy && !GroupBy.IsEmpty)
            {
                writer.WriteLine();
                writer.Write("GROUP BY ");
                GroupBy.WriteSql(writer, sqlGenerator);
            }

            if (null != _orderBy && !OrderBy.IsEmpty && (IsTopMost || Top != null))
            {
                writer.WriteLine();
                writer.Write("ORDER BY ");
                OrderBy.WriteSql(writer, sqlGenerator);
            }

            Top?.WriteSql(writer, sqlGenerator);

            if (_skip != null)
                Skip.WriteSql(writer, sqlGenerator);

            --writer.Indent;
        }

        /// <summary>
        ///     Checks if the statement has an ORDER BY, LIMIT, or OFFSET clause.
        /// </summary>
        /// <returns>
        ///     Non-zero if there is an ORDER BY, LIMIT, or OFFSET clause;
        ///     otherwise, zero.
        /// </returns>
        public bool HaveOrderByLimitOrOffset()
        {
            if (_orderBy != null && !_orderBy.IsEmpty)
                return true;

            if (_top != null)
                return true;

            if (_skip != null)
                return true;

            return false;
        }
    }
}