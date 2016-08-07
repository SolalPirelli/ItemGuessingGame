using System.Collections.Generic;
using ItemGuessingGame.Models;

namespace ItemGuessingGame.ViewModels
{
    /// <summary>
    /// Provided data for an user to guess what an item is.
    /// </summary>
    public sealed class GuessData
    {
        /// <summary>
        /// Name of the item.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Kinds the item may be of.
        /// </summary>
        public IReadOnlyList<ItemKind> PossibleKinds { get; }


        public GuessData( string name, IReadOnlyList<ItemKind> possibleKinds )
        {
            Name = name;
            PossibleKinds = possibleKinds;
        }
    }
}