using _2.Application.Entities;
using _2.Application.Repositories.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ArchitecutreMachins.Features.Commands.DeleteCar
{
    public class DeleteCarCommandHandler : IRequestHandler<DeleteCarCommand, bool>
    {
        private readonly IGenericRepository<Car> _carRepo;


        public DeleteCarCommandHandler(IGenericRepository<Car> carRepo)
        {
            _carRepo = carRepo;
        }

        public async Task<bool> Handle(DeleteCarCommand request, CancellationToken cancellationToken)
        {
            var car = await _carRepo
                .GetQueryable()
                .FirstOrDefaultAsync(c => c.Id == request.Id,
                cancellationToken);

            if(car == null) 
                return false;

            _carRepo.Delete(car);
            await _carRepo.SaveAsync();

            return true;
        }
    }
}
