namespace TodoApp.Application.RequestHandlers.Users.Commands.Login
{
	public class EndpointContext : IEndpointContext
	{
		private readonly ApplicationDbContext _dbContext;

		public int UsersCount { get; set; }

		public EndpointContext(ArfBlocksDependencyProvider dependencyProvider)
		{
			_dbContext = dependencyProvider.GetInstance<ApplicationDbContext>();
			this.UsersCount = _dbContext.Users.Count();
		}
	}
}