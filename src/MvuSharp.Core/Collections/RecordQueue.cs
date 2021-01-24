using System.Collections.Immutable;

namespace MvuSharp.Collections
{
    public sealed class RecordQueue<T> : RecordEnumerable<ImmutableQueue<T>, T>
    {
        public static readonly RecordQueue<T> Empty = new(ImmutableQueue<T>.Empty);

        public bool IsEmpty => Collection.IsEmpty;

        public RecordQueue() : base(ImmutableQueue<T>.Empty)
        {
        }

        public RecordQueue(ImmutableQueue<T> queue) : base(queue)
        {
        }

        public static implicit operator ImmutableQueue<T>(RecordQueue<T> record)
        {
            return record.Collection;
        }

        public static implicit operator RecordQueue<T>(ImmutableQueue<T> queue)
        {
            return new(queue);
        }
    }
}