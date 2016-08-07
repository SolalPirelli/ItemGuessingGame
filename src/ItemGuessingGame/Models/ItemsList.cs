using System.Collections.Generic;
using System.Linq;
using ItemGuessingGame.Infrastructure;

namespace ItemGuessingGame.Models
{
    /// <summary>
    /// List of <see cref="Item" />.
    /// </summary>
    public sealed class ItemsList
    {
        private readonly Dictionary<string, Item> _byName;
        private readonly Dictionary<ItemKind, List<Item>> _byKind;


        /// <summary>
        /// All available kinds.
        /// </summary>
        public IReadOnlyList<ItemKind> Kinds { get; }


        public ItemsList( Dictionary<string, Item> byName, Dictionary<ItemKind, List<Item>> byKind )
        {
            _byName = byName;
            _byKind = byKind;

            Kinds = _byKind.Keys.ToArray();
        }


        /// <summary>
        /// Gets all items of the specified kind.
        /// </summary>
        public IReadOnlyList<Item> OfKind( ItemKind kind )
        {
            return _byKind.GetValueOrDefault( kind );
        }

        /// <summary>
        /// Gets the item with the specified name.
        /// </summary>
        public Item WithName( string name )
        {
            return _byName.GetValueOrDefault( name );
        }
    }
}