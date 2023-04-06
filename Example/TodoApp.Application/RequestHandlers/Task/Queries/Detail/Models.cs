using System;
using System.Collections.Generic;
using TodoApp.Domain.Entities;
using FluentValidation;
using Arfware.ArfBlocks.Core.Abstractions;

namespace TodoApp.Application.RequestHandlers.Tasks.Queries.Detail
{
	public class ResponseModel : IResponseModel
	{
		public Guid Id { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public TodoTaskStatus Status { get; set; }
		public AssignedDepartmentResponseModel AssignedDepartment { get; set; }
		public UserResponseModel CreatedBy { get; set; }

		public class UserResponseModel
		{
			public Guid Id { get; set; }
			public string DisplayName { get; set; }
			public Guid DepartmentId { get; set; }
		}

		public class AssignedDepartmentResponseModel
		{
			public Guid Id { get; set; }
			public string Name { get; set; }
		}
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