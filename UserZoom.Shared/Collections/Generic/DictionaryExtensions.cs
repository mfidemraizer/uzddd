using System.Collections.Generic;

namespace UserZoom.Shared.Collections.Generic
{
    public static class DictionaryExtensions
    {
        public static void AddRange<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, IDictionary<TKey, TValue> dictionaryToAdd)
        {
            foreach(KeyValuePair<TKey, TValue> pair in dictionaryToAdd)
            {
                dictionary.Add(pair.Key, pair.Value);
            }
        }
    }
}
