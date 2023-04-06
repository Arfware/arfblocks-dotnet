using System;
using System.Collections.Generic;
using TodoApp.Domain.Entities;

namespace TodoApp.Application.RequestHandlers.Departments.Queries.All
{
	public class Mapper
	{
		public List<ResponseModel> Map(List<Department> departments)
		{
			var mappedDepartments = new List<ResponseModel>();

			departments?.ForEach((task) =>
			{
				mappedDepartments.Add(new ResponseModel()
				{
					Id = task.Id,
					Name = task.Name,
				});
			});

			return mappedDepartments;
		}
	}
}