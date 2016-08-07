using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace ItemGuessingGame.ImportScripts.ItemTransform
{
    public class Program
    {
        public static void Main( string[] args )
        {
            var path = @"../ItemGuessingGame/Items/file.json";
            var json = File.ReadAllText( path );
            var root = JObject.Parse( json );

            // Do stuff here, e.g. removing items whose name contains a number
            var items = root.Value<JObject>( "things" ).Value<JObject>( "items" );
            foreach( var prop in items.Properties().ToArray() )
            {
                if( prop.Name.Any( char.IsDigit ) )
                {
                    prop.Remove();
                }
            }

            File.WriteAllText( path, root.ToString() );
        }
    }
}