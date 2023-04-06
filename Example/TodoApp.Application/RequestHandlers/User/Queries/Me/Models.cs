using System;
using Arfware.ArfBlocks.Core.Abstractions;
using FluentValidation;
using TodoApp.Domain.Entities;

namespace TodoApp.Application.RequestHandlers.Users.Queries.Me
{
	public class ResponseModel : IResponseModel
	{
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
}