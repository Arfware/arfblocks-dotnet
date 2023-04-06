using Microsoft.EntityFrameworkCore;
using TodoApp.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace TodoApp.Infrastructure.RelationalDB
{
	public class BaseDataAccessLayer<TEntity> where TEntity : BaseEntity
	{
		private readonly ApplicationDbContext _dbContext;

		public BaseDataAccessLayer(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public int SaveChanges()
		{
			return _dbContext.SaveChanges();
		}

		public async Task<int> SaveChangesAsync()
		{
			return await _dbContext.SaveChangesAsync();
		}

		public async Task CommitAsync()
		{
			await _dbContext.SaveChangesAsync();
		}

		public async Task<bool> Add(TEntity entity)
		{
			_dbContext.Set<TEntity>().Add(entity);
			await _dbContext.SaveChangesAsync();
			return true;
		}


		public async Task<bool> AddRange(List<TEntity> entityList)
		{
			foreach (TEntity entity in entityList)
				_dbContext.Set<TEntity>().Add(entity);

			await _dbContext.SaveChangesAsync();
			return true;
		}

		public async Task<List<TEntity>> GetAll()
		{
			return await _dbContext.Set<TEntity>().ToListAsync();
		}

		public async Task<List<TEntity>> GetAllExceptNonDeleted()
		{
			return await _dbContext.Set<TEntity>().Where(t => !t.IsDeleted)
								.ToListAsync();
		}

		public async Task<int> Count()
		{
			return await _dbContext.Set<TEntity>().CountAsync();
		}

		public async Task<TEntity> GetById(Guid id)
		{
			return await Where(f => f.Id == id).FirstOrDefaultAsync();
		}

		public async Task DeleteById(Guid id)
		{
			var entity = await this.GetById(id);
			entity.IsDeleted = true;
			await _dbContext.SaveChangesAsync();
		}

		public async Task Delete(TEntity entity)
		{
			entity.IsDeleted = true;
			_dbContext.Entry(entity).State = EntityState.Modified;
			await _dbContext.SaveChangesAsync();
		}

		public async Task<bool> Update(TEntity entity)
		{
			_dbContext.Entry(entity).State = EntityState.Modified;
			await _dbContext.SaveChangesAsync();
			return true;
		}

		public async Task<bool> UpdateRange(List<TEntity> entityList)
		{
			foreach (TEntity entity in entityList)
			{
				_dbContext.Entry(entity).State = EntityState.Modified;
			}

			await _dbContext.SaveChangesAsync();
			return true;
		}

		public async Task<bool> Remove(TEntity entity)
		{
			_dbContext.Set<TEntity>().Remove(entity);
			await _dbContext.SaveChangesAsync();
			return true;
		}

		public async Task<bool> Remove(Guid entityId)
		{
			TEntity entity = await GetById(entityId);
			_dbContext.Set<TEntity>().Remove(entity);
			await _dbContext.SaveChangesAsync();
			return true;
		}

		public async Task RemoveRange(List<TEntity> entityList)
		{
			_dbContext.Set<TEntity>().RemoveRange(entityList);
			await _dbContext.SaveChangesAsync();
		}

		public IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate)
		{
			return _dbContext.Set<TEntity>().Where(predicate);
		}

		public void AddWithoutCommit(TEntity entity)
		{
			_dbContext.Set<TEntity>().Add(entity);
		}

		public void UpdateWithoutCommit(TEntity entity)
		{
			_dbContext.Set<TEntity>().Update(entity);
		}

		public void DeleteWithoutCommit(TEntity entity)
		{
			entity.IsDeleted = true;
			_dbContext.Set<TEntity>().Update(entity);
		}

		public void RemoveWithoutCommit(TEntity entity)
		{
			_dbContext.Set<TEntity>().Remove(entity);
		}
	}
}
