using _2.Application.Entities;
using _2.Application.Repositories.Interfaces;
using ArchitecutreMachins.Models.Pagination;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ArchitecutreMachin.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class BaseController<TEntity, TDto, TSelectDto>(IGenericRepository<TEntity> repository, IMapper mapper) : ControllerBase
        where TEntity : BaseEntity
    {
        protected readonly IGenericRepository<TEntity> _repository = repository;
        protected readonly IMapper _mapper = mapper;

        [HttpGet]
        public virtual async Task<IActionResult> GetAll([FromQuery] PaginationParams request)
        {
            var entitys = await _repository.GetAllAsync();
            var result = _mapper.Map<List<TSelectDto>>(entitys);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public virtual async Task<IActionResult> GetById(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
                return NotFound();

            var result = _mapper.Map<TSelectDto>(entity);
            return Ok(result);
        }

        [HttpPost]
        public virtual async Task<IActionResult> Create(TDto dto)
        {
            var entity = _mapper.Map<TEntity>(dto);

            await _repository.AddAsync(entity);
            await _repository.SaveAsync();

            var result = _mapper.Map<TSelectDto>(entity);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public virtual async Task<IActionResult> Update(int id, TDto dto)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
                return NotFound();

            _mapper.Map(dto, entity);

            _repository.Update(entity);
            await _repository.SaveAsync();

            var result = _mapper.Map<TSelectDto>(entity);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public virtual async Task<IActionResult> Delete(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
                return NotFound();

            _repository.Delete(entity);
            await _repository.SaveAsync();

            return NoContent();
        }
    }
}
