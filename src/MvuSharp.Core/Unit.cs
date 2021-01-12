using System;

namespace MvuSharp
{
    public struct Unit : IEquatable<Unit>, IComparable<Unit>, IComparable
    {
        public static readonly Unit Value = new();
        
        public static readonly System.Threading.Tasks.Task<Unit> Task = 
            System.Threading.Tasks.Task.FromResult(Value);

        public bool Equals(Unit other) => true;

        public override bool Equals(object obj) => obj is Unit;

        public override int GetHashCode() => 0;

        public static bool operator ==(Unit left, Unit right) => true;

        public static bool operator !=(Unit left, Unit right) => false;

        public int CompareTo(Unit other) => 0;

        public int CompareTo(object obj) => 
            obj is Unit ? 0 : throw new ArgumentException($"Object must be of type {nameof(Unit)}");
    }
}