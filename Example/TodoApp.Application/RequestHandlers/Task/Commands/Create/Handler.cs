using System.Threading.Tasks;
using Arfware.ArfBlocks.Core.Abstractions;
using Arfware.ArfBlocks.Core.RequestResults;
using Arfware.ArfBlocks.Core;
using Microsoft.AspNetCore.Http;
using BusinessModules.Management.Infrastructure.Services;
using TodoApp.Domain.Entities;
using System;
using TodoApp.Infrastructure.Services;
using System.Threading;

namespace TodoApp.Application.RequestHandlers.Tasks.Commands.Create
{
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

		public async Task<ArfBlocksRequestResult> Handle(IRequestModel payload, IEndpointContext context, CancellationToken cancellationToken)
		{
			var mapper = new Mapper();
			var currentClientId = _clientService.GetCurrentUserId();
			var requestPayload = (RequestModel)payload;

			// Map Request Payload to Entity
			var task = mapper.MapToNewEntity(requestPayload);

			// Set Properties
			task.CreatedById = currentClientId;
			task.Status = TodoTaskStatus.Pending;
			task.StatusChangedAt = DateTime.UtcNow;

			// Add to DB
			await dataAccessLayer.Add(task);

			// Create Activity Log
			var currentUserDisplayName = _clientService.GetCurrentUserDisplayName();
			await _activityLogService.TaskCreated(currentClientId, currentUserDisplayName, task.Id);

			// Map to Response Model
			var mappedResponseModel = mapper.MapToNewResponseModel(task);
			return ArfBlocksResults.Success(mappedResponseModel);
		}
	}
}