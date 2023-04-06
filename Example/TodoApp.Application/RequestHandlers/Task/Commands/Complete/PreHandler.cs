using Arfware.ArfBlocks.Core.Abstractions;
using BusinessModules.Management.Infrastructure.Services;
using TodoApp.Infrastructure.Services;
using Arfware.ArfBlocks.Core;

namespace TodoApp.Application.RequestHandlers.Tasks.Commands.Complete
{
	public class PreHandler : IPreRequestHandler
	{
		private readonly DataAccess dataAccessLayer;
		private readonly CurrentClientService _clientService;
		private readonly ActivityLogService _activityLogService;

		public PreHandler(ArfBlocksDependencyProvider dependencyProvider, object dataAccess)
		{
			dataAccessLayer = (DataAccess)dataAccess;
			_clientService = dependencyProvider.GetInstance<CurrentClientService>();
			_activityLogService = dependencyProvider.GetInstance<ActivityLogService>();
		}

		public async Task Handle(IRequestModel payload, CancellationToken cancellationToken)
		{
			var requestPayload = (RequestModel)payload;

			// Create Activity Log
			var currentClientId = _clientService.GetCurrentUserId();
			var currentUserDisplayName = _clientService.GetCurrentUserDisplayName();

			System.Console.WriteLine($"{currentClientId} ::: {currentUserDisplayName}");

			await Task.CompletedTask;
		}
	}
}