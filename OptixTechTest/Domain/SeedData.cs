using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Bogus;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.EntityFrameworkCore;
using OptixTechTest.Domain.Models;

namespace OptixTechTest.Domain;

public static partial class SeedData
{
    public static void Seed(DbContext context)
    {
        var any = context
            .Set<Movie>()
            .Any();

        if (any)
        {
            return;
        }
        
        var movies = ReadMoviesFromCsv();
        
        context.AddRange(movies);
        context.SaveChanges();
    }

    public static async Task SeedAsync(DbContext context, CancellationToken cancellationToken = default)
    {
        var any = await context
            .Set<Movie>()
            .AnyAsync(cancellationToken);
        
        if (any)
        {
            return;
        }
        
        var movies = ReadMoviesFromCsv();
        
        await context.AddRangeAsync(movies, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }
    
    private static IEnumerable<Movie> ReadMoviesFromCsv()
    {
        var csvPath = Path.Combine(Directory.GetCurrentDirectory(), "mymoviedb.csv");
        
        // The CSV is broken, some entries have an unquoted multi-line overview. We need to fix it.
        var fixedCsv = FixCsv(csvPath);
        
        // Create a pool of fake actors in advance so different movies can get the same actors
        var actorPool = CreateFakeActors();
        
        // Read the fixed CSV
        using var reader = new StringReader(fixedCsv);
        using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture) 
        {
            Mode = CsvMode.RFC4180,
            TrimOptions = TrimOptions.Trim
        });

        csv.Read();
        csv.ReadHeader();
        
        while (csv.Read())
        {
            var releaseDate = csv.GetField<DateOnly>("Release_Date");
            var title = csv.GetField<string>("Title")!;
            var overview = csv.GetField<string>("Overview")!;
            var popularity = csv.GetField<decimal>("Popularity");
            var voteCount = csv.GetField<uint>("Vote_Count");
            var voteAverage = csv.GetField<decimal>("Vote_Average");
            var originalLanguage = csv.GetField<string>("Original_Language")!;
            var genres = csv.GetField<string>("Genre")!;
            var posterUrl = csv.GetField<string>("Poster_Url")!;

            yield return new Movie
            {
                ReleaseDate = releaseDate,
                Title = title,
                Overview = overview,
                Popularity = popularity,
                VoteCount = voteCount,
                VoteAverage = voteAverage,
                OriginalLanguage = originalLanguage,
                Genres = genres
                    .Split(',')
                    .Select(g => g.Trim())
                    .ToList(),
                Actors = GetFakeActors(),
                PosterUrl = posterUrl
            };
        }

        yield break;

        // There are no actors in the CSV, get 2-8 fake names from the generated pool per movie
        List<string> GetFakeActors()
        {
            var faker = new Faker();
            var total = RandomNumberGenerator.GetInt32(2, 8);
            var names = new List<string>(total);
            
            for (var i = 0; i < total; i++)
            {
                names.Add(faker.PickRandom(actorPool));
            }
            
            return names;
        }
    }

    private static string FixCsv(string csvPath)
    {
        // Read all lines in the file
        var lines = File.ReadAllLines(csvPath);
        
        // The first line is the header
        var header = lines[0];
        var resultLines = new List<string> { header };
        
        // Process each line, looking for movie records
        var i = 1;
        
        while (i < lines.Length)
        {
            var line = lines[i];
            
            // If the line starts with a date, it's the beginning of a movie record
            if (LineStartsWithDateRegex().IsMatch(line))
            {
                var parts = SplitCsvLine(line);
                
                // If we have 9 parts (all fields), this is a complete record
                if (parts.Count == 9)
                {
                    // Ensure the overview is properly quoted
                    parts[2] = QuoteField(parts[2]);
                    resultLines.Add(JoinCsvLine(parts));
                }
                else
                {
                    // This is an incomplete record, likely due to multi-line overview
                    // Collect all lines until we find the next movie record or reach the end
                    var movieRecord = new StringBuilder(line);
                    i++;
                    
                    while (i < lines.Length)
                    {
                        var nextLine = lines[i];
                        
                        // If this line starts a new movie record, break
                        if (LineStartsWithDateRegex().IsMatch(nextLine))
                        {
                            break;
                        }
                        
                        // Otherwise, append to our current record
                        movieRecord.Append("\n" + nextLine);
                        
                        i++;
                    }
                    
                    // Now parse the complete record and ensure proper quoting
                    var completeRecord = movieRecord.ToString();
                    var parsedRecord = ParseIncompleteRecord(completeRecord);
                    
                    resultLines.Add(parsedRecord);
                    
                    continue; // Skip the i++ at the end of the loop since we already incremented
                }
            }
            
            i++;
        }
        
        // Join all lines back into a single string
        return string.Join("\n", resultLines);
    }

    private static string ParseIncompleteRecord(string record)
    {
        // Parse record by looking for known field patterns
        
        // First get the release date
        var dateMatch = FindDateRegex().Match(record);
        
        if (!dateMatch.Success)
        {
            return record; // Can't parse
        }
        
        var date = dateMatch.Groups[1].Value;
        
        // Find title (will be between the first comma and overview)
        var firstCommaPos = record.IndexOf(',');
        
        // Try to find where the overview starts - after the second comma
        var secondCommaPos = record.IndexOf(',', firstCommaPos + 1);
        
        if (secondCommaPos == -1) 
        {
            return record; // Can't parse
        }
        
        var title = record.Substring(firstCommaPos + 1, secondCommaPos - firstCommaPos - 1);
        
        // Try to find where popularity starts - look for a pattern like ",digit.digit,"
        var popularityMatch = FindPopularityRegex().Match(record);
        
        if (!popularityMatch.Success) 
        {
            return record; // Can't parse
        }
        
        var popularityPos = record.IndexOf(',' + popularityMatch.Groups[1].Value + ',', StringComparison.Ordinal);
        
        // Extract overview - everything between title and popularity
        var overview = record.Substring(secondCommaPos + 1, popularityPos - secondCommaPos - 1);
        
        overview = QuoteField(overview);
        
        // Get the rest of the record
        var rest = record[popularityPos..];
        
        // Construct a properly formatted record
        return $"{date},{QuoteField(title)},{overview}{rest}";
    }

    private static string QuoteField(string field)
    {
        // Remove any existing quotes
        field = field.Trim('"');
        
        // Escape any quotes
        field = field.Replace("\"", "\"\"");
        
        // Add quotes
        return $"\"{field}\"";
    }

    private static List<string> SplitCsvLine(string line)
    {
        var result = new List<string>();
        var inQuotes = false;
        var startIndex = 0;
        
        for (var i = 0; i < line.Length; i++)
        {
            if (line[i] == '"')
            {
                inQuotes = !inQuotes;
            }

            if (line[i] != ',' || inQuotes)
            {
                continue;
            }
            
            result.Add(line.Substring(startIndex, i - startIndex));
            startIndex = i + 1;
        }
        
        // Add the last field
        result.Add(line[startIndex..]);
        
        return result;
    }

    private static string JoinCsvLine(List<string> fields)
    {
        return string.Join(",", fields);
    }
    
    private static IList<string> CreateFakeActors(int count = 100)
    {
        var faker = new Faker();
        return faker.Make(count, () => faker.Name.FullName());
    }

    [GeneratedRegex(@"^\d{4}-\d{2}-\d{2},")]
    private static partial Regex LineStartsWithDateRegex();
    
    [GeneratedRegex(@"^(\d{4}-\d{2}-\d{2}),")]
    private static partial Regex FindDateRegex();
    
    [GeneratedRegex(@",(\d+\.\d+),(\d+),(\d+\.\d+),")]
    private static partial Regex FindPopularityRegex();
}