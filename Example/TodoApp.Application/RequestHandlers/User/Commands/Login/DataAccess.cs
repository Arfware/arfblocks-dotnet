namespace TodoApp.Application.RequestHandlers.Users.Commands.Login
{
	public class DataAccess : BaseDataAccessLayer<User>, IDataAccess
	{
		private readonly ApplicationDbContext _dbContext;

		public DataAccess(ArfBlocksDependencyProvider dependencyProvider)
			: base(dependencyProvider.GetInstance<ApplicationDbContext>())
		{
			_dbContext = dependencyProvider.GetInstance<ApplicationDbContext>();
		}

		public async Task<User> GetUserByEmail(string email)
		{
			return await _dbContext.Users
										.Include(u => u.Department)
										.FirstOrDefaultAsync(u => u.Email == email);
		}
	}
}