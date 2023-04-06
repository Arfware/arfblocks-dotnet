# Request Model

Request model represents user request data. 
Some blocks does not have request models like "/All".

Example:
```c#
namespace TodoApp.Application.RequestHandlers.Tasks.Commands.Create
{
	public class RequestModel : IRequestModel
	{
		public string Title { get; set; }
		public string Description { get; set; }
		public Guid AssignedDepartmentId { get; set; }
	}
}
```