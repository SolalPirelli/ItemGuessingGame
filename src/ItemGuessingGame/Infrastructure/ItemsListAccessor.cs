using System.Collections.Generic;
using ItemGuessingGame.Models;
using Microsoft.Extensions.Configuration;

namespace ItemGuessingGame.Infrastructure
{
    /// <summary>
    /// Loads an <see cref="ItemsList" /> from config.
    /// </summary>
    /// <remarks>
    /// This is a hack to benefit from ASP.NET Core's JSON parsing and file watching.
    /// </remarks>
    public sealed class ItemsListAccessor
    {
        private readonly IConfiguration _config;


        public ItemsList Value { get; private set; }


        public ItemsListAccessor( IConfiguration config )
        {
            _config = config;

            Reload();
        }

        /// <summary>
        /// Reloads all items from the configuration.
        /// </summary>
        private void Reload()
        {
            // First, build a set of items that are in multiple categories and ignore them.
            var known = new HashSet<string>();
            var ignored = new HashSet<string>();
            foreach( var kindSection in _config.GetChildren() )
            {
                foreach( var itemSection in kindSection.GetSection( "items" ).GetChildren() )
                {
                    if( !known.Add( itemSection.Key ) )
                    {
                        ignored.Add( itemSection.Key );
                    }
                }
            }

            var byName = new Dictionary<string, Item>();
            var byKind = new Dictionary<ItemKind, List<Item>>();

            foreach( var kindSection in _config.GetChildren() )
            {
                var kind = new ItemKind(
                    kindSection.Key,
                    kindSection["name"],
                    kindSection["noun"]
                );
                byKind.Add( kind, new List<Item>() );

                foreach( var itemSection in kindSection.GetSection( "items" ).GetChildren() )
                {
                    if( ignored.Contains( itemSection.Key ) )
                    {
                        continue;
                    }

                    var item = new Item(
                        itemSection.Key,
                        kind,
                        string.IsNullOrEmpty( itemSection["picture"] ) ? null : new AttributedString(
                            itemSection["picture"],
                            itemSection["pictureSource"],
                            itemSection["pictureSourceUrl"]
                        ),
                        string.IsNullOrEmpty( itemSection["description"] ) ? null : new AttributedString(
                            itemSection["description"],
                            itemSection["descriptionSource"],
                            itemSection["descriptionSourceUrl"]
                        )
                    );

                    byName.Add( item.Name, item );
                    byKind[kind].Add( item );
                }
            }

            Value = new ItemsList( byName, byKind );

            _config.GetReloadToken().RegisterChangeCallback( _ => Reload(), null );
        }
    }
}