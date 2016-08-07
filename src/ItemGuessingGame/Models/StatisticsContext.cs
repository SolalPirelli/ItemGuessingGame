using Microsoft.EntityFrameworkCore;

namespace ItemGuessingGame.Models
{
    /// <summary>
    /// Entity Framework context for <see cref="ItemStatistics" />.
    /// </summary>
    public sealed class StatisticsContext : DbContext
    {
        public DbSet<ItemStatistics> Statistics { get; private set; }


        public StatisticsContext( DbContextOptions<StatisticsContext> options ) : base( options )
        {
            Database.EnsureCreated();
        }
    }
}