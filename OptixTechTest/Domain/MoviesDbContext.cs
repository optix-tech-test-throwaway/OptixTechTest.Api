using Microsoft.EntityFrameworkCore;
using OptixTechTest.Domain.Models;

namespace OptixTechTest.Domain;

public class MoviesDbContext : DbContext
{
    public DbSet<Movie> Movies { get; set; }
    
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
