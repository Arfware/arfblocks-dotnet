using System;
using System.Threading.Tasks;
using Arfware.ArfBlocks.Core.Abstractions;
using Microsoft.AspNetCore.Http;
using Arfware.ArfBlocks.Core.Exceptions;
using Arfware.ArfBlocks.Core;
using TodoApp.Infrastructure.RelationalDB;
using Microsoft.EntityFrameworkCore;
using TodoApp.Domain.BusinessRules;
using TodoApp.Infrastructure.Services;
using System.Threading;

namespace TodoApp.Application.RequestHandlers.Tasks.Commands.Reject
{
	public class Verificator : IRequestVerificator
	{
		private readonly ApplicationDbContext _dbContext;
		private readonly CurrentClientService _clientService;

		public Verificator(ArfBlocksDependencyProvider dependencyProvider)
		{
			_dbContext = dependencyProvider.GetInstance<ApplicationDbContext>();
			_clientService = dependencyProvider.GetInstance<CurrentClientService>();
		}

		public async Task VerificateActor(IRequestModel payload, EndpointContext context, CancellationToken cancellationToken)
		{
			// Get Request Payload
			var requestPayload = (RequestModel)payload;
			var currentUserId = _clientService.GetCurrentUserId();

			// Get Task from Database
			var task = await _dbContext.Tasks
											.FirstOrDefaultAsync(r => r.Id == requestPayload.TaskId);

			var currentUser = await _dbContext.Users
											.FirstOrDefaultAsync(u => u.Id == currentUserId);

			// Check Result
			var verificationResult = TaskBusinessRules.CheckForReject(task, currentUser);
			if (verificationResult.HasError)
				throw new ArfBlocksVerificationException(verificationResult.ErrorCode);
		}

		public async Task VerificateDomain(IRequestModel payload, EndpointContext context, CancellationToken cancellationToken)
		{
			await Task.CompletedTask;
		}
	}
}