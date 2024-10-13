# PreHandler

PreHandler is an operation that runs before main Block operation. 

Overview:
```c#
namespace TodoApp.Application.RequestHandlers.Tasks.Commands.Create
{
	public class PreHandler : IPreRequestHandler
	{
		//...
		public PreHandler(ArfBlocksDependencyProvider dependencyProvider)
		{
			//...
		}

		public async Task Handle(IRequestModel payload, EndpointContext context, CancellationToken cancellationToken)
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
	public class PreHandler : IPreRequestHandler
	{
		private readonly CurrentClientService _clientService;
		private readonly ActivityLogService _activityLogService;

		public PreHandler(ArfBlocksDependencyProvider dependencyProvider)
		{
			_clientService = dependencyProvider.GetInstance<CurrentClientService>();
			_activityLogService = dependencyProvider.GetInstance<ActivityLogService>();
		}

		public async Task Handle(IRequestModel payload, EndpointContext context, CancellationToken cancellationToken)
		{
			var currentClientId = _clientService.GetCurrentUserId();
			var currentUserDisplayName = _clientService.GetCurrentUserDisplayName();

			System.Console.WriteLine($"{currentClientId} ::: {currentUserDisplayName}");

			await Task.CompletedTask;
		}
	}
}
```