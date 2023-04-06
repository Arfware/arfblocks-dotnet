using System;
using Arfware.ArfBlocks.Core.Abstractions;
using FluentValidation;
using TodoApp.Domain.Entities;

namespace TodoApp.Application.RequestHandlers.Users.Commands.Login
{
	public class ResponseModel : IResponseModel
	{
		public string JwtToken { get; set; }
		public Guid UserId { get; set; }
		public string Email { get; set; }
		public string DisplayName { get; set; }
		public DepartmentResponseModel Department { get; set; }

		public class DepartmentResponseModel
		{
			public Guid Id { get; set; }
			public string Name { get; set; }
		}
	}

	public class RequestModel : IRequestModel
	{
		public string Email { get; set; }
	}


	public class RequestModel_Validator : AbstractValidator<RequestModel>
	{
		public RequestModel_Validator()
		{
			RuleFor(x => x.Email)
				.NotEmpty().WithMessage("EMAIL_NOT_VALID")
				.EmailAddress().WithMessage("EMAIL_NOT_VALID");
		}
	}
}