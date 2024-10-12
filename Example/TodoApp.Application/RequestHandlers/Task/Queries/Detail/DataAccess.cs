using System;
using System.Threading.Tasks;
using Arfware.ArfBlocks.Core;
using Arfware.ArfBlocks.Core.Abstractions;
using Microsoft.EntityFrameworkCore;
using TodoApp.Domain.Entities;
using TodoApp.Infrastructure.RelationalDB;

namespace TodoApp.Application.RequestHandlers.Tasks.Queries.Detail
{
	public class DataAccess : IDataAccess
	{
		private readonly ApplicationDbContext _dbContext;

		public DataAccess(ArfBlocksDependencyProvider dependencyProvider)
		{
			_dbContext = dependencyProvider.GetInstance<ApplicationDbContext>();
		}

		public async Task<TodoTask> GetFilledTaskById(Guid taskId)
		{
			return await _dbContext.Tasks
										.Include(t => t.AssignedDepartment)
										.Include(t => t.CreatedBy)
										.FirstOrDefaultAsync(d => !d.IsDeleted && d.Id == taskId);
		}
	}
}