using System.Collections.Generic;
using System.Collections.Immutable;

namespace MvuSharp.Collections
{
    public sealed class RecordSortedDictionary<TKey, TValue>
        : RecordCollection<ImmutableSortedDictionary<TKey, TValue>, KeyValuePair<TKey, TValue>>
    {
        public static readonly RecordSortedDictionary<TKey, TValue> Empty =
            new(ImmutableSortedDictionary<TKey, TValue>.Empty);

        public RecordSortedDictionary(ImmutableSortedDictionary<TKey, TValue> dictionary)
            : base(dictionary)
        {
        }

        public static implicit operator ImmutableSortedDictionary<TKey, TValue>(
            RecordSortedDictionary<TKey, TValue> record)
        {
            return record.Collection;
        }

        public static explicit operator RecordSortedDictionary<TKey, TValue>(
            ImmutableSortedDictionary<TKey, TValue> dictionary)
        {
            return new(dictionary);
        }
    }
}