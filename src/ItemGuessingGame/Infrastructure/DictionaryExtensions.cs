using System.Collections.Generic;

namespace ItemGuessingGame.Infrastructure
{
    /// <summary>
    /// Extensions for <see cref="IDictionary{TKey, TValue}" />.
    /// </summary>
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Get the value associated with the specified key, or the default value if the key does not exist.
        /// </summary>
        public static TValue GetValueOrDefault<TKey, TValue>( this IDictionary<TKey, TValue> dic, TKey key )
        {
            TValue value;
            dic.TryGetValue( key, out value );
            return value;
        }
    }
}