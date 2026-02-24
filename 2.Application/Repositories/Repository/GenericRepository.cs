using _2.Application.Context;
using _2.Application.Entities;
using _2.Application.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3.Infrastructure.Services
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<TEntity> _dbset;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbset = context.Set<TEntity>();
        }

        public async Task<List<TEntity>> GetAllAsync()
            => await _dbset.ToListAsync();

        public async Task<TEntity?> GetByIdAsync(int id)
            => await _dbset.FindAsync(id);

        public async Task AddAsync(TEntity entity)
            => await _dbset.AddAsync(entity);

        public void Update(TEntity entity)
            => _dbset.Update(entity);

        public void Delete(TEntity entity)
            => _dbset.Remove(entity);

        public async Task SaveAsync()
            => await _context.SaveChangesAsync();

        public IQueryable<TEntity> GetQueryable()
        {
            return _context.Set<TEntity>();
        }
    }
}
