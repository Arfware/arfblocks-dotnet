using System;
using System.Threading.Tasks;
using Arfware.ArfBlocks.Core.Abstractions;
using Arfware.ArfBlocks.Core.RequestResults;
using Arfware.ArfBlocks.Core;
using Microsoft.AspNetCore.Http;
using BusinessModules.Management.Infrastructure.Services;
using TodoApp.Infrastructure.Services;
using System.Threading;

namespace TodoApp.Application.RequestHandlers.Tasks.Commands.Delete
{
	public class Handler : IRequestHandler
	{
		private readonly DataAccess dataAccessLayer;
		private readonly CurrentClientService _clientService;
		private readonly ActivityLogService _activityLogService;
		public Handler(ArfBlocksDependencyProvider depencyProvider, object dataAccess)
		{
			dataAccessLayer = (DataAccess)dataAccess;
			_clientService = depencyProvider.GetInstance<CurrentClientService>();
			_activityLogService = depencyProvider.GetInstance<ActivityLogService>();
		}

		public async Task<ArfBlocksRequestResult> Handle(IRequestModel payload, CancellationToken cancellationToken)
		{
			// Get Payload
			var requestPayload = (RequestModel)payload;

			// Get and Delete
			var company = await dataAccessLayer.GetById(requestPayload.TaskId);
			await dataAccessLayer.Delete(company);

			// Create Activity Log
			var currentUserId = _clientService.GetCurrentUserId();
			var currentUserDisplayName = _clientService.GetCurrentUserDisplayName();
			await _activityLogService.TaskDeleted(currentUserId, currentUserDisplayName, company.Id);

			// Create Reponse
			var responseModel = new ResponseModel() { TaskId = requestPayload.TaskId };
			return ArfBlocksResults.Success(responseModel);
		}
	}
}