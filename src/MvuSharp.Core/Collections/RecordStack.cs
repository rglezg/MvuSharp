using System.Collections.Immutable;

namespace MvuSharp.Collections
{
    public sealed class RecordStack<T> : RecordEnumerable<ImmutableStack<T>, T>
    {
        public static readonly RecordStack<T> Empty = new(ImmutableStack<T>.Empty);

        public bool IsEmpty => Collection.IsEmpty;

        public RecordStack() : base(ImmutableStack<T>.Empty)
        {
        }

        public RecordStack(ImmutableStack<T> stack) : base(stack)
        {
        }

        public static implicit operator ImmutableStack<T>(RecordStack<T> record)
        {
            return record.Collection;
        }

        public static explicit operator RecordStack<T>(ImmutableStack<T> stack)
        {
            return new(stack);
        }
    }
}