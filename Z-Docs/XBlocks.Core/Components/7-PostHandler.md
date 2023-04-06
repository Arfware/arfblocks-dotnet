# PostHandler

PostHandler is an operation that runs after main Block operation. 

Overview:
```c#
namespace TodoApp.Application.RequestHandlers.Tasks.Commands.Create
{
	public class PostHandler : IPostRequestHandler
	{
		//...
		public PostHandler(ArfBlocksDependencyProvider dependencyProvider, object dataAccess)
		{
			//...
		}

		public async Task Handle(IRequestModel payload, ArfBlocksRequestResult response, CancellationToken cancellationToken)
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
	public class PostHandler : IPostRequestHandler
	{
		private readonly DataAccess dataAccessLayer;
		private readonly CurrentClientService _clientService;
		private readonly ActivityLogService _activityLogService;

		public PostHandler(ArfBlocksDependencyProvider dependencyProvider, object dataAccess)
		{
			dataAccessLayer = (DataAccess)dataAccess;
			_clientService = dependencyProvider.GetInstance<CurrentClientService>();
			_activityLogService = dependencyProvider.GetInstance<ActivityLogService>();
		}

		public async Task Handle(IRequestModel payload, ArfBlocksRequestResult response, CancellationToken cancellationToken)
		{
			var responsePayload = (ResponseModel)response.Payload;
			var currentClientId = _clientService.GetCurrentUserId();
			var currentUserDisplayName = _clientService.GetCurrentUserDisplayName();

			// Create Activity Log
			await _activityLogService.TaskCreated(currentClientId, currentUserDisplayName, responsePayload.Id);
		}
	}
}
```