using OptixTechTest.Domain;

namespace OptixTechTest.UnitTests.Utilities;

public class DatabaseFixture : IAsyncLifetime
{
    public MoviesDbContext DbContext { get; private set; } = null!;

    public async Task InitializeAsync()
    {
        // Arrange
        var context = new MockDbContextFactory().CreateDbContext();
        
        await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();
        
        await context.AddRangeAsync(MockDbContextFactory.TestMovies);
        await context.SaveChangesAsync();

        DbContext = context;
    }

    public async Task DisposeAsync()
    {
        await DbContext.DisposeAsync();
    }
}
