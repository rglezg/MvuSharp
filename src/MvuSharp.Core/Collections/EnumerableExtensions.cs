using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace MvuSharp.Collections
{
    public static class EnumerableExtensions
    {
        public static RecordEnumerable<TSeq, T> ToRecordSeq<TSeq, T>(this TSeq seq)
            where TSeq : IEnumerable<T> =>
            new(seq);

        public static RecordArray<T> ToRecordArray<T>(this IEnumerable<T> source) =>
            new(source.ToImmutableArray());
        
        public static RecordList<T> ToRecordList<T>(this IEnumerable<T> source) =>
            new(source.ToImmutableList());

        public static RecordHashSet<T> ToRecordHashSet<T>(this IEnumerable<T> source) =>
            new(source);

        public static RecordSortedSet<T> ToRecordSortedSet<T>(this IEnumerable<T> source) =>
            new(source.ToImmutableSortedSet());

        public static RecordSortedDictionary<TKey, TValue> ToRecordSortedDictionary<TKey, TValue>(
            this IEnumerable<KeyValuePair<TKey, TValue>> source) =>
            new (source.ToImmutableSortedDictionary());
        
        public static RecordSortedDictionary<TKey, TValue> ToRecordSortedDictionary<T, TKey, TValue>(
            this IEnumerable<T> source, Func<T, TKey> selectKey, Func<T, TValue> selectValue) =>
            new (source.Select(e => 
                new KeyValuePair<TKey, TValue>(selectKey(e), selectValue(e))).ToImmutableSortedDictionary());
        
        public static RecordDictionary<TKey, TValue> ToRecordDictionary<TKey, TValue>(
            this IEnumerable<KeyValuePair<TKey, TValue>> source) =>
            new(source.ToImmutableDictionary());
        
        public static RecordDictionary<TKey, TValue> ToRecordDictionary<T, TKey, TValue>(
            this IEnumerable<T> source, Func<T, TKey> selectKey, Func<T, TValue> selectValue) =>
            new (source.Select(e => 
                new KeyValuePair<TKey, TValue>(selectKey(e), selectValue(e))).ToImmutableDictionary());
    }
}