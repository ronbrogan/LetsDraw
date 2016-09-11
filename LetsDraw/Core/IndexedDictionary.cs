using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetsDraw.Core
{
    public class IndexedDictionary<TKey, TValue>
    {
        private List<TValue> values = new List<TValue>();
        private Dictionary<TKey, int> indicies = new Dictionary<TKey, int>();

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

            values.Add(value);
            var addedIndex = values.IndexOf(value);
            indicies.Add(key, addedIndex);
            return addedIndex;
        }

        public List<TValue> Values => values;

        public int Count => values.Count;
    }
}
