# Validator
Validator is using for validating Block operation.

Validator consists 2 base operations:
- Validate Request Model
- Validate Domain 

Overview:
```c#
namespace TodoApp.Application.RequestHandlers.Tasks.Commands.Create
{
	public class Validator : IRequestValidator
	{
		//....
		public Validator(ArfBlocksDependencyProvider dependencyProvider)
		{
			//....
		}

		public void ValidateRequestModel(IRequestModel payload, IEndpointContext context, CancellationToken cancellationToken)
		{
			//....
		}

		public async Task ValidateDomain(IRequestModel payload, IEndpointContext context, CancellationToken cancellationToken)
		{
			//....
		}
	}
}
```

##  Validate Request Model

In this method; you must validate incoming request model for Block operation.

You can use 3rd Party Libraries for model validation like "FluentValidation":
```c#
public class RequestModel_Validator : AbstractValidator<RequestModel>
{
	public RequestModel_Validator()
	{
		RuleFor(x => x.Title)
			.NotEmpty().WithMessage("COMPANY_NAME_IS_EMPTY");

		RuleFor(x => x.AssignedDepartmentId)
			.NotNull().WithMessage("ASSIGNED_DEPARTMENT_ID_NOT_VALID")
			.NotEqual(Guid.Empty).WithMessage("ASSIGNED_DEPARTMENT_ID_NOT_VALID");
	}
}
```

```c#
public void ValidateRequestModel(IRequestModel payload, IEndpointContext context, CancellationToken cancellationToken)
{
	// Get Request Payload
	var requestModel = (RequestModel)payload;

	// Request Model Validation
	var validationResult = new RequestModel_Validator().Validate(requestModel);
	if (!validationResult.IsValid)
	{
		var errors = validationResult.ToString("~");
		throw new ArfBlocksValidationException(errors);
	}
}
```

##  Validate Domain

In this method; you must validate domain for Block operation.

```c#
public async Task ValidateDomain(IRequestModel payload, IEndpointContext context, CancellationToken cancellationToken)
{
	// Get Request Payload
	var requestModel = (RequestModel)payload;

	// DB Validations
	await _dbValidator.ValidateDepartmentExist(requestModel.AssignedDepartmentId);
}
```