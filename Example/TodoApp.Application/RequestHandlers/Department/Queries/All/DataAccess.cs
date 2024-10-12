using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arfware.ArfBlocks.Core;
using Arfware.ArfBlocks.Core.Abstractions;
using Microsoft.EntityFrameworkCore;
using TodoApp.Domain.Entities;
using TodoApp.Infrastructure.RelationalDB;

namespace TodoApp.Application.RequestHandlers.Departments.Queries.All
{
	public class DataAccess : IDataAccess
	{
		private readonly ApplicationDbContext _dbContext;

		public DataAccess(ArfBlocksDependencyProvider dependencyProvider)
		{
			_dbContext = dependencyProvider.GetInstance<ApplicationDbContext>();
		}

		public async Task<List<Department>> GetAllDepartments()
		{
			return await _dbContext.Departments
										.Where(d => !d.IsDeleted)
										.OrderBy(i => i.Name)
										.ToListAsync();
		}
	}
}