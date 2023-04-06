using System;
using System.Collections.Generic;
using TodoApp.Domain.Entities;

namespace TodoApp.Application.RequestHandlers.Tasks.Queries.MyTasks
{
	public class Mapper
	{
		public List<ResponseModel> Map(List<TodoTask> tasks)
		{
			var mappedTasks = new List<ResponseModel>();

			tasks?.ForEach((task) =>
			{
				mappedTasks.Add(new ResponseModel()
				{
					Id = task.Id,
					Title = task.Title,
					Description = task.Description,
					Status = task.Status,
					AssignedDepartment = new ResponseModel.AssignedDepartmentResponseModel()
					{
						Id = task.AssignedDepartment.Id,
						Name = task.AssignedDepartment.Name,
					},
					CreatedBy = new ResponseModel.UserResponseModel()
					{
						Id = task.CreatedBy.Id,
						DisplayName = $"{task.CreatedBy.FirstName} {task.CreatedBy.LastName}"
					},
				});
			});

			return mappedTasks;
		}
	}
}