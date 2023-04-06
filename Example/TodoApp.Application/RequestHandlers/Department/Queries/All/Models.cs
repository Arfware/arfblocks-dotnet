using System;
using Arfware.ArfBlocks.Core.Abstractions;
using TodoApp.Domain.Entities;

namespace TodoApp.Application.RequestHandlers.Departments.Queries.All
{
	public class ResponseModel : IResponseModel<Array>
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
	}
}