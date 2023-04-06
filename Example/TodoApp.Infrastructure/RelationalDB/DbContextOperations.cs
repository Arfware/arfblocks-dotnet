using TodoApp.Domain.Entities.Base;
using Microsoft.EntityFrameworkCore;

namespace TodoApp.Infrastructure.RelationalDB;
public class DbContextOperations
{
	private readonly ApplicationDbContext _dbcontext;
	public DbContextOperations(ApplicationDbContext dbContext)
	{
		_dbcontext = dbContext;
	}

	public async Task Create<TEntity>(TEntity entity) where TEntity : CoreEntity
	{
		_dbcontext.Set<TEntity>().Add(entity);
		await _dbcontext.SaveChangesAsync();
	}

	public async Task<TEntity> GetById<TEntity>(Guid id) where TEntity : CoreEntity
	{
		return await _dbcontext.Set<TEntity>().Where(a => a.Id == id).FirstOrDefaultAsync();
	}

}