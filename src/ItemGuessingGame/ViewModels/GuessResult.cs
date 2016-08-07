using ItemGuessingGame.Models;

namespace ItemGuessingGame.ViewModels
{
    /// <summary>
    /// Result of an user's guess.
    /// </summary>
    public sealed class GuessResult
    {
        /// <summary>
        /// Whether the guess was correct.
        /// </summary>
        public bool IsCorrect { get; }

        /// <summary>
        /// The item.
        /// </summary>
        public Item Item { get; }

        /// <summary>
        /// Statistics about the item.
        /// </summary>
        public ItemStatistics Statistics { get; }


        public GuessResult( bool isCorrect, Item item, ItemStatistics statistics )
        {
            IsCorrect = isCorrect;
            Item = item;
            Statistics = statistics;
        }
    }
}