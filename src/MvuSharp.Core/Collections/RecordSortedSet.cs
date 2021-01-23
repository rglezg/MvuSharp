using System.Collections.Immutable;

namespace MvuSharp.Collections
{
    public sealed class RecordSortedSet<T> : RecordCollection<ImmutableSortedSet<T>, T>
    {
        public static readonly RecordSortedSet<T> Empty = new(ImmutableSortedSet<T>.Empty);

        public RecordSortedSet(ImmutableSortedSet<T> set) : base(set)
        {
        }

        public static implicit operator ImmutableSortedSet<T>(RecordSortedSet<T> record)
        {
            return record.Collection;
        }

        public static explicit operator RecordSortedSet<T>(ImmutableSortedSet<T> SortedSet)
        {
            return new(SortedSet);
        }
    }
}