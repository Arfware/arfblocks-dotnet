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