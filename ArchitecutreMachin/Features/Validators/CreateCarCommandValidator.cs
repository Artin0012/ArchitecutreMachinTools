using ArchitecutreMachins.Features.Commands.CreateCars;
using FluentValidation;

namespace ArchitecutreMachins.Features.Validators
{
    public class CreateCarCommandValidator : AbstractValidator<CreateCarCommand>
    {
        public CreateCarCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Car name is required")
                .MinimumLength(2)
                .MaximumLength(50);

            RuleFor(x => x.FeatureIds)
                .NotEmpty()
                .WithMessage("At least one feature is required");
        }

    }
}
