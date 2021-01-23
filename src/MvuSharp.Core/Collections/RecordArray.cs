using System.Collections.Immutable;

namespace MvuSharp.Collections
{
    public sealed class RecordArray<T> : RecordCollection<ImmutableArray<T>, T>
    {
        public static readonly RecordArray<T> Empty = new(ImmutableArray<T>.Empty);

        public RecordArray(ImmutableArray<T> array) : base(array)
        {
        }

        public static implicit operator ImmutableArray<T>(RecordArray<T> record)
        {
            return record.Collection;
        }

        public static explicit operator RecordArray<T>(ImmutableArray<T> array)
        {
            return new(array);
        }
    }
}