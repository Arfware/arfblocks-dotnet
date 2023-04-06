using System;
using Arfware.ArfBlocks.Core.Abstractions;
using FluentValidation;
using TodoApp.Domain.Entities;

namespace TodoApp.Application.RequestHandlers.Tasks.Commands.Update
{
	public class ResponseModel : IResponseModel
	{
		public Guid Id { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public Guid AssignedDepartmentId { get; set; }
		public Guid CreatedById { get; set; }
		public TodoTaskStatus Status { get; set; }
	}

	public class RequestModel : IRequestModel
	{
		public Guid Id { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public Guid AssignedDepartmentId { get; set; }
	}


	public class RequestModel_Validator : AbstractValidator<RequestModel>
	{
		public RequestModel_Validator()
		{
			RuleFor(x => x.Id)
				.NotNull().WithMessage("TASK_ID_NOT_VALID")
				.NotEqual(Guid.Empty).WithMessage("TASK_ID_NOT_VALID");

			RuleFor(x => x.Title)
				.NotEmpty().WithMessage("TITLE_IS_EMPTY");

			RuleFor(x => x.AssignedDepartmentId)
				.NotNull().WithMessage("ASSIGNED_DEPARTMENT_ID_NOT_VALID")
				.NotEqual(Guid.Empty).WithMessage("ASSIGNED_DEPARTMENT_ID_NOT_VALID");
		}
	}
}