namespace TodoApp.Application.RequestHandlers.Tasks.Commands.Update.Tests;

public class Success : IArfBlocksTest
{
	private DbContextOperations _dbContextOperation;

	private ArfBlocksDependencyProvider _dependencyProvider;

	// public void SwitchUser(CurrentUserModel user)
	// {
	// 	_dependencyProvider.Add<CurrentUserModel>(user);
	// }
	public void SetDependencies(ArfBlocksDependencyProvider dependencyProvider)
	{
		_dependencyProvider = dependencyProvider;
		var _dbContext = dependencyProvider.GetInstance<ApplicationDbContext>();
		_dbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
		_dbContextOperation = new DbContextOperations(_dbContext);
	}

	public async Task SetActor()
	{
		// NOP:
		await Task.CompletedTask;

		// SwitchUser(TestDefinitions.Actors.SystemUser);
	}

	// Course course = null;

	public async Task PrepareTest()
	{
		// NOP:
		await Task.CompletedTask;
		// course = TestDefinitions.Courses.DefaultCourse();
		// await _dbContextOperation.Create<Course>(course);
	}

	public async Task RunTest()
	{
		// NOP:
		await Task.CompletedTask;

		// var requestPaylaod = new Application.RequestHandlers.Vehicles.Commands.Create.RequestModel()
		// {
		// 	CourseId = course.Id,
		// 	Class = "C",
		// 	Year = "2008",
		// 	Brand = "Hyundai",
		// 	Model = "Accent",
		// 	Plate = "34AZ2848",
		// 	EngineVolume = "1.5",
		// 	EnginePower = "110 Hp",
		// 	IsActive = true,
		// 	Type = VehicleTypes.CarAndTruck,
		// 	FuelType = FuelTypes.Diesel,
		// 	GearboxType = GearBoxTypes.Manuel,
		// };

		// var requestOperator = new ArfBlocksRequestOperator(_dependencyProvider);
		// var response = await requestOperator.OperateInternalRequest<Application.RequestHandlers.Vehicles.Commands.Create.Handler>(requestPaylaod);

		// response.HasError.Should().Be(false);

		// // Response Payload Control
		// var responsePayload = (Application.RequestHandlers.Vehicles.Commands.Create.ResponseModel)response.Payload;
		// responsePayload.Id.Should().NotBeEmpty().And.NotBe(Guid.Empty);

		// // Db Control
		// var vehicleOnDb = await _dbContextOperation.GetById<Vehicle>(responsePayload.Id);
		// vehicleOnDb.CourseId.Should().Be(requestPaylaod.CourseId);
		// vehicleOnDb.Class.Should().Be(requestPaylaod.Class);
		// vehicleOnDb.Year.Should().Be(requestPaylaod.Year);
		// vehicleOnDb.Brand.Should().Be(requestPaylaod.Brand);
		// vehicleOnDb.Model.Should().Be(requestPaylaod.Model);
	}


}