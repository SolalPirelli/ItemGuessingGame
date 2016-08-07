namespace ItemGuessingGame.Models
{
    /// <summary>
    /// String with an attribution, i.e. a name/url describing where it came from.
    /// </summary>
    public sealed class AttributedString
    {
        /// <summary>
        /// The string value.
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// The name of the source.
        /// </summary>
        public string SourceName { get; }

        /// <summary>
        /// The URL of the source.
        /// </summary>
        public string SourceUrl { get; }



        public AttributedString(string value, string sourceName, string sourceUrl)
        {
            Value = value;
            SourceName = sourceName;
            SourceUrl = sourceUrl;
        }
    }
}