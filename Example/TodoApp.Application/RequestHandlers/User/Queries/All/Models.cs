using System;
using System.Collections.Generic;
using Arfware.ArfBlocks.Core.Abstractions;
using TodoApp.Domain.Entities;

namespace TodoApp.Application.RequestHandlers.Users.Queries.All
{
	public class ResponseModel : IResponseModel<Array>
	{
		public Guid Id { get; set; }
		public string Email { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string DisplayName { get; set; }
		public AssignedDepartmentResponseModel AssignedDepartment { get; set; }

		public class AssignedDepartmentResponseModel
		{
			public Guid Id { get; set; }
			public string Name { get; set; }
		}
	}
}