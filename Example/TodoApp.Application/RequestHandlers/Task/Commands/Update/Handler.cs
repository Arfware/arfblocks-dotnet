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

namespace TodoApp.Application.RequestHandlers.Tasks.Commands.Update
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

		public async Task<ArfBlocksRequestResult> Handle(IRequestModel payload, CancellationToken cancellationToken)
		{
			var mapper = new Mapper();
			var currentClientId = _clientService.GetCurrentUserId();
			var requestPayload = (RequestModel)payload;

			// Map Request Payload to Entity
			var newEntity = mapper.MapToNewEntity(requestPayload);
			var oldEntity = await dataAccessLayer.GetById(requestPayload.Id);

			// Set Properties
			oldEntity.Title = newEntity.Title;
			oldEntity.Description = newEntity.Description;
			oldEntity.AssignedDepartmentId = newEntity.AssignedDepartmentId;

			// Update on DB
			await dataAccessLayer.Update(oldEntity);

			// Create Activity Log
			var currentUserDisplayName = _clientService.GetCurrentUserDisplayName();
			await _activityLogService.TaskUpdated(currentClientId, currentUserDisplayName, oldEntity.Id);

			// Map to Response Model
			var mappedResponseModel = mapper.MapToNewResponseModel(oldEntity);
			return ArfBlocksResults.Success(mappedResponseModel);
		}
	}
}