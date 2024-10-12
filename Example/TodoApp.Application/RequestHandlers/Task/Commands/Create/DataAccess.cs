using System;
using System.Threading.Tasks;
using TodoApp.Domain.Entities;
using Arfware.ArfBlocks.Core;
using Arfware.ArfBlocks.Core.Abstractions;
using Microsoft.EntityFrameworkCore;
using TodoApp.Infrastructure.RelationalDB;

namespace TodoApp.Application.RequestHandlers.Tasks.Commands.Create
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