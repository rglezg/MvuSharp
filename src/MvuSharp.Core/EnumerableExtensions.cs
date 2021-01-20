using System.Collections.Generic;

namespace MvuSharp
{
    public static class EnumerableExtensions
    {
        public static RecordSeq<TSeq, T> ToRecordSeq<TSeq, T>(this TSeq seq)
            where TSeq : IEnumerable<T> =>
            new(seq);
        
        public static RecordSet<TSet, T> ToRecordSet<TSet, T>(this TSet set)
            where TSet : ISet<T> =>
            new(set);
        
        public static RecordMap<TMap, TKey, TValue> ToRecordMap<TMap, TKey, TValue>(this TMap map)
            where TMap : IReadOnlyDictionary<TKey, TValue> =>
            new(map);
    }
}