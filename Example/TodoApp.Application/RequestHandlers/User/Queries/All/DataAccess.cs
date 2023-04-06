using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arfware.ArfBlocks.Core;
using Arfware.ArfBlocks.Core.Abstractions;
using Microsoft.EntityFrameworkCore;
using TodoApp.Domain.Entities;
using TodoApp.Infrastructure.RelationalDB;

namespace TodoApp.Application.RequestHandlers.Users.Queries.All
{
	public class DataAccess : IDataAccess
	{
		private readonly ApplicationDbContext _dbContext;

		public DataAccess(ArfBlocksDependencyProvider depencyProvider)
		{
			_dbContext = depencyProvider.GetInstance<ApplicationDbContext>();
		}

		public async Task<List<User>> GetAllUsers()
		{
			return await _dbContext.Users
										.Include(d => d.Department)
										.Where(d => !d.IsDeleted)
										.OrderBy(i => i.FirstName)
										.ToListAsync();
		}
	}
}