using _2.Application.Entities;
using _2.Application.Repositories.Interfaces;
using ArchitecutreMachin.Models.CarFeatures;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ArchitecutreMachin.Controllers
{
    [Authorize]
    public class CarFeatureController :
        BaseController<CarFeature, CarFeatureDto, CarFeatureSelectDto>
    {
        private readonly IGenericRepository<Car> _carRepository;
        public CarFeatureController(IGenericRepository<CarFeature> repository
            , IGenericRepository<Car> carRepository,
            IMapper mapper) : base(repository, mapper)
        {
            _carRepository = carRepository;

        }

        private string? CurrentUserId =>
        User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        [HttpGet]
        public override async Task<IActionResult> GetAll()
        {
            if (CurrentUserId == null)
                return Unauthorized();

            var cars = await _carRepository
                .GetQueryable()
                .Where(c => c.UserId == CurrentUserId)
                .Include(c => c.Features)
                .ToListAsync();

            var features = cars
                .SelectMany(c => c.Features)
                .Distinct()
                .ToList();

            return Ok(_mapper.Map<List<CarFeatureSelectDto>>(features));
        }

        [HttpPost]
        public override async Task<IActionResult> Create(CarFeatureDto dto)
        {
            if (CurrentUserId == null)
                return Unauthorized();

            var car = await _carRepository
                .GetQueryable()
                .Include(c => c.Features)
                .FirstOrDefaultAsync(c =>
                    c.Id == dto.CarId &&
                    c.UserId == CurrentUserId);

            if (car == null)
                return NotFound("Car not found");

            var feature = _mapper.Map<CarFeature>(dto);

            car.Features.Add(feature);

            await _repository.SaveAsync();

            return Ok(_mapper.Map<CarFeatureSelectDto>(feature));
        }

        [HttpDelete("{id}")]
        public override async Task<IActionResult> Delete(int id)
        {
            if (CurrentUserId == null)
                return Unauthorized();

            var feature = await _repository
                .GetQueryable()
                .Include(f => f.Cars)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (feature == null)
                return NotFound();

            var userCarIds = await _carRepository
                .GetQueryable()
                .Where(c => c.UserId == CurrentUserId)
                .Select(c => c.Id)
                .ToListAsync();

            bool allowed = feature.Cars
                .Any(c => userCarIds.Contains(c.Id));

            if (!allowed && !User.IsInRole("Admin"))
                return Forbid();

            _repository.Delete(feature);
            await _repository.SaveAsync();

            return NoContent();
        }
    }
}
