using System;
using System.Threading.Tasks;
using Arfware.ArfBlocks.Core.Abstractions;
using Microsoft.AspNetCore.Http;
using Arfware.ArfBlocks.Core.Exceptions;
using Arfware.ArfBlocks.Core;
using TodoApp.Infrastructure.Services;
using System.Threading;

namespace TodoApp.Application.RequestHandlers.Users.Queries.Me
{
	public class Validator : IRequestValidator
	{
		private readonly DbValidationService _dbValidator;
		private readonly CurrentClientService _clientService;
		public Validator(ArfBlocksDependencyProvider dependencyProvider)
		{
			_dbValidator = dependencyProvider.GetInstance<DbValidationService>();
			_clientService = dependencyProvider.GetInstance<CurrentClientService>();
		}

		public void ValidateRequestModel(IRequestModel payload, IEndpointContext context, CancellationToken cancellationToken)
		{ }

		public async Task ValidateDomain(IRequestModel payload, IEndpointContext context, CancellationToken cancellationToken)
		{
			var currentUserId = _clientService.GetCurrentUserId();

			// DB Validations
			await _dbValidator.ValidateUserExist(currentUserId);
		}
	}
}