namespace TodoApp.Application.RequestHandlers.Users.Commands.Login
{
	public class HandlerContext : EndpointContext
	{
		private readonly ApplicationDbContext _dbContext;

		public int UsersCount { get; set; }

		public HandlerContext(ArfBlocksDependencyProvider dependencyProvider)
		{
			_dbContext = dependencyProvider.GetInstance<ApplicationDbContext>();
			this.UsersCount = _dbContext.Users.Count();
		}
	}
}