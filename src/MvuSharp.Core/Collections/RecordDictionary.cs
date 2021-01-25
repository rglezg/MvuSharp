using System.Collections.Generic;
using System.Collections.Immutable;

namespace MvuSharp.Collections
{
    public sealed class RecordDictionary<TKey, TValue>
        : RecordCollection<ImmutableDictionary<TKey, TValue>, KeyValuePair<TKey, TValue>>
    {
        public static readonly RecordDictionary<TKey, TValue> Empty =
            new(ImmutableDictionary<TKey, TValue>.Empty);

        public RecordDictionary(ImmutableDictionary<TKey, TValue> dictionary)
            : base(dictionary)
        {
        }

        protected override bool CollectionEquals(
            RecordEnumerable<ImmutableDictionary<TKey, TValue>, KeyValuePair<TKey, TValue>> other)
        {
            if (Collection.Count != other.Collection.Count) return false;
            foreach (var (key, value) in other.Collection)
            {
                if (Collection.TryGetValue(key, out var selfValue) && Equals(selfValue, value)) continue;
                return false;
            }

            return true;
        }

        public static implicit operator ImmutableDictionary<TKey, TValue>(RecordDictionary<TKey, TValue> record)
        {
            return record.Collection;
        }

        public static implicit operator RecordDictionary<TKey, TValue>(ImmutableDictionary<TKey, TValue> dictionary)
        {
            return new(dictionary);
        }
    }
}