using System.Text.Json;
using Arfware.ArfBlocks.Core.Abstractions;
using Arfware.ArfBlocks.Core.Models;

var builder = WebApplication.CreateBuilder(args);

// For Dotnet-Ef Commands
var configurations = builder.Configuration.GetSection("ProjectNameConfigurations").Get<ProjectNameConfigurations>();
var dbContext = new ApplicationDbContext(configurations.RelationalDbConfiguration);
builder.Services.AddSingleton<ApplicationDbContext>(dbContext);

var defaultSeeder = new DefaultSeeder(dbContext);
await defaultSeeder.Seed();
System.Console.WriteLine("Default DB Seeding Completed.");

// ArfBlocks Dependencies
builder.Services.AddArfBlocks(options =>
{
	options.ApplicationProjectNamespace = "TodoApp.Application";
	options.ConfigurationSection = builder.Configuration.GetSection("ProjectNameConfigurations");
	options.LogLevel = LogLevels.Warning;
	options.PreOperateHandler = typeof(TodoApp.Application.DefaultHandlers.Operators.Commands.PreOperate.Handler);
	options.PostOperateHandler = typeof(TodoApp.Application.DefaultHandlers.Operators.Commands.PostOperate.Handler);
	// options.PreOperateAction = (EndpointModel endpoint, IRequestModel requestPayload) =>
	// {
	// 	System.Console.WriteLine("\n\n--------------------");
	// 	System.Console.WriteLine($"Object: {endpoint.ObjectName} | Action: {endpoint.ActionName} | Type: {endpoint.EndpointType}");
	// 	System.Console.WriteLine(JsonSerializer.Serialize(requestPayload as object, new JsonSerializerOptions() { WriteIndented = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase }));
	// 	System.Console.WriteLine("--------------------\n\n");
	// };
	// options.PostOperateAction = (EndpointModel endpoint, ArfBlocksRequestResult response) =>
	// {
	// 	System.Console.WriteLine("\n\n--------------------");
	// 	System.Console.WriteLine($"Object: {endpoint.ObjectName} | Action: {endpoint.ActionName} | Type: {endpoint.EndpointType}");
	// 	System.Console.WriteLine($"Status Code: {response.StatusCode} | ResponseCode: {response.Code} | Has Error: {response.HasError} | ErrorCode?: {response.Error?.Message}");
	// 	System.Console.WriteLine(JsonSerializer.Serialize(response.Payload, new JsonSerializerOptions() { WriteIndented = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase }));
	// 	System.Console.WriteLine("--------------------\n\n");
	// };
});

string DefaultCorsPolicy = "DefaultCorsPolicy";
builder.Services.AddCors(options =>
			{
				// Development Cors Policy
				options.AddPolicy(name: DefaultCorsPolicy,
					builder =>
					{
						builder.AllowAnyHeader()
						.AllowAnyMethod()
						.AllowAnyOrigin();
					});
			});


var app = builder.Build();

app.UseCors(DefaultCorsPolicy);

app.UseArfBlocksRequestHandlers((Action<UseRequestHandlersOptions>)(options =>
{
	options.AuthorizationType = UseRequestHandlersOptions.AuthorizationTypes.Jwt;
	options.AuthorizationPolicy = UseRequestHandlersOptions.AuthorizationPolicies.AssumeAllAuthorized;
	options.JwtAuthorizationOptions = new UseRequestHandlersOptions.JwtAuthorizationOptionsModel()
	{
		Audience = JwtService.Audience,
		Secret = JwtService.Secret,
	};
}));

app.Run();