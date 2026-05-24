using _2.Application.Entities;
using _2.Application.Repositories.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace ArchitecutreMachins.Features.Commands.CreateCars
{
    //Impliment => IRequestHandler
    public class CreateCarCommandHandler : IRequestHandler<CreateCarCommand, int>
    {
        private readonly IGenericRepository<Car> _carRepository;
        private readonly IGenericRepository<CarFeature> _FeatureRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CreateCarCommandHandler(IGenericRepository<Car> CarRepository, IGenericRepository<CarFeature> FeatureRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _carRepository = CarRepository;
            _FeatureRepository = FeatureRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<int> Handle(CreateCarCommand request, CancellationToken cancellationToken)
        {

            var userId = _httpContextAccessor
                .HttpContext?
                .User
                .FindFirst(ClaimTypes.NameIdentifier)?
                .Value;

            var feature =  _FeatureRepository
                .GetQueryable()
                .Where(f => request.FeatureIds.Contains(f.Id))
                .ToList();

            var car = new Car
            {
                Name = request.Name,
                UserId = userId,
                Features = feature
            };

             _carRepository.AddAsync(car);
             _carRepository.SaveAsync();

            return car.Id;
        }


    }
}
