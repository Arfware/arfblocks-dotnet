using System;
using System.Threading.Tasks;
using Arfware.ArfBlocks.Core.Abstractions;
using Arfware.ArfBlocks.Core.RequestResults;
using Arfware.ArfBlocks.Core;
using Microsoft.AspNetCore.Http;
using TodoApp.Infrastructure.Services;
using Arfware.ArfBlocks.Core.Attributes;
using System.Threading;

namespace TodoApp.Application.RequestHandlers.Users.Queries.All
{
	// [InternalHandler]
	// [AuthorizedHandler]
	// [AllowAnonymousHandler]
	public class Handler : IRequestHandler
	{
		private readonly DataAccess _dataAccessLayer;
		private readonly DbValidationService _dbValidationService;
		public Handler(ArfBlocksDependencyProvider dependencyProvider, object dataAccess)
		{
			_dataAccessLayer = (DataAccess)dataAccess;
			_dbValidationService = dependencyProvider.GetInstance<DbValidationService>();
		}

		public async Task<ArfBlocksRequestResult> Handle(IRequestModel payload, EndpointContext context, CancellationToken cancellationToken)
		{
			// Get All Users from DB
			var allUsers = await _dataAccessLayer.GetAllUsers();

			// Build and Return Response
			var mappedTasks = new Mapper().Map(allUsers);
			return ArfBlocksResults.Success(mappedTasks);
		}
	}
}