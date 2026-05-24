using _2.Application.Entities;
using _2.Application.Repositories.Interfaces;
using ArchitecutreMachin.Models.CarFeatures;
using ArchitecutreMachin.Models.Cars;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ArchitecutreMachins.Features.Queries.GetAllCars
{
    public class GetAllCarsQueryHandler : IRequestHandler<GetAllCarsQuery, List<CarSelectDto>>
    {
        private readonly IGenericRepository<Car> _repository;
        private readonly IMapper _mapper;

        public GetAllCarsQueryHandler(IGenericRepository<Car> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<CarSelectDto>> Handle(GetAllCarsQuery request, CancellationToken cancellationToken)
        {
            var car = await _repository
                .GetQueryable()
                .AsNoTracking()
                .Include(f => f.Features)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            return _mapper.Map<List<CarSelectDto>>(car);
        }
    }
}
