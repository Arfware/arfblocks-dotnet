using System;
using TodoApp.Domain.Entities;

namespace TodoApp.Application.RequestHandlers.Users.Queries.Me
{
	public class Mapper
	{
		public ResponseModel MapToResponseModel(User user)
		{
			return new ResponseModel()
			{
				UserId = user.Id,
				Email = user.Email,
				DisplayName = $"{user.FirstName} {user.LastName}",
				Department = new ResponseModel.DepartmentResponseModel()
				{
					Id = user.Department.Id,
					Name = user.Department.Name,
				}
			};
		}
	}
}