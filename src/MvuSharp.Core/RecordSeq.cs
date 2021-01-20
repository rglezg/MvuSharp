using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MvuSharp
{
    public class RecordSeq<TSeq, T> : IEnumerable<T>, IEquatable<RecordSeq<TSeq, T>>
    where TSeq : IEnumerable<T>
    {
        public TSeq Collection { get; }

        public RecordSeq(TSeq seq)
        {
            Collection = seq;
        }

        public IEnumerator<T> GetEnumerator() => Collection.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        protected virtual bool CollectionEquals(RecordSeq<TSeq, T> other) => 
            Collection.SequenceEqual(other.Collection);

        public bool Equals(RecordSeq<TSeq, T> other) => 
            !ReferenceEquals(null, other) && (ReferenceEquals(this, other) || CollectionEquals(other));

        public override bool Equals(object obj) =>
            !ReferenceEquals(null, obj) && (ReferenceEquals(this, obj) ||
                                            obj.GetType() == GetType() && Equals((RecordSeq<TSeq, T>) obj));

        public override int GetHashCode()
        {
            if (Collection == null) return 0;
            var hash = new HashCode();
            foreach (var item in Collection)
            {
                hash.Add(item.GetHashCode());
            }

            return hash.ToHashCode();
        }

        public static bool operator ==(RecordSeq<TSeq, T> left, RecordSeq<TSeq, T> right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(RecordSeq<TSeq, T> left, RecordSeq<TSeq, T> right)
        {
            return !Equals(left, right);
        }
    }
    
    public class RecordSet<TSet, T> : RecordSeq<TSet, T> where TSet : IEnumerable<T>, ISet<T>
    {
        public RecordSet(TSet seq) : base(seq)
        {
        }

        protected override bool CollectionEquals(RecordSeq<TSet, T> other)
        {
            if (!(other is RecordSet<TSet, T> otherSet)) return false;
            return Collection.SetEquals(otherSet.Collection);
        }
    }

    public class RecordMap<TMap, TKey, TValue> : RecordSeq<TMap, KeyValuePair<TKey, TValue>>
        where TMap : IEnumerable<KeyValuePair<TKey, TValue>>, IReadOnlyDictionary<TKey, TValue>
    {
        public RecordMap(TMap seq) : base(seq)
        {
        }

        protected override bool CollectionEquals(RecordSeq<TMap, KeyValuePair<TKey, TValue>> other)
        {
            if (!(other is RecordMap<TMap, TKey, TValue>)) return false;
            foreach (var (key, value) in Collection)
            {
                if (!other.Collection.TryGetValue(key, out var otherValue) || !value.Equals(otherValue))
                {
                    return false;
                }
            }
            return true;
        }
    }
}