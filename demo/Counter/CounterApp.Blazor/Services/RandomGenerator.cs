using System;

namespace CounterApp.Blazor.Services
{
    public class RandomGenerator
    {
        private readonly Random _random = new();
        
        public int RandomInt(int min, int max)
        {
            return _random.Next(min, max);
        }
    }
}