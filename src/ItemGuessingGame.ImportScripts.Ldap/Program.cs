using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Novell.Directory.Ldap;

namespace ItemGuessingGame.ImportScripts.Ldap
{
    public class Program
    {
        public static void Main( string[] args )
        {
            var host = "ldap.example.org";
            var port = 389;
            var searchBase = "o=example,c=org";
            var searchFilter = "(!(name=John Doe))";
            var nameAttribute = "name";
            Func<string, string> transformer = name =>
            {
                if( name.Length > 8 )
                {
                    // Ignore the item
                    return null;
                }

                return name;
            };

            var names = new HashSet<string>();

            var conn = new LdapConnection(); // its Dispose() never returns for some reason
            conn.Connect( host, port );

            var fullFilter = $"(&{searchFilter}({nameAttribute}=*))";
            var constraints = new LdapSearchConstraints
            (
                msLimit: 0, // Must be 0 otherwise weird things happen :|
                serverTimeLimit: 0, // No time limit for the server itself
                dereference: LdapSearchConstraints.DEREF_NEVER, // Don't follow aliases
                maxResults: 0, // No limit on results
                doReferrals: false, // Ignored for async ops
                batchSize: 1000, // Use batches
                handler: null, // Ignored for async ops
                hop_limit: 10 // Ignored for async ops
            );
            var resultsQueue = conn.Search( searchBase, LdapConnection.SCOPE_SUB, fullFilter, new[] { nameAttribute }, false, null, constraints );

            int count = 0;
            LdapMessage message;
            while( ( message = resultsQueue.getResponse() ) != null )
            {
                var result = message as LdapSearchResult;
                if( result == null )
                {
                    continue;
                }

                foreach( var name in result.Entry.getAttribute( nameAttribute ).StringValueArray )
                {
                    var transformed = transformer( name );
                    if( transformed == null )
                    {
                        Console.WriteLine( $"Ignored: {name}" );
                    }
                    else
                    {
                        Console.WriteLine( $"{count}: {transformed} (original: {name})" );
                        names.Add( transformed );
                    }
                }

                count++;
            }

            File.WriteAllText( "people.json", JsonConvert.SerializeObject( new
            {
                people = new
                {
                    name = "Person",
                    noun = "a person",
                    items = names.ToDictionary( n => n, n => new { description = (string) null, picture = (string) null } )
                }
            } ) );

            Console.WriteLine( "Finished." );
            Console.Read();
        }
    }
}