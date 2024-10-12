using System;
using System.Threading.Tasks;
using Arfware.ArfBlocks.Core.Abstractions;
using Arfware.ArfBlocks.Core.RequestResults;
using Arfware.ArfBlocks.Core;
using Microsoft.AspNetCore.Http;
using TodoApp.Infrastructure.Services;
using System.Threading;

namespace TodoApp.Application.RequestHandlers.Departments.Queries.All
{
	public class PostHandler : IPostRequestHandler
	{
		private readonly DataAccess _dataAccessLayer;
		public PostHandler(ArfBlocksDependencyProvider dependencyProvider, object dataAccess)
		{
			_dataAccessLayer = (DataAccess)dataAccess;
		}

		public async Task Handle(IRequestModel payload, ArfBlocksRequestResult response, IEndpointContext context, CancellationToken cancellationToken)
		{
			// NOP:
			await Task.CompletedTask;

			System.Console.WriteLine("PostHandler worked...");
		}
	}
}