namespace ItemGuessingGame.Models
{
    /// <summary>
    /// Possible item kinds.
    /// </summary>
    public sealed class ItemKind
    {
        /// <summary>
        /// ID of the kind, not for display.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Name of the kind, to display alone.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Noun for the kind, to display in a sentence such as "The item is {kind noun}".
        /// </summary>
        public string Noun { get; }


        public ItemKind( string id, string name, string noun )
        {
            Id = id;
            Name = name;
            Noun = noun;
        }
    }
}