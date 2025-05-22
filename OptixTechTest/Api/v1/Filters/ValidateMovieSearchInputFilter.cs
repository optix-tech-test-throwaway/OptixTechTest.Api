using FluentValidation;
using OptixTechTest.Core.Models;

namespace OptixTechTest.Api.v1.Filters;

/// <summary>
/// Provides input validation for movie search requests.
/// </summary>
/// <remarks>
/// This filter validates <see cref="MovieSearchInput"/> objects in the request pipeline 
/// using FluentValidation.
/// </remarks>
public class ValidateMovieSearchInputFilter : IEndpointFilter
{
    private readonly IValidator<MovieSearchInput> _validator;

    /// <summary>
    /// Initializes a new instance of the <see cref="ValidateMovieSearchInputFilter"/> class.
    /// </summary>
    /// <param name="validator">The validator for <see cref="MovieSearchInput"/> objects.</param>
    public ValidateMovieSearchInputFilter(IValidator<MovieSearchInput> validator)
    {
        _validator = validator;
    }

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        // Parameter binding guarantees that the search input is always present in the request
        var searchInput = (MovieSearchInput)context.Arguments.FirstOrDefault(a => a is MovieSearchInput)!;
        
        // Validate the search input
        var validationResult = await _validator.ValidateAsync(searchInput);

        if (validationResult.IsValid)
        {
            return await next(context);
        }
        
        var errors = validationResult.Errors
            .GroupBy(e => e.PropertyName)
            .ToDictionary(
                g => g.Key, 
                g => g.Select(e => e.ErrorMessage).ToArray()
            );
            
        return TypedResults.ValidationProblem(
            errors,
            title: "Validation Failed",
            detail: "The provided search input is invalid"
        );
    }
}
