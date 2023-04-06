# Built-in Dependency Provider

You can add dependencies to dependency provider for using in blocks.

Adding Example:
```c#
public class ApplicationDependencyProvider : ArfBlocksDependencyProvider
{
	public ApplicationDependencyProvider(IHttpContextAccessor httpContextAccessor, ProjectNameConfigurations projectConfigurations)
	{
		// Instances
		base.Add<RelationalDbConfiguration>(projectConfigurations.RelationalDbConfiguration);
		base.Add<EnvironmentConfiguration>(projectConfigurations.EnvironmentConfiguration);
		base.Add<IHttpContextAccessor>(httpContextAccessor);
		base.Add<IdentifiedClient>(new IdentifiedClient());

		// Types
		base.Add<ApplicationDbContext, ApplicationDbContext>();
		base.Add<DbValidationService, DbValidationService>();
		base.Add<EnvironmentService, EnvironmentService>();
		base.Add<ClientProvider, ClientProvider>();
		base.Add<CurrentClientService, CurrentClientService>();
		base.Add<IJwtService, JwtService>();
		base.Add<ActivityLogService, ActivityLogService>();
	}
}
```

Using Example:
```c#
private readonly ActivityLogService _activityLogService;

public Handler(ArfBlocksDependencyProvider dependencyProvider, object dataAccess)
{
	//...
	_activityLogService = dependencyProvider.GetInstance<ActivityLogService>();
	//...
}

//...
await _activityLogService.TaskCompleted(currentClientId, currentUserDisplayName, task.Id);
//...

```
