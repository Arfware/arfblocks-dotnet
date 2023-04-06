using System;
using TodoApp.Domain.Entities;

namespace TodoApp.Application.RequestHandlers.Tasks.Commands.Reject
{
	public class Mapper
	{
		public ResponseModel MapToNewResponseModel(TodoTask task)
		{
			return new ResponseModel()
			{
				TaskId = task.Id,
				Status = task.Status,
			};
		}
	}
}