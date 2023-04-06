# Data Access

It is using for access data from data sources such as SQL, NoSQL databases in other Block components.

Overview:
```c#
namespace TodoApp.Application.RequestHandlers.Tasks.Commands.Create
{
	public class DataAccess : IDataAccess
	{
		//...
		public DataAccess(ArfBlocksDependencyProvider depencyProvider)
		{
			//...
		}
	}
}
```

Example:
```c#
namespace TodoApp.Application.RequestHandlers.Tasks.Commands.Create
{
	public class DataAccess : IDataAccess
	{
		private readonly ApplicationDbContext _dbContext;

		public DataAccess(ArfBlocksDependencyProvider depencyProvider)
		{
			_dbContext = depencyProvider.GetInstance<ApplicationDbContext>();
		}

		public async Task CreateTask(Task task)
		{
			_dbContext.Tasks.Add(task);
			await _dbContext.SaveChangesAsync();
		}
	}
}
```

