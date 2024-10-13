using System;
using System.Threading.Tasks;
using System.Collections;
using Arfware.ArfBlocks.Core.Abstractions;
using Arfware.ArfBlocks.Core.RequestResults;
using Arfware.ArfBlocks.Core;
using Microsoft.AspNetCore.Http;
using BusinessModules.Management.Infrastructure.Services;
using TodoApp.Infrastructure.Services;
using System.Threading;
using Arfware.ArfBlocks.Core.Attributes;

namespace TodoApp.Application.RequestHandlers.Tasks.Commands.Complete
{
	[InternalHandler]
	public class Handler : IRequestHandler
	{
		private readonly DataAccess dataAccessLayer;
		private readonly CurrentClientService _clientService;
		private readonly ActivityLogService _activityLogService;

		public Handler(ArfBlocksDependencyProvider dependencyProvider, object dataAccess)
		{
			dataAccessLayer = (DataAccess)dataAccess;
			_clientService = dependencyProvider.GetInstance<CurrentClientService>();
			_activityLogService = dependencyProvider.GetInstance<ActivityLogService>();
		}

		public async Task<ArfBlocksRequestResult> Handle(IRequestModel payload, EndpointContext context, CancellationToken cancellationToken)
		{
			var mapper = new Mapper();
			var currentClientId = _clientService.GetCurrentUserId();
			var requestPayload = (RequestModel)payload;

			// Map Request Payload to Entity
			var task = await dataAccessLayer.GetById(requestPayload.TaskId);

			// Set Properties
			task.Status = Domain.Entities.TodoTaskStatus.Completed;
			task.StatusChangedAt = DateTime.UtcNow;

			// Update on DB
			await dataAccessLayer.Update(task);

			// Create Activity Log
			var currentUserDisplayName = _clientService.GetCurrentUserDisplayName();
			await _activityLogService.TaskCompleted(currentClientId, currentUserDisplayName, task.Id);

			// Map to Response Model
			var mappedResponseModel = mapper.MapToNewResponseModel(task);
			return ArfBlocksResults.Success(mappedResponseModel);
		}
	}
}