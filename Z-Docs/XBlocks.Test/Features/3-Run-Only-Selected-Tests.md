# Run Only Selected Tests

You can run only selected tests.

```c#
await app.RunTests(app.Configuration, options =>
{
	//...
	options.SelectedTestList = new List<Type>()
	{
		typeof(TodoApp.Application.RequestHandlers.Tasks.Commands.Complete.Tests.Success)
	};
	//...
});
```