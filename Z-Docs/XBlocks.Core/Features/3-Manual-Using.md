# Manual Using

You can use any block in API requests.

Example:
```c#
// Queries
[HttpGet("[action]")]
public async Task<ActionResult<List<Application.RequestHandlers.Tasks.Queries.All.ResponseModel>>> All()
{
	return await _requestOperator.OperateHttpRequest<Application.RequestHandlers.Tasks.Queries.All.Handler>(null);
}
// Commands
[HttpPost("[action]")]
public async Task<ActionResult<List<Application.RequestHandlers.Tasks.Commands.Create.ResponseModel>>> Create(Application.RequestHandlers.Tasks.Commands.Create.RequestModel payload)
{
	return await _requestOperator.OperateHttpRequest<Application.RequestHandlers.Tasks.Commands.Create.Handler>(payload);
}

```
