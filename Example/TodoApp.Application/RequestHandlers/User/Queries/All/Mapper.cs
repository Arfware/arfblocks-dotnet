using System;
using System.Collections.Generic;
using TodoApp.Domain.Entities;

namespace TodoApp.Application.RequestHandlers.Users.Queries.All
{
	public class Mapper
	{
		public List<ResponseModel> Map(List<User> users)
		{
			var mappedUsers = new List<ResponseModel>();

			users?.ForEach((user) =>
			{
				mappedUsers.Add(new ResponseModel()
				{
					Id = user.Id,
					Email = user.Email,
					FirstName = user.FirstName,
					LastName = user.LastName,
					DisplayName = $"{user.FirstName} {user.LastName}",
					AssignedDepartment = new ResponseModel.AssignedDepartmentResponseModel()
					{
						Id = user.Department.Id,
						Name = user.Department.Name,
					},
				});
			});

			return mappedUsers;
		}
	}
}