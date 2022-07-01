using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Motakim.Utilities
{
    internal sealed class DictionaryWrapper<TKey, TValue> : IReadOnlyDictionary<TKey, TValue>
    {
        private Dictionary<TKey, TValue> _Dictionary;

        internal DictionaryWrapper(Dictionary<TKey, TValue> dictionary)
        {
            _Dictionary = dictionary;
        }

        public TValue this[TKey key] => _Dictionary[key];

        public IEnumerable<TKey> Keys => _Dictionary.Keys;

        public IEnumerable<TValue> Values => _Dictionary.Values;

        public int Count => _Dictionary.Count;

        public bool ContainsKey(TKey key) => _Dictionary.ContainsKey(key);

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => _Dictionary.GetEnumerator();

        public bool TryGetValue(TKey key, out TValue value) => _Dictionary.TryGetValue(key, out value);

        IEnumerator IEnumerable.GetEnumerator() => _Dictionary.GetEnumerator();
    }
}
