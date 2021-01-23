using System.Collections.Generic;
using System.Collections.Immutable;

namespace MvuSharp.Collections
{
    public sealed class RecordHashSet<T> : RecordCollection<ImmutableHashSet<T>, T>
    {
        public static readonly RecordHashSet<T> Empty = new(ImmutableHashSet<T>.Empty);

        public RecordHashSet(IEnumerable<T> seq) : base(seq.ToImmutableHashSet())
        {
        }

        protected override bool CollectionEquals(RecordEnumerable<ImmutableHashSet<T>, T> other)
        {
            return Collection.SetEquals(other.Collection);
        }

        public static implicit operator ImmutableHashSet<T>(RecordHashSet<T> record)
        {
            return record.Collection;
        }

        public static explicit operator RecordHashSet<T>(ImmutableHashSet<T> hashSet)
        {
            return new(hashSet);
        }
    }
}