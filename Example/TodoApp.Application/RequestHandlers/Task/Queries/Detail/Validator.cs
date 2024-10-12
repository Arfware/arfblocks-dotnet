using System;
using System.Threading.Tasks;
using Arfware.ArfBlocks.Core.Abstractions;
using Microsoft.AspNetCore.Http;
using Arfware.ArfBlocks.Core.Exceptions;
using Arfware.ArfBlocks.Core;
using TodoApp.Infrastructure.Services;
using System.Threading;

namespace TodoApp.Application.RequestHandlers.Tasks.Queries.Detail
{
	public class Validator : IRequestValidator
	{
		private readonly DbValidationService _dbValidator;
		public Validator(ArfBlocksDependencyProvider dependencyProvider)
		{
			_dbValidator = dependencyProvider.GetInstance<DbValidationService>();
		}

		public void ValidateRequestModel(IRequestModel payload, IEndpointContext context, CancellationToken cancellationToken)
		{
			// Get Request Payload
			var requestModel = (RequestModel)payload;

			// Request Model Validation
			var validationResult = new RequestModel_Validator().Validate(requestModel);
			if (!validationResult.IsValid)
			{
				var errors = validationResult.ToString("~");
				throw new ArfBlocksValidationException(errors);
			}
		}

		public async Task ValidateDomain(IRequestModel payload, IEndpointContext context, CancellationToken cancellationToken)
		{
			// Get Request Payload
			var requestModel = (RequestModel)payload;

			// DB Validations
			await _dbValidator.ValidateTaskExist(requestModel.TaskId);
		}
	}
}