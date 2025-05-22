using Microsoft.EntityFrameworkCore;
using OptixTechTest.Domain.Models;

namespace OptixTechTest.Domain;

/// <summary>
/// Database context for movie-related data using Entity Framework Core.
/// </summary>
public class MoviesDbContext : DbContext
{
    /// <summary>
    /// Gets or sets the collection of movies in the database.
    /// </summary>
    public DbSet<Movie> Movies { get; set; }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="MoviesDbContext"/> class.
    /// </summary>
    /// <param name="options">The options to be used by the DbContext.</param>
    public MoviesDbContext(DbContextOptions<MoviesDbContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    
        modelBuilder.Entity<Movie>()
            .HasIndex(m => m.Genres)
            .HasMethod("GIN");
        
        modelBuilder.Entity<Movie>()
            .HasIndex(m => m.Actors)
            .HasMethod("GIN");
    }
}
