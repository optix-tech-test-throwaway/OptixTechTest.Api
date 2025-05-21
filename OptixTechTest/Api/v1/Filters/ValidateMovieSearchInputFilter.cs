using FluentValidation;
using OptixTechTest.Core.Models;

namespace OptixTechTest.Api.v1.Filters;

public class ValidateMovieSearchInputFilter : IEndpointFilter
{
    private readonly IValidator<MovieSearchInput> _validator;

    public ValidateMovieSearchInputFilter(IValidator<MovieSearchInput> validator)
    {
        _validator = validator;
    }

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
