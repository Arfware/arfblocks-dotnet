using Arfware.ArfBlocks.Core.Abstractions;
using BusinessModules.Management.Infrastructure.Services;
using TodoApp.Infrastructure.Services;
using Arfware.ArfBlocks.Core;

namespace TodoApp.Application.RequestHandlers.Tasks.Commands.Complete
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

		public async Task Handle(IRequestModel payload, ArfBlocksRequestResult response, EndpointContext context, CancellationToken cancellationToken)
		{
			var requestPayload = (RequestModel)payload;
			var responsePayload = (ResponseModel)response.Payload;

			// Create Activity Log
			var currentClientId = _clientService.GetCurrentUserId();
			var currentUserDisplayName = _clientService.GetCurrentUserDisplayName();

			await _activityLogService.TaskCompleted(currentClientId, currentUserDisplayName, responsePayload.TaskId);
		}
	}
}