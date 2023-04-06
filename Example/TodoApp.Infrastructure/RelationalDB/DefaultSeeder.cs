using System;
using Microsoft.EntityFrameworkCore;
using TodoApp.Domain.Entities;
using TodoApp.Domain.Entities.Base;

namespace TodoApp.Infrastructure.RelationalDB
{
	public class DefaultSeeder
	{
		private readonly ApplicationDbContext _dbContext;
		public DefaultSeeder(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task Seed()
		{
			var utcDate = new DateTime(2021, 12, 30, 9, 1, 14, 890, DateTimeKind.Utc);

			_dbContext.Departments.AddRange(new List<Department>(){
				new Department() { Id = Guid.Parse("927dbce3-f162-4e80-8991-4c71d7aa7153"), Name = "Human Resources", CreatedAt = utcDate },
				new Department() { Id = Guid.Parse("423e95a1-44ce-4b4c-bffe-37d4548e51bd"), Name = "Sales", CreatedAt = utcDate },
				new Department() { Id = Guid.Parse("f20c58c7-52d6-4975-aef1-fd5f9fafc841"), Name = "Marketing", CreatedAt = utcDate },
				new Department() { Id = Guid.Parse("138ff80c-4139-4428-a1e0-2a475aa969c4"), Name = "Information Technologies", CreatedAt = utcDate }
			});

			_dbContext.Users.AddRange(new List<User>() {
				new User()
				{
					Id = Guid.Parse("f973d74b-b7df-40a6-a530-017dcdd870e7"),
					DepartmentId = Guid.Parse("927dbce3-f162-4e80-8991-4c71d7aa7153"),
					FirstName = "Mary",
					LastName = "Gleen",
					Email = "mary@company.com",
					CreatedAt = utcDate,
				},
				new User()
				{
					Id = Guid.Parse("3f05215c-b48e-479f-985d-001f2bdf2b7b"),
					DepartmentId = Guid.Parse("423e95a1-44ce-4b4c-bffe-37d4548e51bd"),
					FirstName = "John",
					LastName = "Doe",
					Email = "john@company.com",
					CreatedAt = utcDate,
				}
			});

			await _dbContext.SaveChangesAsync();
		}
	}
}
