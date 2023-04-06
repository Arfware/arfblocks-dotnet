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

namespace TodoApp.Application.RequestHandlers.Users.Commands.Login
{
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

		public async Task<ArfBlocksRequestResult> Handle(IRequestModel payload, CancellationToken cancellationToken)
		{
			var mapper = new Mapper();
			var requestPayload = (RequestModel)payload;

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