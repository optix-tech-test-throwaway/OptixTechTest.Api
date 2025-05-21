using FluentValidation;
using OptixTechTest.Core.Models;

namespace OptixTechTest.Api.v1.Validators;

public class MovieSearchInputValidator : AbstractValidator<MovieSearchInput>
{
    public MovieSearchInputValidator()
    {
        RuleFor(m => m.Query)
            .MaximumLength(256)
            .WithMessage("Query must be less than 256 characters");
        
        RuleFor(m => m.Language)
            .MaximumLength(2)
            .WithMessage("Languages are specified as a two character code, e.g. en, es, fr, etc.");
        
        RuleFor(m => m.Limit)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Limit must be greater than or equal to 0");
        
        RuleFor(m => m.Cursor)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Cursor must be greater than or equal to 0");

        RuleFor(m => m.OrderBy).IsInEnum();
        RuleFor(m => m.Direction).IsInEnum();
    }
}
