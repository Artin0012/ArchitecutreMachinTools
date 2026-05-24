using ArchitecutreMachins.Features.Commands.DeleteCar;
using FluentValidation;

namespace ArchitecutreMachins.Features.Validators
{
    public class DeleteCarCommandValidator : AbstractValidator<DeleteCarCommand>
    {
        public DeleteCarCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Invalid Car Id");
        }
    }
}
