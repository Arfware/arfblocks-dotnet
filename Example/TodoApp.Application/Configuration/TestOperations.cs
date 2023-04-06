using Arfware.ArfBlocks.Test.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace TodoApp.Application.Configuration
{
	public class TestOperations : ITestOperations
	{
		private readonly ApplicationDbContext _dbContext;
		public TestOperations(ArfBlocksDependencyProvider dependencyProvider)
		{
			_dbContext = dependencyProvider.GetInstance<ApplicationDbContext>();
		}

		public async Task PreExecuting()
		{
			// System.Console.WriteLine("\n_________TEST_EXECUTION_STARTED________\n");	
			System.Console.WriteLine();
			System.Console.WriteLine("- Deleting Old Database Data...");

			// await _dbContext.Database.EnsureDeletedAsync();

			System.Console.WriteLine("  Deleted.");

			System.Console.WriteLine("- Migrating Scheme and Default Data...");

			// await _dbContext.Database.MigrateAsync();

			System.Console.WriteLine("  Migrated.");

			System.Console.WriteLine("- Starting Default DB Seesing...");
			var defaultSeeder = new DefaultSeeder(_dbContext);
			await defaultSeeder.Seed();
			System.Console.WriteLine("  Completed.");
		}

		public async Task PostExecuting()
		{
			await Task.CompletedTask;
			// System.Console.WriteLine("\n_________TEST_EXECUTION_COMPLETED________\n");
		}
	}
}
