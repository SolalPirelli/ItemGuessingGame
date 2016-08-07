using System;
using System.IO;
using ItemGuessingGame.Controllers;
using ItemGuessingGame.Infrastructure;
using ItemGuessingGame.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ItemGuessingGame
{
    public sealed class Program
    {
        public static void Main( string[] args )
        {
            new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot( Directory.GetCurrentDirectory() )
                .UseIISIntegration()
                .UseStartup<Program>()
                .Build()
                .Run();
        }


        public IConfigurationRoot Configuration { get; }

        public IConfigurationRoot ItemsConfiguration { get; }


        public Program( IHostingEnvironment env )
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath( env.ContentRootPath )
                .AddJsonFile( "settings.json" )
                .AddEnvironmentVariables()
                .Build();

            // This makes it easier to have separate files for separate kinds of items,
            // so that scripts which output a file don't have to worry about other items.
            var builder = new ConfigurationBuilder()
                .SetBasePath( env.ContentRootPath );
            foreach( var file in env.ContentRootFileProvider.GetDirectoryContents( "Items" ) )
            {
                builder.AddJsonFile( file.PhysicalPath, optional: false, reloadOnChange: true );
            }

            ItemsConfiguration = builder.Build();
        }


        public void ConfigureServices( IServiceCollection services )
        {
            services.AddMvc();

            services.AddDbContext<StatisticsContext>( options => options.UseSqlite( Configuration.GetConnectionString( "sqlite" ) ) );

            services.Configure<MainOptions>( Configuration.GetSection( "Options" ) );

            services.AddSingleton( new Random() );
            services.AddSingleton( new ItemsListAccessor( ItemsConfiguration ) );
        }

        public void Configure( IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory )
        {
            loggerFactory.AddConsole( LogLevel.Information, false );

            app.UseMvc()
               .UseStaticFiles()
               .UseExceptionHandler( "/error" );
        }
    }
}