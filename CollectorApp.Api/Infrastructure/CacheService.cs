using System;
using System.Runtime.Caching;
using Serilog;

namespace CollectorApp.Api.Infrastructure
{
    public static class CacheService
    {
        private static readonly ObjectCache _cache = MemoryCache.Default;

        /// <summary>
        /// Pobiera dane z cache lub wykonuje funkcję, jeśli cache jest pusty.
        /// </summary>
        /// <typeparam name="T">Typ zwracanych danych</typeparam>
        /// <param name="cacheKey">Unikalny klucz dla danych</param>
        /// <param name="getDataFunc">Metoda, która pobiera dane (np. z Subiekta)</param>
        /// <param name="durationMinutes">Czas życia cache w minutach</param>
        public static T GetOrSet<T>(string cacheKey, Func<T> getDataFunc, int durationMinutes = 60)
        {
            if (_cache.Get(cacheKey) is T cachedItem)
            {
                Log.Debug("Cache hit dla klucza: {CacheKey}", cacheKey);
                return cachedItem;
            }

            Log.Information("Cache miss dla klucza: {CacheKey}. Pobieranie danych...", cacheKey);

            T data = getDataFunc();

            if (data != null)
            {
                CacheItemPolicy policy = new CacheItemPolicy
                {
                    AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(durationMinutes)
                };
                _cache.Set(cacheKey, data, policy);
            }

            return data;
        }

        public static void Clear(string cacheKey)
        {
            _cache.Remove(cacheKey);
            Log.Debug("Wyczyszczono cache dla klucza: {CacheKey}", cacheKey);
        }
    }
}