using System.Collections.Generic;
using System.Linq;

namespace Motakim.Utilities
{
    internal static class DictionaryExtensions
    {
        public static IReadOnlyDictionary<TKey, TValue> AsReadOnly<TKey, TValue>(this Dictionary<TKey, TValue> dictionary)
        {
            return new DictionaryWrapper<TKey, TValue>(dictionary);
        }
    }
}
