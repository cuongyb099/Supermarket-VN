using System.Collections.Generic;

namespace KatLib.Pooling
{
    public static class GenericPool<T> where T : class, new()
    {
        private static readonly Stack<T> _pools = new();
        
        public static T Get() => _pools.Count > 0 ? _pools.Pop() : new T();

        public static void Return(T item)
        {
            if(_pools.Contains(item) || item == null) return;
            
            _pools.Push(item);
        }
        
        public static void Clear() => _pools.Clear();

        public static void PrePool(int count)
        {
            for (int i = 0; i < count; i++)
            {
                _pools.Push(new T());
            }
        }
    }
}