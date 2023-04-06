using System;
using Arfware.ArfBlocks.Core.Abstractions;
using FluentValidation;

namespace TodoApp.Application.RequestHandlers.Tasks.Commands.Delete
{
	public class ResponseModel : IResponseModel
	{
		public Guid TaskId { get; set; }
	}

	public class RequestModel : IRequestModel
	{
		public Guid TaskId { get; set; }
	}

	// VALIDATOR 
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