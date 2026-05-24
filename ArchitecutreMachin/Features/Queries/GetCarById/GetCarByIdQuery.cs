using ArchitecutreMachin.Models.Cars;
using MediatR;

namespace ArchitecutreMachins.Features.Queries.GetCarById
{
    public class GetCarByIdQuery : IRequest<CarSelectDto>
    {
        public int Id { get; set; }
    }
}
