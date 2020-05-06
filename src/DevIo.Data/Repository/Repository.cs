using DevIO.Data.Context;
using DevIO.Business.Interfaces;
using DevIO.Business.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DevIO.Data.Repository
{
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity, new() //The new() is needed to instantiate classes of type TEntity.
    {
        protected readonly AppDbContext _appDbContext;
        protected readonly DbSet<TEntity> _dbSet;

        public Repository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
            _dbSet = appDbContext.Set<TEntity>();
        }

        public virtual async Task Add(TEntity entity)
        {
            _dbSet.Add(entity);
            await SaveChanges();
        }

        public virtual async Task Delete(Guid id)
        {
            //_dbSet.Remove(await _dbSet.FindAsync(id)); //With DB access.
            _dbSet.Remove(new TEntity { Id = id }); // Without DB access.
            await SaveChanges();
        }

        public async Task<IEnumerable<TEntity>> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.AsNoTracking().Where(predicate).ToListAsync();
        }

        public virtual async Task<IList<TEntity>> GetAll()
        {
            return await _dbSet.ToListAsync();
        }

        public virtual async Task<TEntity> GetById(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<int> SaveChanges()
        {
            return await _appDbContext.SaveChangesAsync();
        }

        public virtual async Task Update(TEntity entity)
        {
            _dbSet.Update(entity);
            await SaveChanges();
        }

        public void Dispose()
        {
            _appDbContext?.Dispose();
        }
    }
}
