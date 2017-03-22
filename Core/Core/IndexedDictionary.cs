using System.Collections.Generic;

namespace Core.Core
{
    public class IndexedDictionary<TKey, TValue>
    {
        private List<TValue> values;
        private Dictionary<TKey, int> indicies;

        public IndexedDictionary()
        {
            values = new List<TValue>();
            indicies = new Dictionary<TKey, int>();
        }

        public IndexedDictionary(int capacity)
        {
            values = new List<TValue>(capacity);
            indicies = new Dictionary<TKey, int>(capacity);
        }

        public bool ContainsKey(TKey key)
        {
            return indicies.ContainsKey(key);
        }

        public TValue this[TKey key] => values[indicies[key]];

        public TValue this[int index] => values[index];

        public int Add(TKey key, TValue value)
        {
            if (indicies.ContainsKey(key))
            {
                return indicies[key];
            }

            var current = values.Count;
            values.Insert(current, value);
            indicies.Add(key, current);
            return current;
        }

        public List<TValue> Values => values;

        public int Count => values.Count;
    }
}
