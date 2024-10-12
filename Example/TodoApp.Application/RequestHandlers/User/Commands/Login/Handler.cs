namespace TodoApp.Application.RequestHandlers.Users.Commands.Login
{
	[AllowAnonymousHandler]
	public class Handler : IRequestHandler
	{
		private readonly DataAccess dataAccessLayer;
		private readonly CurrentClientService _clientService;
		private readonly IJwtService _jwtService;
		private readonly ActivityLogService _activityLogService;

		public Handler(ArfBlocksDependencyProvider dependencyProvider, object dataAccess)
		{
			dataAccessLayer = (DataAccess)dataAccess;
			_clientService = dependencyProvider.GetInstance<CurrentClientService>();
			_jwtService = dependencyProvider.GetInstance<IJwtService>();
			_activityLogService = dependencyProvider.GetInstance<ActivityLogService>();
		}

		public async Task<ArfBlocksRequestResult> Handle(IRequestModel payload, IEndpointContext context, CancellationToken cancellationToken)
		{
			var mapper = new Mapper();
			var requestPayload = (RequestModel)payload;

			var endpointContext = (EndpointContext)context;
			System.Console.WriteLine(endpointContext.UsersCount);

			// Get User from DB
			var user = await dataAccessLayer.GetUserByEmail(requestPayload.Email);

			// Build JWT Token
			var jwtToken = _jwtService.GenerateJwt(user);

			// Create Activity Log
			var currentUserDisplayName = _clientService.GetCurrentUserDisplayName();
			await _activityLogService.UserLogined(user.Id, currentUserDisplayName, user.Id);

			// Map to Response Model
			var mappedResponseModel = mapper.MapToResponseModel(user, jwtToken);
			return ArfBlocksResults.Success(mappedResponseModel);
		}
	}
}