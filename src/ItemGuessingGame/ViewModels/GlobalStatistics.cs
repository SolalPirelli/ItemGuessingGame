using System.Collections.Generic;
using ItemGuessingGame.Models;

namespace ItemGuessingGame.ViewModels
{
    /// <summary>
    /// Global statistics information.
    /// </summary>
    public sealed class GlobalStatistics
    {
        /// <summary>
        /// The total number of guesses.
        /// </summary>
        public int TotalGuesses { get; }

        /// <summary>
        /// The number of correct guesses.
        /// </summary>
        public int TotalCorrectGuesses { get; }

        /// <summary>
        /// Most correctly guessed items, in order.
        /// </summary>
        public IReadOnlyList<ItemStatistics> MostCorrectlyGuessed { get; }

        /// <summary>
        /// Least correctly guessed items, in order.
        /// </summary>
        public IReadOnlyList<ItemStatistics> LeastCorrectlyGuessed { get; }


        public GlobalStatistics( int totalGuesses,
                               int totalCorrectGuesses,
                               IReadOnlyList<ItemStatistics> mostCorrectlyGuessed,
                               IReadOnlyList<ItemStatistics> leastCorrectlyGuessed )
        {
            TotalGuesses = totalGuesses;
            TotalCorrectGuesses = totalCorrectGuesses;
            MostCorrectlyGuessed = mostCorrectlyGuessed;
            LeastCorrectlyGuessed = leastCorrectlyGuessed;
        }
    }
}