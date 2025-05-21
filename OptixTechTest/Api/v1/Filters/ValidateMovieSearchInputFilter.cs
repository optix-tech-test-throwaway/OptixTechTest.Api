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

    /// <summary>
    /// Validates the <see cref="MovieSearchInput"/> in the request and invokes the next filter if valid.
    /// </summary>
    /// <param name="context">The endpoint filter invocation context.</param>
    /// <param name="next">The next endpoint filter in the pipeline.</param>
    /// <returns>
    /// A <see cref="ValueTask{Object}"/> representing the result of the filter. 
    /// Returns validation errors if validation fails, otherwise proceeds with the next filter.
    /// </returns>
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        // Ensure the request body is present
        if (context.Arguments.FirstOrDefault(a => a is MovieSearchInput) is not MovieSearchInput searchInput)
        {
            return TypedResults.ValidationProblem(
                new Dictionary<string, string[]>
                {
                    { "body", ["Request body is required"]}
                },
                title: "Missing body",
                detail: "The request must include a valid search input body"
            );
        }
        
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
