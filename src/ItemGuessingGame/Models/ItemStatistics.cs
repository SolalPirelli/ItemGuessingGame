using System;
using System.ComponentModel.DataAnnotations;

namespace ItemGuessingGame.Models
{
    /// <summary>
    /// Statistics about an item.
    /// </summary>
    /// <remarks>
    /// This class is stored in a database.
    /// </remarks>
    public class ItemStatistics
    {
        /// <summary>
        /// Name of the item the statistics are about.
        /// </summary>
        [Key]
        public string ItemName { get; private set; }

        /// <summary>
        /// Total number of guesses for the item.
        /// </summary>
        public int GuessCount { get; set; }

        /// <summary>
        /// Number of correct guesses for the item.
        /// </summary>
        public int CorrectGuessCount { get; set; }

        /// <summary>
        /// Percentage of correct guesses for the item, rounded to two decimals.
        /// </summary>
        public double CorrectGuessPercentage
        {
            get
            {
                if( GuessCount == 0 )
                {
                    return 0;
                }

                return Math.Round( 100 * ( (double) CorrectGuessCount / GuessCount ), 2 );
            }
        }


        public ItemStatistics( string itemName )
        {
            ItemName = itemName;
        }

        private ItemStatistics()
        {
            // For serialization
        }
    }
}
