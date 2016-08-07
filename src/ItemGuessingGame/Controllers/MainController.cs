using System;
using System.Linq;
using System.Threading.Tasks;
using ItemGuessingGame.Infrastructure;
using ItemGuessingGame.Models;
using ItemGuessingGame.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ItemGuessingGame.Controllers
{
    /// <summary>
    /// Main and only controller for the website.
    /// </summary>
    public sealed class MainController : Controller
    {
        private readonly ItemsList _items;
        private readonly StatisticsContext _statsContext;
        private readonly MainOptions _options;
        private readonly Random _random;


        public MainController( ItemsListAccessor items, StatisticsContext statsContext,
                               IOptions<MainOptions> options, Random random )
        {
            _items = items.Value;
            _statsContext = statsContext;
            _options = options.Value;
            _random = random;
        }


        /// <summary>
        /// Main page, showing an item and asking the user to guess its kind.
        /// </summary>
        [HttpGet( "/" )]
        public IActionResult Index()
        {
            var kind = _random.OneOf( _items.Kinds );
            var item = _random.OneOf( _items.OfKind( kind ) );

            return View( "Index", new GuessData( item.Name, _items.Kinds ) );
        }

        /// <summary>
        /// Guess result page, revealing the solution.
        /// </summary>
        [HttpPost( "/guess" )]
        public async Task<IActionResult> Guess( [FromForm] string itemName, [FromForm] string guessId )
        {
            var item = _items.WithName( itemName );
            if( item == null )
            {
                return Error();
            }

            var isCorrect = item.Kind.Id == guessId;

            var stat = await _statsContext.Statistics.FirstOrDefaultAsync( s => s.ItemName == itemName );
            if( stat == null )
            {
                stat = new ItemStatistics( itemName );
                _statsContext.Add( stat );
            }

            stat.GuessCount++;
            if( isCorrect )
            {
                stat.CorrectGuessCount++;
            }

            await _statsContext.SaveChangesAsync();

            return View( "Result", new GuessResult( isCorrect, item, stat ) );
        }

        /// <summary>
        /// Statistics page.
        /// </summary>
        [HttpGet( "/stats" )]
        public async Task<IActionResult> Statistics()
        {
            var totalGuesses = await _statsContext.Statistics.SumAsync( i => i.GuessCount );
            var totalCorrectGuesses = await _statsContext.Statistics.SumAsync( i => i.CorrectGuessCount );

            // * 1.0 necessary to force the division to return a float
            var mostCorrectlyGuessed = await _statsContext.Statistics.Where( s => s.GuessCount > 0 )
                                                          .OrderByDescending( s => s.CorrectGuessCount * 1.0 / s.GuessCount )
                                                          .ThenBy( s => s.ItemName )
                                                          .Take( _options.StatisticsRankingLength )
                                                          .ToArrayAsync();
            var leastCorrectlyGuessed = await _statsContext.Statistics.Where( s => s.GuessCount > 0 )
                                                           .OrderBy( s => s.CorrectGuessCount * 1.0 / s.GuessCount )
                                                           .ThenBy( s => s.ItemName )
                                                           .Take( _options.StatisticsRankingLength )
                                                           .ToArrayAsync();

            return View( "Statistics", new GlobalStatistics( totalGuesses, totalCorrectGuesses, mostCorrectlyGuessed, leastCorrectlyGuessed ) );
        }

        /// <summary>
        /// Error page.
        /// </summary>
        [HttpGet( "/error" )]
        public IActionResult Error()
        {
            return View( "Error" );
        }
    }
}