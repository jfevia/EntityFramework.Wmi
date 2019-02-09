using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace System.Data.WMI.EF6.Gen
{
    internal sealed class KeyToListMap<TKey, TValue>
    {
        // Fields
        private readonly Dictionary<TKey, List<TValue>> _map;

        // Methods
        internal KeyToListMap(IEqualityComparer<TKey> comparer)
        {
            _map = new Dictionary<TKey, List<TValue>>(comparer);
        }

        // Properties
        internal IEnumerable<TValue> AllValues
        {
            get
            {
                foreach (var key in Keys)
                {
                    var values = ListForKey(key);
                    foreach (var value in values)
                        yield return value;
                }
            }
        }

        internal IEnumerable<TKey> Keys => _map.Keys;

        internal IEnumerable<KeyValuePair<TKey, List<TValue>>> KeyValuePairs => _map;

        internal void Add(TKey key, TValue value)
        {
            if (!_map.TryGetValue(key, out var list))
            {
                list = new List<TValue>();
                _map[key] = list;
            }

            list.Add(value);
        }

        internal void AddRange(TKey key, IEnumerable<TValue> values)
        {
            foreach (var local in values)
                Add(key, local);
        }

        internal bool ContainsKey(TKey key)
        {
            return _map.ContainsKey(key);
        }

        internal IEnumerable<TValue> EnumerateValues(TKey key)
        {
            if (!_map.TryGetValue(key, out var values))
                yield break;

            foreach (var value in values)
                yield return value;
        }

        internal ReadOnlyCollection<TValue> ListForKey(TKey key)
        {
            return new ReadOnlyCollection<TValue>(_map[key]);
        }

        internal bool RemoveKey(TKey key)
        {
            return _map.Remove(key);
        }

        internal void ToCompactString(StringBuilder builder)
        {
            foreach (var local in Keys)
            {
                StringUtil.FormatStringBuilder(builder, "{0}", local);
                builder.Append(": ");
                IEnumerable<TValue> list = ListForKey(local);
                StringUtil.ToSeparatedString(builder, list, ",", "null");
                builder.Append("; ");
            }
        }

        internal bool TryGetListForKey(TKey key, out ReadOnlyCollection<TValue> valueCollection)
        {
            valueCollection = null;
            if (!_map.TryGetValue(key, out var list))
                return false;

            valueCollection = new ReadOnlyCollection<TValue>(list);
            return true;
        }
    }
}