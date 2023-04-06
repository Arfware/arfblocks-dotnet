# Test Ignoring

You can ignore tests when running all tests.

```c#
await app.RunTests(app.Configuration, options =>
{
	//......
	options.IgnoredTestList = new List<Type>()
	{
		typeof(TodoApp.Application.RequestHandlers.Tasks.Commands.Complete.Tests.Success)
	};
	//......
});
```