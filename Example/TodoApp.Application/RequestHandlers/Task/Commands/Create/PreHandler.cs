using Arfware.ArfBlocks.Core.Abstractions;
using Arfware.ArfBlocks.Core;
using BusinessModules.Management.Infrastructure.Services;
using TodoApp.Infrastructure.Services;

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

		public async Task Handle(IRequestModel payload, IEndpointContext context, CancellationToken cancellationToken)
		{
			var currentClientId = _clientService.GetCurrentUserId();
			var currentUserDisplayName = _clientService.GetCurrentUserDisplayName();

			System.Console.WriteLine($"{currentClientId} ::: {currentUserDisplayName}");

			await Task.CompletedTask;
		}
	}
}