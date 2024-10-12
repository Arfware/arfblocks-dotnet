namespace TodoApp.Application.RequestHandlers.Users.Commands.Login
{
	public class Validator : IRequestValidator
	{
		private readonly DbValidationService _dbValidator;
		public Validator(ArfBlocksDependencyProvider dependencyProvider)
		{
			_dbValidator = dependencyProvider.GetInstance<DbValidationService>();
		}

		public void ValidateRequestModel(IRequestModel payload, IEndpointContext context, CancellationToken cancellationToken)
		{
			// Get Request Payload
			var requestPayload = (RequestModel)payload;

			// Request Model Validation
			var validationResult = new RequestModel_Validator().Validate(requestPayload);
			if (!validationResult.IsValid)
			{
				var errors = validationResult.ToString("~");
				throw new ArfBlocksValidationException(errors);
			}
		}

		public async Task ValidateDomain(IRequestModel payload, IEndpointContext context, CancellationToken cancellationToken)
		{
			// Get Request Payload
			var requestPayload = (RequestModel)payload;

			// DB Validations
			await _dbValidator.ValidateUserExist(requestPayload.Email);
		}
	}
}