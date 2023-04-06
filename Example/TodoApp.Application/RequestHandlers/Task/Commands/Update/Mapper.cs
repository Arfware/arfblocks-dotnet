using System;
using TodoApp.Domain.Entities;

namespace TodoApp.Application.RequestHandlers.Tasks.Commands.Update
{
	public class Mapper
	{
		public TodoTask MapToNewEntity(RequestModel payload)
		{
			var mappedTask = new TodoTask()
			{
				Title = payload.Title,
				Description = payload.Description,
				AssignedDepartmentId = payload.AssignedDepartmentId,
			};

			return mappedTask;
		}

		public ResponseModel MapToNewResponseModel(TodoTask task)
		{
			return new ResponseModel()
			{
				Id = task.Id,
				Title = task.Title,
				Description = task.Description,
				AssignedDepartmentId = task.AssignedDepartmentId,
				CreatedById = task.CreatedById,
				Status = task.Status,
			};
		}
	}
}