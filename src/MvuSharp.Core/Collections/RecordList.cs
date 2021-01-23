using System.Collections.Immutable;

namespace MvuSharp.Collections
{
    public sealed class RecordList<T> : RecordCollection<ImmutableList<T>, T>
    {
        public static readonly RecordList<T> Empty = new(ImmutableList<T>.Empty);

        public RecordList(ImmutableList<T> list) : base(list)
        {
        }

        public static implicit operator ImmutableList<T>(RecordList<T> record)
        {
            return record.Collection;
        }

        public static implicit operator RecordList<T>(ImmutableList<T> list)
        {
            return new(list);
        }
    }
}