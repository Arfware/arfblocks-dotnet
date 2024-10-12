using System;
using System.Threading.Tasks;
using Arfware.ArfBlocks.Core.Abstractions;
using Arfware.ArfBlocks.Core.RequestResults;
using Arfware.ArfBlocks.Core;
using Microsoft.AspNetCore.Http;
using TodoApp.Infrastructure.Services;
using System.Threading;

namespace TodoApp.Application.RequestHandlers.Tasks.Queries.Detail
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
			var requestPayload = (RequestModel)payload;

			var task = await _dataAccessLayer.GetFilledTaskById(requestPayload.TaskId);

			var mappedCompany = new Mapper().Map(task);
			return ArfBlocksResults.Success(mappedCompany);
		}
	}
}