# Response Model

Response model represents request response data scheme.
All Blocks must have response model.

Example:
```c#
namespace TodoApp.Application.RequestHandlers.Tasks.Commands.Create
{
	public class ResponseModel : IResponseModel
	{
		public Guid Id { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public Guid AssignedDepartmentId { get; set; }
		public Guid CreatedById { get; set; }
		public TodoTaskStatus Status { get; set; }
	}
}
```