using System;
using TodoApp.Domain.Entities;
using Arfware.ArfBlocks.Core;
using Arfware.ArfBlocks.Core.Abstractions;
using TodoApp.Infrastructure.RelationalDB;

namespace TodoApp.Application.RequestHandlers.Tasks.Commands.Update
{
	public class DataAccess : BaseDataAccessLayer<TodoTask>, IDataAccess
	{
		private readonly ApplicationDbContext _dbContext;

		public DataAccess(ArfBlocksDependencyProvider dependencyProvider)
			: base(dependencyProvider.GetInstance<ApplicationDbContext>())
		{
			_dbContext = dependencyProvider.GetInstance<ApplicationDbContext>();
		}
	}
}