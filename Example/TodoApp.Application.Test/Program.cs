var builder = WebApplication.CreateBuilder(args);

// ArfBlocks Dependencies
builder.Services.AddArfBlocks(options =>
{
	options.ApplicationProjectNamespace = "TodoApp.Application";
	options.ConfigurationSection = builder.Configuration.GetSection("ProjectNameConfigurations");
	options.LogLevel = LogLevels.Warning;
});

builder.Services.AddArfBlocksTests(options =>
{
	options.ApplicationProjectNamespace = "TodoApp.Application";
	options.LogLevel = LogLevels.Warning;
});

var configurations = builder.Configuration.GetSection("ProjectNameConfigurations").Get<ProjectNameConfigurations>();
// var dbContext = new ApplicationDbContext(new ApplicationDbContext(configurations.RelationalDbConfiguration));

var app = builder.Build();

await app.RunTests(app.Configuration, options =>
{
	options.TestConfigurationsType = typeof(TestOperations);

	// Run Only Selected Tests
	options.SelectedTestList = new List<Type>()
	{
		typeof(TodoApp.Application.RequestHandlers.Tasks.Commands.Complete.Tests.Success)
	};

	// Skip Running Ignored Tests 
	// options.IgnoredTestList = new List<Type>()
	// {
	// 	typeof(TodoApp.Application.RequestHandlers.Tasks.Commands.Complete.Tests.Success)
	// };
});
