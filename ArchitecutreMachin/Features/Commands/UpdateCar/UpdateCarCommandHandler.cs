using _2.Application.Entities;
using _2.Application.Repositories.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ArchitecutreMachins.Features.Commands.UpdateCar
{
    public class UpdateCarCommandHandler : IRequestHandler<UpdateCarCommand, bool>
    {
        private readonly IGenericRepository<Car> _carRepo;
        private readonly IGenericRepository<CarFeature> _featureRepo;


        public UpdateCarCommandHandler(IGenericRepository<Car> carRepo, IGenericRepository<CarFeature> featureRepo)
        {
            _carRepo = carRepo;
            _featureRepo = featureRepo;
        }
        public async Task<bool> Handle(UpdateCarCommand request, CancellationToken cancellationToken)
        {
            var car = await _carRepo
               .GetQueryable()
               .Include(c => c.Features)
               .FirstOrDefaultAsync(
                   c => c.Id == request.Id,
                   cancellationToken);

            if (car == null)
                return false;

            car.Name = request.Name;

            car.Features.Clear();

            var features = await _featureRepo
                .GetQueryable()
                .Where(f => request.FeatureIds.Contains(f.Id))
                .ToListAsync(cancellationToken);

            foreach (var f in features)
                car.Features.Add(f);

            await _carRepo.SaveAsync();

            return true;
        }
    }
}
