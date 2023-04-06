using System;
using System.Collections.Generic;
using Arfware.ArfBlocks.Core.Abstractions;
using TodoApp.Domain.Entities;

namespace TodoApp.Application.RequestHandlers.Tasks.Queries.MyTasks
{
	public class ResponseModel : IResponseModel<Array>
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
		}

		public class AssignedDepartmentResponseModel
		{
			public Guid Id { get; set; }
			public string Name { get; set; }
		}
	}
}