using Microsoft.EntityFrameworkCore;
using OptixTechTest.Domain;
using OptixTechTest.Domain.Models;

namespace OptixTechTest.UnitTests.Utilities;

public class MockDbContextFactory : IDbContextFactory<MoviesDbContext>
{
    public MoviesDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<MoviesDbContext>()
            .UseNpgsql("User id=postgres;Password=postgres;Server=127.0.0.1;Port=5433;Database=TestMovieDb;Pooling=true;")
            .Options;
        
        return new MoviesDbContext(options);
    }
    
    public static List<Movie> TestMovies { get; } =
    [
        new()
        {
            Id = Guid.NewGuid(),
            ReleaseDate = new DateOnly(2023, 7, 21),
            Title = "Oppenheimer",
            Overview = "The story of American scientist J. Robert Oppenheimer and his role in the development of the atomic bomb.",
            Popularity = 94.6m,
            VoteCount = 8532u,
            VoteAverage = 8.2m,
            OriginalLanguage = "en",
            Genres = ["Biography", "Drama", "History"],
            Actors = ["Cillian Murphy", "Emily Blunt", "Matt Damon", "Robert Downey Jr."],
            PosterUrl = "https://image.tmdb.org/t/p/w500/oppenheimer_poster.jpg"
        },
        new()
        {
            Id = Guid.NewGuid(),
            ReleaseDate = new DateOnly(2023, 3, 17),
            Title = "Amélie",
            Overview = "Une jeune femme, Amélie Poulain, décide d'aider les gens qui l'entourent et part à la recherche de l'amour.",
            Popularity = 78.3m,
            VoteCount = 12034u,
            VoteAverage = 8.4m,
            OriginalLanguage = "fr",
            Genres = ["Comedy", "Romance"],
            Actors = ["Audrey Tautou", "Mathieu Kassovitz", "Rufus", "Yolande Moreau"],
            PosterUrl = "https://image.tmdb.org/t/p/w500/amelie_poster.jpg"
        },
        new()
        {
            Id = Guid.NewGuid(),
            ReleaseDate = new DateOnly(2019, 5, 30),
            Title = "Parasite",
            Overview =
                "Greed and class discrimination threaten the newly formed symbiotic relationship between the wealthy Park family and the destitute Kim clan.",
            Popularity = 88.7m,
            VoteCount = 15762u,
            VoteAverage = 8.5m,
            OriginalLanguage = "ko",
            Genres = ["Drama", "Thriller", "Comedy"],
            Actors = ["Song Kang-ho", "Lee Sun-kyun", "Cho Yeo-jeong", "Choi Woo-shik"],
            PosterUrl = "https://image.tmdb.org/t/p/w500/parasite_poster.jpg"
        },
        new()
        {
            Id = Guid.NewGuid(),
            ReleaseDate = new DateOnly(2010, 7, 16),
            Title = "Inception",
            Overview = "A thief who steals corporate secrets through the use of dream-sharing technology is given the inverse task of planting an idea into the mind of a C.E.O.",
            Popularity = 83.2m,
            VoteCount = 24981u,
            VoteAverage = 8.3m,
            OriginalLanguage = "en",
            Genres = ["Action", "Sci-Fi", "Thriller"],
            Actors = ["Leonardo DiCaprio", "Joseph Gordon-Levitt", "Ellen Page", "Tom Hardy"],
            PosterUrl = "https://image.tmdb.org/t/p/w500/inception_poster.jpg"
        },
        new()
        {
            Id = Guid.NewGuid(),
            ReleaseDate = new DateOnly(2001, 4, 11),
            Title = "Le Fabuleux Destin d'Amélie Poulain",
            Overview = "Amélie is an innocent and naive girl in Paris with her own sense of justice. She decides to help those around her and, along the way, discovers love.",
            Popularity = 67.5m,
            VoteCount = 9876u,
            VoteAverage = 8.1m,
            OriginalLanguage = "fr",
            Genres = ["Comedy", "Romance"],
            Actors = ["Audrey Tautou", "Mathieu Kassovitz", "Rufus", "Lorella Cravotta"],
            PosterUrl = "https://image.tmdb.org/t/p/w500/amelie_poulain_poster.jpg"
        },
        new()
        {
            Id = Guid.NewGuid(),
            ReleaseDate = new DateOnly(2022, 12, 16),
            Title = "Avatar: The Way of Water",
            Overview =
                "Jake Sully lives with his newfound family formed on the extrasolar moon Pandora. Once a familiar threat returns to finish what was previously started, Jake must work with Neytiri and the army of the Na'vi race to protect their home.",
            Popularity = 92.1m,
            VoteCount = 18543u,
            VoteAverage = 7.8m,
            OriginalLanguage = "en",
            Genres = ["Action", "Adventure", "Sci-Fi"],
            Actors = ["Sam Worthington", "Zoe Saldana", "Sigourney Weaver", "Kate Winslet"],
            PosterUrl = "https://image.tmdb.org/t/p/w500/avatar_way_of_water_poster.jpg"
        },
        new()
        {
            Id = Guid.NewGuid(),
            ReleaseDate = new DateOnly(2023, 10, 20),
            Title = "Dune: Part Two",
            Overview = "Paul Atreides unites with Chani and the Fremen while seeking revenge against the conspirators who destroyed his family.",
            Popularity = 91.3m,
            VoteCount = 7652u,
            VoteAverage = 8.5m,
            OriginalLanguage = "en",
            Genres = ["Sci-Fi", "Adventure", "Drama"],
            Actors = ["Timothée Chalamet", "Zendaya", "Rebecca Ferguson", "Javier Bardem"],
            PosterUrl = "https://image.tmdb.org/t/p/w500/dune_part_two_poster.jpg"
        },
        new()
        {
            Id = Guid.NewGuid(),
            ReleaseDate = new DateOnly(2016, 6, 3),
            Title = "Your Name",
            Overview = "Two strangers find themselves linked in a bizarre way. When a connection forms, will distance be the only thing to keep them apart?",
            Popularity = 76.4m,
            VoteCount = 10321u,
            VoteAverage = 8.4m,
            OriginalLanguage = "ja",
            Genres = ["Animation", "Drama", "Fantasy", "Romance"],
            Actors = ["Ryunosuke Kamiki", "Mone Kamishiraishi", "Ryo Narita", "Aoi Yuki"],
            PosterUrl = "https://image.tmdb.org/t/p/w500/your_name_poster.jpg"
        },
        new()
        {
            Id = Guid.NewGuid(),
            ReleaseDate = new DateOnly(2024, 3, 15),
            Title = "Poor Things",
            Overview = "The incredible tale about the fantastical evolution of Bella Baxter, a young woman brought back to life by the brilliant and unorthodox scientist Dr. Godwin Baxter.",
            Popularity = 82.9m,
            VoteCount = 6421u,
            VoteAverage = 7.9m,
            OriginalLanguage = "en",
            Genres = ["Comedy", "Drama", "Romance", "Sci-Fi"],
            Actors = ["Emma Stone", "Mark Ruffalo", "Willem Dafoe", "Ramy Youssef"],
            PosterUrl = "https://image.tmdb.org/t/p/w500/poor_things_poster.jpg"
        },
        new()
        {
            Id = Guid.NewGuid(),
            ReleaseDate = new DateOnly(2021, 8, 13),
            Title = "Intouchables",
            Overview = "Après un accident de parapente, Philippe, riche aristocrate, engage comme aide à domicile Driss, un jeune de banlieue tout juste sorti de prison. Bref la personne la moins adaptée pour le job.",
            Popularity = 72.8m,
            VoteCount = 14325u,
            VoteAverage = 8.3m,
            OriginalLanguage = "fr",
            Genres = ["Comedy", "Drama"],
            Actors = ["François Cluzet", "Omar Sy", "Anne Le Ny", "Audrey Fleurot"],
            PosterUrl = "https://image.tmdb.org/t/p/w500/intouchables_poster.jpg"
        }
    ];
}
