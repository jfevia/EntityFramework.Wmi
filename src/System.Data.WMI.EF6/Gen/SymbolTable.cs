using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees;

namespace System.Data.WMI.EF6.Gen
{
    /// <summary>
    ///     The symbol table is quite primitive - it is a stack with a new entry for
    ///     each scope.  Lookups search from the top of the stack to the bottom, until
    ///     an entry is found.
    ///     The symbols are of the following kinds
    ///     <list type="bullet">
    ///         <item><see cref="Symbol" /> represents tables (extents/nested selects/unnests)</item>
    ///         <item><see cref="JoinSymbol" /> represents Join nodes</item>
    ///         <item><see cref="Symbol" /> columns.</item>
    ///     </list>
    ///     Symbols represent names <see cref="SqlGenerator.Visit(DbVariableReferenceExpression)" /> to be resolved,
    ///     or things to be renamed.
    /// </summary>
    internal sealed class SymbolTable
    {
        private readonly List<Dictionary<string, Symbol>> _symbols = new List<Dictionary<string, Symbol>>();

        internal void EnterScope()
        {
            _symbols.Add(new Dictionary<string, Symbol>(StringComparer.OrdinalIgnoreCase));
        }

        internal void ExitScope()
        {
            _symbols.RemoveAt(_symbols.Count - 1);
        }

        internal void Add(string name, Symbol value)
        {
            _symbols[_symbols.Count - 1][name] = value;
        }

        internal Symbol Lookup(string name)
        {
            for (var i = _symbols.Count - 1; i >= 0; --i)
                if (_symbols[i].ContainsKey(name))
                    return _symbols[i][name];

            return null;
        }
    }
}