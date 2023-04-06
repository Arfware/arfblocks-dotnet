using System;
using System.Threading.Tasks;
using Arfware.ArfBlocks.Core.Abstractions;
using Microsoft.AspNetCore.Http;
using Arfware.ArfBlocks.Core.Exceptions;
using Arfware.ArfBlocks.Core;
using TodoApp.Infrastructure.Services;
using System.Threading;

namespace TodoApp.Application.RequestHandlers.Tasks.Commands.Delete
{
	public class Validator : IRequestValidator
	{
		private readonly DbValidationService _dbValidator;
		public Validator(ArfBlocksDependencyProvider dependencyProvider)
		{
			_dbValidator = dependencyProvider.GetInstance<DbValidationService>();
		}

		public void ValidateRequestModel(IRequestModel payload, CancellationToken cancellationToken)
		{
			// Get Request Payload
			var requestPayload = (RequestModel)payload;

			// Request Model Validation
			var validationResult = new RequestModel_Validator().Validate(requestPayload);
			if (!validationResult.IsValid)
			{
				var errors = validationResult.ToString("~");
				throw new ArfBlocksValidationException(errors);
			}
		}

		public async Task ValidateDomain(IRequestModel payload, CancellationToken cancellationToken)
		{
			// Get Request Payload
			var requestPayload = (RequestModel)payload;

			// DB Validations
			await _dbValidator.ValidateTaskExist(requestPayload.TaskId);
		}
	}
}