using System;
using Microsoft.EntityFrameworkCore;
using TodoApp.Domain.Entities;
using TodoApp.Domain.Entities.Base;

namespace TodoApp.Infrastructure.RelationalDB
{
	public class ApplicationDbContext : DbContext
	{
		// Tables
		public DbSet<Department> Departments { get; set; }
		public DbSet<User> Users { get; set; }
		public DbSet<TodoTask> Tasks { get; set; }

		public ApplicationDbContext(RelationalDbConfiguration configuration) : base(BuildDbContextOptions(configuration))
		{ }

		public static DbContextOptions<ApplicationDbContext> BuildDbContextOptions(RelationalDbConfiguration configuration)
		{
			var dbContextOptionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
			// dbContextOptionsBuilder.UseSqlServer(configuration.ConnectionString);
			dbContextOptionsBuilder.UseInMemoryDatabase("example-task-db");

			return dbContextOptionsBuilder.Options;
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			var utcDate = new DateTime(2021, 12, 30, 9, 1, 14, 890, DateTimeKind.Utc);

			modelBuilder.Entity<Department>().HasData(
				new Department() { Id = Guid.Parse("927dbce3-f162-4e80-8991-4c71d7aa7153"), Name = "Human Resources", CreatedAt = utcDate },
				new Department() { Id = Guid.Parse("423e95a1-44ce-4b4c-bffe-37d4548e51bd"), Name = "Sales", CreatedAt = utcDate },
				new Department() { Id = Guid.Parse("f20c58c7-52d6-4975-aef1-fd5f9fafc841"), Name = "Marketing", CreatedAt = utcDate },
				new Department() { Id = Guid.Parse("138ff80c-4139-4428-a1e0-2a475aa969c4"), Name = "Information Technologies", CreatedAt = utcDate }
			);

			modelBuilder.Entity<User>().HasData(
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
			);
		}


		public override int SaveChanges()
		{
			AddTimestamps();
			return base.SaveChanges();
		}

		public override int SaveChanges(bool acceptAllChangesOnSuccess)
		{
			AddTimestamps();
			return base.SaveChanges(acceptAllChangesOnSuccess);
		}

		public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
		{
			AddTimestamps();
			return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
		}

		public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			AddTimestamps();
			return await base.SaveChangesAsync(cancellationToken);
		}

		private void AddTimestamps()
		{
			var normalEntities = ChangeTracker.Entries()
				.Where(x => x.Entity is BaseEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));

			foreach (var entity in normalEntities)
			{
				if (entity.State == EntityState.Added)
				{
					((BaseEntity)entity.Entity).CreatedAt = DateTime.UtcNow;
				}
				else if (entity.State == EntityState.Modified)
				{
					((BaseEntity)entity.Entity).UpdatedAt = DateTime.UtcNow;
				}
				else if (entity.State == EntityState.Deleted)
				{
					((BaseEntity)entity.Entity).DeletedAt = DateTime.UtcNow;
				}
			}
		}
	}
}
