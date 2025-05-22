using System.ComponentModel;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using OptixTechTest.Api.v1;
using OptixTechTest.Core.Models;
using OptixTechTest.Domain.Services;
using OptixTechTest.UnitTests.Utilities;

namespace OptixTechTest.UnitTests;

[Collection("Sequential")]
public class MovieEndpointsV1Tests
{
    [Fact]
    public async Task CanSearchMoviesByTitle()
    {
        // Arrange
        await using var context = new MockDbContextFactory().CreateDbContext();
        
        await context.Database.EnsureDeletedAsync();
        await context.Database.MigrateAsync();
        
        await context.AddRangeAsync(MockDbContextFactory.TestMovies);
        await context.SaveChangesAsync();

        var movieService = new MovieService(context);

        // Act
        var result = await MovieEndpointsV1.SearchMovies(movieService, new MovieSearchInput
        {
            Query = "avatar"
        }, CancellationToken.None);
        
        // Assert
        Assert.IsType<Ok<PagedListResult<MovieDto>>>(result);

        Assert.NotNull(result.Value?.Results);
        Assert.NotEmpty(result.Value.Results);
        Assert.Single(result.Value.Results);
        Assert.Equal("Avatar: The Way of Water", result.Value.Results[0].Title);
    }
    
    [Fact]
    public async Task CanLimitResultsPerSearch()
    {
        // Arrange
        await using var context = new MockDbContextFactory().CreateDbContext();
        
        await context.Database.EnsureDeletedAsync();
        await context.Database.MigrateAsync();
        
        await context.AddRangeAsync(MockDbContextFactory.TestMovies);
        await context.SaveChangesAsync();

        var movieService = new MovieService(context);

        // Act
        var result = await MovieEndpointsV1.SearchMovies(movieService, new MovieSearchInput
        {
            Limit = 6
        }, CancellationToken.None);
        
        // Assert
        Assert.IsType<Ok<PagedListResult<MovieDto>>>(result);

        Assert.NotNull(result.Value?.Results);
        Assert.NotEmpty(result.Value.Results);
        Assert.Equal(6, result.Value.Results.Count);
    }
    
    [Fact]
    public async Task CanPageThroughResults()
    {
        // Arrange
        await using var context = new MockDbContextFactory().CreateDbContext();
        
        await context.Database.EnsureDeletedAsync();
        await context.Database.MigrateAsync();
        
        await context.AddRangeAsync(MockDbContextFactory.TestMovies);
        await context.SaveChangesAsync();
        
        var movieService = new MovieService(context);

        // Act
        var firstPageResult = await MovieEndpointsV1.SearchMovies(movieService, new MovieSearchInput
        {
            Limit = 3
        }, CancellationToken.None);
    
        // Assert
        Assert.IsType<Ok<PagedListResult<MovieDto>>>(firstPageResult);
        Assert.NotNull(firstPageResult.Value?.Results);
        Assert.Equal(3, firstPageResult.Value.Results.Count);
    
        // Store the IDs from the first page to verify the second page has different results
        var firstPageIds = firstPageResult.Value.Results.Select(m => m.Id).ToList();
    
        Assert.NotNull(firstPageResult.Value.NextCursor);
        Assert.Equal(3, firstPageResult.Value.NextCursor);
    
        // Act
        var secondPageResult = await MovieEndpointsV1.SearchMovies(movieService, new MovieSearchInput
        {
            Limit = 7,
            Cursor = firstPageResult.Value.NextCursor.Value
        }, CancellationToken.None);
    
        // Assert
        Assert.IsType<Ok<PagedListResult<MovieDto>>>(secondPageResult);
        Assert.NotNull(secondPageResult.Value?.Results);
        Assert.Equal(7, secondPageResult.Value.Results.Count);
        
        var secondPageIds = secondPageResult.Value.Results.Select(m => m.Id).ToList();
        
        // The next cursor should be null since there are no more results
        Assert.Null(secondPageResult.Value.NextCursor);
        
        // Verify the second page has different results than the first page
        Assert.Empty(firstPageIds.Intersect(secondPageIds));
    }

    [Fact]
    public async Task CanFilterByGenre()
    {
        // Arrange
        await using var context = new MockDbContextFactory().CreateDbContext();

        await context.Database.EnsureDeletedAsync();
        await context.Database.MigrateAsync();
        
        await context.AddRangeAsync(MockDbContextFactory.TestMovies);
        await context.SaveChangesAsync();
        
        var movieService = new MovieService(context);

        string[] genres = ["Comedy", "Romance"];
        
        // Act
        var result = await MovieEndpointsV1.SearchMovies(movieService, new MovieSearchInput
        {
            Genres = genres
        }, CancellationToken.None);
        
        // Assert
        Assert.NotNull(result.Value?.Results);
        Assert.NotEmpty(result.Value.Results);
        
        foreach (var movie in result.Value.Results)
        {
            Assert.NotNull(movie.Genres);
            Assert.NotEmpty(movie.Genres);
        
            // Check that all specified genres are present in the movie's genres
            foreach (var genre in genres)
            {
                Assert.Contains(genre, movie.Genres);
            }
        }
    }

    [Fact]
    public async Task CanFilterByActor()
    {
        // Arrange
        await using var context = new MockDbContextFactory().CreateDbContext();

        await context.Database.EnsureDeletedAsync();
        await context.Database.MigrateAsync();
        
        await context.AddRangeAsync(MockDbContextFactory.TestMovies);
        await context.SaveChangesAsync();
        
        var movieService = new MovieService(context);

        string[] actors = ["Leonardo DiCaprio"];
        
        // Act
        var result = await MovieEndpointsV1.SearchMovies(movieService, new MovieSearchInput
        {
            Actors = actors
        }, CancellationToken.None);
        
        // Assert
        Assert.NotNull(result.Value?.Results);
        Assert.NotEmpty(result.Value.Results);
        
        foreach (var movie in result.Value.Results)
        {
            Assert.NotNull(movie.Actors);
            Assert.NotEmpty(movie.Actors);
        
            // Check that all specified actors are present in the movie's actors
            foreach (var actor in actors)
            {
                Assert.Contains(actor, movie.Actors);
            }
        }
    }
    
    [Fact]
    public async Task CanSortMoviesByReleaseDate()
    {
        // Arrange
        await using var context = new MockDbContextFactory().CreateDbContext();
        
        await context.Database.EnsureDeletedAsync();
        await context.Database.MigrateAsync();
        
        await context.AddRangeAsync(MockDbContextFactory.TestMovies);
        await context.SaveChangesAsync();

        var movieService = new MovieService(context);

        // Act
        // Sort by release date in ascending order
        var ascendingResult = await MovieEndpointsV1.SearchMovies(movieService, new MovieSearchInput
        {
            OrderBy = MovieOrderBy.ReleaseDate,
            Direction = ListSortDirection.Ascending
        }, CancellationToken.None);
        
        // Sort by release date in descending order
        var descendingResult = await MovieEndpointsV1.SearchMovies(movieService, new MovieSearchInput
        {
            OrderBy = MovieOrderBy.ReleaseDate,
            Direction = ListSortDirection.Descending
        }, CancellationToken.None);
        
        // Assert
        Assert.IsType<Ok<PagedListResult<MovieDto>>>(ascendingResult);
        Assert.IsType<Ok<PagedListResult<MovieDto>>>(descendingResult);

        Assert.NotNull(ascendingResult.Value?.Results);
        Assert.NotEmpty(ascendingResult.Value.Results);
        
        Assert.NotNull(descendingResult.Value?.Results);
        Assert.NotEmpty(descendingResult.Value.Results);
        
        // Verify ascending order
        for (var i = 1; i < ascendingResult.Value.Results.Count; i++)
        {
            Assert.True(ascendingResult.Value.Results[i-1].ReleaseDate <= ascendingResult.Value.Results[i].ReleaseDate,
                $"Movie at index {i-1} should have release date earlier than or equal to movie at index {i} when sorted in ascending order"
            );
        }
        
        // Verify descending order
        for (var i = 1; i < descendingResult.Value.Results.Count; i++)
        {
            Assert.True(
                descendingResult.Value.Results[i-1].ReleaseDate >= descendingResult.Value.Results[i].ReleaseDate,
                $"Movie at index {i-1} should have release date later than or equal to movie at index {i} when sorted in descending order"
            );
        }
    }
    
    [Fact]
    public async Task CanSortMoviesByTitle()
    {
        // Arrange
        await using var context = new MockDbContextFactory().CreateDbContext();
        
        await context.Database.EnsureDeletedAsync();
        await context.Database.MigrateAsync();
        
        await context.AddRangeAsync(MockDbContextFactory.TestMovies);
        await context.SaveChangesAsync();

        var movieService = new MovieService(context);

        // Act
        // Sort by title in ascending order (A to Z)
        var ascendingResult = await MovieEndpointsV1.SearchMovies(movieService, new MovieSearchInput
        {
            OrderBy = MovieOrderBy.Title,
            Direction = ListSortDirection.Ascending
        }, CancellationToken.None);
        
        // Sort by title in descending order (Z to A)
        var descendingResult = await MovieEndpointsV1.SearchMovies(movieService, new MovieSearchInput
        {
            OrderBy = MovieOrderBy.Title,
            Direction = ListSortDirection.Descending
        }, CancellationToken.None);
        
        // Assert
        Assert.IsType<Ok<PagedListResult<MovieDto>>>(ascendingResult);
        Assert.IsType<Ok<PagedListResult<MovieDto>>>(descendingResult);

        Assert.NotNull(ascendingResult.Value?.Results);
        Assert.NotEmpty(ascendingResult.Value.Results);
        
        Assert.NotNull(descendingResult.Value?.Results);
        Assert.NotEmpty(descendingResult.Value.Results);
        
        // Verify ascending order (A to Z)
        for (var i = 1; i < ascendingResult.Value.Results.Count; i++)
        {
            Assert.True(
                string.Compare(ascendingResult.Value.Results[i-1].Title, ascendingResult.Value.Results[i].Title, StringComparison.OrdinalIgnoreCase) <= 0,
                $"Movie at index {i-1} with title '{ascendingResult.Value.Results[i-1].Title}' should come before or equal to movie at index {i} with title '{ascendingResult.Value.Results[i].Title}' when sorted alphabetically"
            );
        }
        
        // Verify descending order (Z to A)
        for (var i = 1; i < descendingResult.Value.Results.Count; i++)
        {
            Assert.True(
                string.Compare(descendingResult.Value.Results[i-1].Title, descendingResult.Value.Results[i].Title, StringComparison.OrdinalIgnoreCase) >= 0,
                $"Movie at index {i-1} with title '{descendingResult.Value.Results[i-1].Title}' should come after or equal to movie at index {i} with title '{descendingResult.Value.Results[i].Title}' when sorted in reverse alphabetical order"
            );
        }
    }
}
