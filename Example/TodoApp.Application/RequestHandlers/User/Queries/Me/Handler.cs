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

namespace TodoApp.Application.RequestHandlers.Users.Queries.Me
{
	public class Handler : IRequestHandler
	{
		private readonly DataAccess dataAccessLayer;
		private readonly CurrentClientService _clientService;

		public Handler(ArfBlocksDependencyProvider dependencyProvider, object dataAccess)
		{
			dataAccessLayer = (DataAccess)dataAccess;
			_clientService = dependencyProvider.GetInstance<CurrentClientService>();
		}

		public async Task<ArfBlocksRequestResult> Handle(IRequestModel payload, IEndpointContext context, CancellationToken cancellationToken)
		{
			var mapper = new Mapper();
			var currentClientId = _clientService.GetCurrentUserId();

			// Get User from DB
			var user = await dataAccessLayer.GetUserById(currentClientId);

			// Map to Response Model
			var mappedResponseModel = mapper.MapToResponseModel(user);
			return ArfBlocksResults.Success(mappedResponseModel);
		}
	}
}