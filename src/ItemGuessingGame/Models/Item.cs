namespace ItemGuessingGame.Models
{
    /// <summary>
    /// Item whose kind users have to guess.
    /// </summary>
    public sealed class Item
    {
        /// <summary>
        /// Name of the item, which also serves as its ID since names must be unique.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Kind of the item.
        /// </summary>
        public ItemKind Kind { get; }

        /// <summary>
        /// Picture of the item, if available.
        /// </summary>
        public AttributedString PictureUrl { get; }
        
        /// <summary>
        /// Description of the item, if available.
        /// </summary>
        public AttributedString Description { get; }
        

        public Item( string name, ItemKind kind, AttributedString pictureUrl, AttributedString description )
        {
            Name = name;
            Kind = kind;
            PictureUrl = pictureUrl;
            Description = description;
        }
    }
}