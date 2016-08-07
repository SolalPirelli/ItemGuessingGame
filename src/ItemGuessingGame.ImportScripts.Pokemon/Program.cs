using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ItemGuessingGame.ImportScripts.Pokemon
{
    public sealed class Program
    {
        public static void Main( string[] args )
        {
            Console.OutputEncoding = Encoding.UTF8;

            DoWork().Wait();

            Console.WriteLine( "Finished." );
            Console.Read();
        }

        private static async Task DoWork()
        {
            var client = new HttpClient();
            var picturesDir = Directory.CreateDirectory( "img" );
            var items = new Dictionary<string, object>();

            for( int n = 1; n <= 721; n++ )
            {
                // Be nice!
                await Task.Delay( 200 );

                Console.Write( $"{n}..." );

                var json = await GetStringAsyncWithRetry( $"https://pokeapi.co/api/v2/pokemon-species/{n}/", 10 );
                var root = JObject.Parse( json );

                var name = root.Value<JArray>( "names" )
                               .First( IsInEnglish )
                               .Value<string>( "name" );

                // The Bulbagarden name has the nice property that one cannot guess it without knowing about the Pokémon,
                // which is exactly what the website needs, i.e. you can't check for the existence of '/img/thing.png'
                // to know if 'thing' is a Pokémon or not.
                var bulbaName = GetBulbagardenName( n, name );
                var pictureSourceUrl = $"http://archives.bulbagarden.net/wiki/File:{bulbaName}.png";
                var picturePath = picturesDir.Name + "/" + bulbaName + ".png";
                if( !File.Exists( picturePath ) )
                {
                    var picturePage = await client.GetStringAsync( pictureSourceUrl );
                    var pictureUrl = Regex.Match( picturePage, @"fullImageLink.*?<a href=""(.*?)"">" ).Groups[1].Value;

                    using( var pictureFileStream = File.OpenWrite( picturePath ) )
                    {
                        var pictureStream = await client.GetStreamAsync( pictureUrl );
                        await pictureStream.CopyToAsync( pictureFileStream );
                    }
                }

                items.Add( name, new
                {
                    description = root.Value<JArray>( "flavor_text_entries" )
                                      .First( IsInEnglish )
                                      .Value<string>( "flavor_text" )
                                      .Replace( '\n', ' ' ),
                    descriptionSource = "Pokéapi",
                    descriptionSourceUrl = "https://pokeapi.co/",
                    picture = "/" + picturePath,
                    pictureSource = "Bulbagarden",
                    pictureSourceUrl = pictureSourceUrl

                } );

                Console.WriteLine( name );
            }

            File.WriteAllText( "pokemon.json", JsonConvert.SerializeObject( new
            {
                pokemon = new
                {
                    name = "Pokémon",
                    noun = "a Pokémon",
                    items = items
                }
            }, Formatting.Indented ) );
        }

        private static Task<string> GetStringAsyncWithRetry( string url, int maxRetries )
        {
            try
            {
                return new HttpClient().GetStringAsync( url );
            }
            catch
            {
                if( maxRetries == 0 )
                {
                    throw;
                }
                return GetStringAsyncWithRetry( url, maxRetries - 1 );
            }
        }

        private static bool IsInEnglish( JToken token )
        {
            return token["language"]["name"].Value<string>() == "en";
        }

        private static string GetBulbagardenName( int index, string name )
        {
            name = name.Replace( ' ', '_' ) // Not strictly needed, Bulbagarden auto-redirects
                       .TrimEnd( '.' ); // For Mime Jr.

            if( name.StartsWith( "Nidoran" ) )
            {
                name = "Nidoran"; // For Nidoran(m/f)
            }
            if( name == "Giratina" )
            {
                name += "-Origin";
            }
            if( name == "Shaymin" )
            {
                name += "-Land";
            }
            if( name == "Deerling" || name == "Sawsbuck" )
            {
                name += "-Spring";
            }

            return index.ToString().PadLeft( 3, '0' ) + name;
        }
    }
}