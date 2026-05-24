using _2.Application.Entities;
using _2.Application.Repositories.Interfaces;
using ArchitecutreMachin.Models.Cars;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ArchitecutreMachins.Features.Queries.GetCarById
{
    public class GetCarByIdQueryHandler : IRequestHandler<GetCarByIdQuery, CarSelectDto>
    {
        private readonly IGenericRepository<Car> _repository;
        private readonly IMapper _mapper;

        public GetCarByIdQueryHandler(IGenericRepository<Car> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<CarSelectDto> Handle(GetCarByIdQuery request, CancellationToken cancellationToken)
        {
            var car = await _repository
                .GetQueryable()
                .AsNoTracking()
                .Include(f => f.Features)
                .FirstOrDefaultAsync(c => c.Id == request.Id,
                cancellationToken);

            if (car == null)
                return null;

            return _mapper.Map<CarSelectDto>(car);
        }
    }
}
