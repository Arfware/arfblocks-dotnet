using System;
using Microsoft.EntityFrameworkCore;
using Arfware.ArfBlocks.Core.Exceptions;
using System.Threading.Tasks;
using TodoApp.Infrastructure.RelationalDB;

namespace TodoApp.Infrastructure.Services
{
	public class DbValidationService
	{
		private readonly ApplicationDbContext _dbContext;

		public DbValidationService(ApplicationDbContext dbContext)
		{
			this._dbContext = dbContext;
		}

		public async Task ValidateTaskExist(Guid taskId)
		{
			// Get
			var taskExist = await _dbContext.Tasks.AnyAsync(d => d.Id == taskId);

			// Check
			if (!taskExist)
				throw new ArfBlocksValidationException("TASK_NOT_EXIST");
		}

		public async Task ValidateDepartmentExist(Guid departmentId)
		{
			// Get
			var departmentExist = await _dbContext.Departments.AnyAsync(d => d.Id == departmentId);

			// Check
			if (!departmentExist)
				throw new ArfBlocksValidationException("DEPARTMENT_NOT_EXIST");
		}

		public async Task ValidateUserExist(Guid userId)
		{
			// Get
			var userExist = await _dbContext.Users.AnyAsync(d => d.Id == userId);

			// Check
			if (!userExist)
				throw new ArfBlocksValidationException("USER_NOT_EXIST");
		}

		public async Task ValidateUserExist(string email)
		{
			// Get
			var userExist = await _dbContext.Users.AnyAsync(d => d.Email == email);

			// Check
			if (!userExist)
				throw new ArfBlocksValidationException("USER_NOT_EXIST");
		}
	}
}
