using ArchitecutreMachin.Models.Cars;
using ArchitecutreMachins.Models.Pagination;
using MediatR;

namespace ArchitecutreMachins.Features.Queries.GetAllCars
{
    public class GetAllCarsQuery : PaginationParams, IRequest<List<CarSelectDto>>
    {

    }
}
