using _2.Application.Context;
using _2.Application.Entities;
using _2.Application.Repositories.Interfaces;
using ArchitecutreMachin.Models;
using ArchitecutreMachin.Models.Cars;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ArchitecutreMachin.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
    public class CarController :
        BaseController<Car, CarDto, CarSelectDto>
    {
        private readonly IGenericRepository<CarFeature> _FeatureRepo;

        public CarController(IGenericRepository<Car> repository, IMapper mapper, IGenericRepository<CarFeature> featureRepo)
            : base(repository, mapper)
        {
            _FeatureRepo = featureRepo;
        }

        private string CurrentUserId => User.FindFirstValue(ClaimTypes.NameIdentifier);

        [HttpPost]
        public override async Task<IActionResult> Create(CarDto dto)
        {
            if (CurrentUserId == null)
                return Unauthorized();

            var car = _mapper.Map<Car>(dto);
            car.UserId = CurrentUserId;

            var features = await _FeatureRepo
                .GetQueryable()
                .Where(f => dto.FeatureIds.Contains(f.Id))
                .ToListAsync();

            car.Features = features;

            await _repository.AddAsync(car);
            await _repository.SaveAsync();

            return Ok(_mapper.Map<CarSelectDto>(car));
        }

        [HttpGet]
        public override async Task<IActionResult> GetAll()
        {
            if (CurrentUserId == null)
                return Unauthorized();

            var cars = await _repository
                .GetQueryable()
                .Where(c => c.UserId == CurrentUserId)
                .Include(c => c.Features)
                .ToListAsync();

            return Ok(_mapper.Map<List<CarSelectDto>>(cars));
        }

        [HttpGet("{id}")]
        public override async Task<IActionResult> GetById(int id)
        {
            if (CurrentUserId == null)
                return Unauthorized();

            var car = await _repository
                .GetQueryable()
                .FirstOrDefaultAsync(c =>
                    c.Id == id &&
                    c.UserId == CurrentUserId);

            if (car == null)
                return NotFound();

            return Ok(_mapper.Map<CarSelectDto>(car));
        }

        [HttpPut("{id}")]
        public override async Task<IActionResult> Update(int id, CarDto dto)
        {
            if (CurrentUserId == null)
                return Unauthorized();

            var car = await _repository
                .GetQueryable()
                .Include(c => c.Features)
                .FirstOrDefaultAsync(c =>
                    c.Id == id &&
                    c.UserId == CurrentUserId);

            if (car == null)
                return NotFound();

            car.Name = dto.Name;

            car.Features.Clear();

            var features = await _FeatureRepo
                .GetQueryable()
                .Where(f => dto.FeatureIds.Contains(f.Id))
                .ToListAsync();

            foreach (var f in features)
                car.Features.Add(f);

            await _repository.SaveAsync();

            return Ok(_mapper.Map<CarSelectDto>(car));
        }

        [HttpDelete("{id}")]
        public override async Task<IActionResult> Delete(int id)
        {
            if (CurrentUserId == null)
                return Unauthorized();

            var car = await _repository
                .GetQueryable()
                .FirstOrDefaultAsync(c =>
                    c.Id == id &&
                    c.UserId == CurrentUserId);

            if (car == null)
                return NotFound();

            _repository.Delete(car);
            await _repository.SaveAsync();

            return NoContent();
        }


    }
}
