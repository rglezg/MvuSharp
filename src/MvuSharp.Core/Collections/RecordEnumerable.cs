using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MvuSharp.Collections
{
    public class RecordEnumerable<TSeq, T> : IEnumerable<T>, IEquatable<RecordEnumerable<TSeq, T>>
        where TSeq : IEnumerable<T>
    {
        private readonly Lazy<int> _hashCode;

        public TSeq Collection { get; }

        internal RecordEnumerable(TSeq seq)
        {
            Collection = seq;
            _hashCode = new Lazy<int>(() =>
            {
                var hash = new HashCode();
                foreach (var item in Collection)
                {
                    hash.Add(item.GetHashCode());
                }

                return hash.ToHashCode();
            });
        }

        public IEnumerator<T> GetEnumerator() => Collection.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        protected virtual bool CollectionEquals(RecordEnumerable<TSeq, T> other) =>
            Collection.SequenceEqual(other.Collection);

        public bool Equals(RecordEnumerable<TSeq, T> other) =>
            !ReferenceEquals(null, other)
            && (ReferenceEquals(this, other)
                || Collection.Count() == other.Collection.Count()
                && CollectionEquals(other));

        public override bool Equals(object obj) =>
            !ReferenceEquals(null, obj) && (ReferenceEquals(this, obj) ||
                                            obj.GetType() == GetType() && Equals((RecordEnumerable<TSeq, T>) obj));

        public override int GetHashCode()
        {
            return _hashCode.Value;
        }

        public static bool operator ==(RecordEnumerable<TSeq, T> left, RecordEnumerable<TSeq, T> right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(RecordEnumerable<TSeq, T> left, RecordEnumerable<TSeq, T> right)
        {
            return !Equals(left, right);
        }
    }
}