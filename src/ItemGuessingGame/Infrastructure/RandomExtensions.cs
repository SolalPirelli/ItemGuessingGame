using System;
using System.Collections.Generic;

namespace ItemGuessingGame.Infrastructure
{
    /// <summary>
    /// Extensions for <see cref="Random" />.
    /// </summary>
    public static class RandomExtensions
    {
        /// <summary>
        /// Randomly pick one of the specified values.
        /// </summary>
        public static T OneOf<T>( this Random random, IReadOnlyList<T> values )
        {
            return values[random.Next( values.Count )];
        }
    }
}