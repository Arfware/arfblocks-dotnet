using System;
using System.Threading.Tasks;
using Arfware.ArfBlocks.Core.Abstractions;
using Arfware.ArfBlocks.Core.RequestResults;
using Arfware.ArfBlocks.Core;
using Microsoft.AspNetCore.Http;
using TodoApp.Infrastructure.Services;
using System.Threading;

namespace TodoApp.Application.RequestHandlers.Tasks.Queries.MyTasks
{
	public class Handler : IRequestHandler
	{
		private readonly DataAccess _dataAccessLayer;
		private readonly CurrentClientService _clientService;
		private readonly DbValidationService _dbValidationService;
		public Handler(ArfBlocksDependencyProvider dependencyProvider, object dataAccess)
		{
			_dataAccessLayer = (DataAccess)dataAccess;
			_clientService = dependencyProvider.GetInstance<CurrentClientService>();
			_dbValidationService = dependencyProvider.GetInstance<DbValidationService>();
		}

		public async Task<ArfBlocksRequestResult> Handle(IRequestModel payload, CancellationToken cancellationToken)
		{
			var currentUserId = _clientService.GetCurrentUserId();

			var myTasks = await _dataAccessLayer.GetPendingTasks(currentUserId);

			var mappedTasks = new Mapper().Map(myTasks);
			return ArfBlocksResults.Success(mappedTasks);
		}
	}
}