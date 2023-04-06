using System;
using Arfware.ArfBlocks.Core.Abstractions;
using FluentValidation;
using TodoApp.Domain.Entities;

namespace TodoApp.Application.RequestHandlers.Tasks.Commands.Reject
{
	public class ResponseModel : IResponseModel
	{
		public Guid TaskId { get; set; }
		public TodoTaskStatus Status { get; set; }
	}

	public class RequestModel : IRequestModel
	{
		public Guid TaskId { get; set; }
		public string Message { get; set; }
	}


	public class RequestModel_Validator : AbstractValidator<RequestModel>
	{
		public RequestModel_Validator()
		{
			RuleFor(x => x.TaskId)
				.NotNull().WithMessage("TASK_ID_NOT_VALID")
				.NotEqual(Guid.Empty).WithMessage("TASK_ID_NOT_VALID");
		}
	}
}