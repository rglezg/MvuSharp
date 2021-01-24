using System.Collections.Generic;

namespace MvuSharp.Collections
{
    public class RecordCollection<TCollection, T> : RecordEnumerable<TCollection, T>, IReadOnlyCollection<T>
        where TCollection : IEnumerable<T>, IReadOnlyCollection<T>
    {
        internal RecordCollection(TCollection seq) : base(seq)
        {
        }

        public int Count => Collection.Count;

        protected override bool CollectionEquals(RecordEnumerable<TCollection, T> other)
        {
            if (!(other is RecordCollection<TCollection, T> otherCollection)) return false;
            return Count == otherCollection.Count && base.CollectionEquals(other);
        }
    }
}