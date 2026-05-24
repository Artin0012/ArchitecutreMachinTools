using MediatR;

namespace ArchitecutreMachins.Features.Commands.DeleteCar
{
    public class DeleteCarCommand :IRequest<bool>
    {
        public int Id { get; set; }
    }
}
