using System;
using TodoApp.Domain.Entities;
using Arfware.ArfBlocks.Core;
using Arfware.ArfBlocks.Core.Abstractions;
using TodoApp.Infrastructure.RelationalDB;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TodoApp.Application.DefaultHandlers.Operators.Commands.PostOperate;

public class DataAccess : BaseDataAccessLayer<User>, IDataAccess
{
	private readonly ApplicationDbContext _dbContext;

	public DataAccess(ArfBlocksDependencyProvider dependencyProvider)
		: base(dependencyProvider.GetInstance<ApplicationDbContext>())
	{
		_dbContext = dependencyProvider.GetInstance<ApplicationDbContext>();
	}
}
