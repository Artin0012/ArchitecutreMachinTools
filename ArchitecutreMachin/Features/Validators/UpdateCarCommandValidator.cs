using ArchitecutreMachins.Features.Commands.UpdateCar;
using FluentValidation;

namespace ArchitecutreMachins.Features.Validators
{
    public class UpdateCarCommandValidator : AbstractValidator<UpdateCarCommand>
    {
        public UpdateCarCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Invalid Car Id");

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name is required")
                .MinimumLength(2)
                .MaximumLength(50);

            RuleFor(x => x.FeatureIds)
                .NotEmpty()
                .WithMessage("At least one feature must be selected");
        }
    }
}
