using System;
using System.Threading.Tasks;
using Arfware.ArfBlocks.Core.Abstractions;
using Arfware.ArfBlocks.Core.RequestResults;
using Arfware.ArfBlocks.Core;
using Microsoft.AspNetCore.Http;
using TodoApp.Infrastructure.Services;
using System.Threading;

namespace TodoApp.Application.RequestHandlers.Tasks.Queries.All
{
	public class Handler : IRequestHandler
	{
		private readonly DataAccess _dataAccessLayer;
		private readonly DbValidationService _dbValidationService;
		public Handler(ArfBlocksDependencyProvider dependencyProvider, object dataAccess)
		{
			_dataAccessLayer = (DataAccess)dataAccess;
			_dbValidationService = dependencyProvider.GetInstance<DbValidationService>();
		}

		public async Task<ArfBlocksRequestResult> Handle(IRequestModel payload, IEndpointContext context, CancellationToken cancellationToken)
		{
			var allTasks = await _dataAccessLayer.GetAllTasks();

			var mappedTasks = new Mapper().Map(allTasks);
			return ArfBlocksResults.Success(mappedTasks);
		}
	}
}