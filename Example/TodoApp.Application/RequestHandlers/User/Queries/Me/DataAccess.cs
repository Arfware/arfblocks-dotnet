using System;
using TodoApp.Domain.Entities;
using Arfware.ArfBlocks.Core;
using Arfware.ArfBlocks.Core.Abstractions;
using TodoApp.Infrastructure.RelationalDB;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TodoApp.Application.RequestHandlers.Users.Queries.Me
{
	public class DataAccess : BaseDataAccessLayer<User>, IDataAccess
	{
		private readonly ApplicationDbContext _dbContext;

		public DataAccess(ArfBlocksDependencyProvider depencyProvider)
			: base(depencyProvider.GetInstance<ApplicationDbContext>())
		{
			_dbContext = depencyProvider.GetInstance<ApplicationDbContext>();
		}

		public async Task<User> GetUserById(Guid userId)
		{
			return await _dbContext.Users
										.Include(u => u.Department)
										.FirstOrDefaultAsync(u => u.Id == userId);
		}
	}
}