using System;
using System.Collections.Generic;

namespace PseudoLINQ
{
    public sealed class KeyValue<TKey, TValue> : IComparable<KeyValue<TKey, TValue>>
    {
        public TKey Key { get; }

        public TValue Value { get; }

        private IComparer<TKey> _comparer;

        public KeyValue(TKey key, TValue value, IComparer<TKey> comparer = null)
        {
            Key = key;
            Value = value;

            _comparer = comparer ?? Comparer<TKey>.Default;
        }

        public int CompareTo(KeyValue<TKey, TValue> other)
        {
            if (ReferenceEquals(null, other))
            {
                return 1;
            }
            if (ReferenceEquals(this, other))
            {
                return 0;
            }

            return _comparer.Compare(this.Key, other.Key);
        }
    }
}
