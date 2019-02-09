using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Globalization;

namespace System.Data.WMI.EF6.Gen
{
    /// <summary>
    ///     <see cref="SymbolTable" />
    ///     This class represents an extent/nested select statement,
    ///     or a column.
    ///     The important fields are Name, Type and NewName.
    ///     NewName starts off the same as Name, and is then modified as necessary.
    ///     The rest are used by special symbols.
    ///     e.g. NeedsRenaming is used by columns to indicate that a new name must
    ///     be picked for the column in the second phase of translation.
    ///     IsUnnest is used by symbols for a collection expression used as a from clause.
    ///     This allows <see cref="SqlGenerator.AddFromSymbol(SqlSelectStatement, string, Symbol, bool)" /> to add the column
    ///     list
    ///     after the alias.
    /// </summary>
    internal class Symbol : ISqlFragment
    {
        public Symbol(string name, TypeUsage type)
        {
            Name = name;
            NewName = name;
            Type = type;
        }

        internal Dictionary<string, Symbol> Columns { get; } = new Dictionary<string, Symbol>(StringComparer.CurrentCultureIgnoreCase);

        internal bool NeedsRenaming { get; set; }

        internal bool IsUnnest { get; set; } = false;

        public string Name { get; }

        public string NewName { get; set; }

        internal TypeUsage Type { get; set; }

        /// <summary>
        ///     Write this symbol out as a string for sql.  This is just
        ///     the new name of the symbol (which could be the same as the old name).
        ///     We rename columns here if necessary.
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="sqlGenerator"></param>
        public void WriteSql(SqlWriter writer, SqlGenerator sqlGenerator)
        {
            if (NeedsRenaming)
            {
                string newName;
                var i = sqlGenerator.AllColumnNames[NewName];
                do
                {
                    ++i;
                    newName = Name + i.ToString(CultureInfo.InvariantCulture);
                } while (sqlGenerator.AllColumnNames.ContainsKey(newName));

                sqlGenerator.AllColumnNames[NewName] = i;

                // Prevent it from being renamed repeatedly.
                NeedsRenaming = false;
                NewName = newName;

                // Add this column name to list of known names so that there are no subsequent
                // collisions
                sqlGenerator.AllColumnNames[newName] = 0;
            }

            writer.Write(SqlGenerator.QuoteIdentifier(NewName));
        }
    }
}