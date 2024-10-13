# Verificator
Verificator is using for verification of Block operation.

Verificator consists 2 base operations:
- Verificate Actor
- Verificate Domain

Overview:
```c#
namespace TodoApp.Application.RequestHandlers.Tasks.Commands.Update
{
	public class Verificator : IRequestVerificator
	{
		//....
		public Verificator(ArfBlocksDependencyProvider dependencyProvider)
		{
			//....
		}

		public async Task VerificateActor(IRequestModel payload, EndpointContext context, CancellationToken cancellationToken)
		{
			//...
		}


		public async Task VerificateDomain(IRequestModel payload, EndpointContext context, CancellationToken cancellationToken)
		{
			//...
		}
	}
}
```

## Verificate Actor

In this method; you must verify actor for Block Operation.

```c#
public async Task VerificateActor(IRequestModel payload, EndpointContext context, CancellationToken cancellationToken)
{
	// Get Request Payload
	var requestPayload = (RequestModel)payload;
	var currentUserId = _clientService.GetCurrentUserId();

	await _dbVerificatorService.VerifyUserIsTaskCreator(requestPayload.TaskId, currentUserId);
}
```

## Verificate Domain

In this method; you must verify domain for Block operation.

```c#
public async Task VerificateDomain(IRequestModel payload, EndpointContext context, CancellationToken cancellationToken)
{
	// Get Request Payload
	var requestPayload = (RequestModel)payload;
	var currentUserId = _clientService.GetCurrentUserId();
	
	await _dbVerificator.VerifyTaskCanBeUpdated(requestPayload.TaskId, currentUserId);
}
```