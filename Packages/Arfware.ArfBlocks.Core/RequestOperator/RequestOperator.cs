using Arfware.ArfBlocks.Core.Abstractions;
using Arfware.ArfBlocks.Core.Exceptions;
using Arfware.ArfBlocks.Core.Models;
using Arfware.ArfBlocks.Core.RequestResults;
using Arfware.ArfBlocks.Core.Contexts;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace Arfware.ArfBlocks.Core;

public class ArfBlocksRequestOperator
{
	ArfBlocksDependencyProvider _dependencyProvider;
	public ArfBlocksRequestOperator(ArfBlocksDependencyProvider dependencyProvider)
	{
		_dependencyProvider = dependencyProvider;
	}

	// For HTTP Requests
	public async Task<ActionResult> OperateHttpRequest<T>(IRequestModel payload = null) where T : class
	{
		var endpoint = this.GetTypeByRefencedType<T>();
		ArfBlocksRequestResult requestResult = await OperateByEndpoint(endpoint, payload);

		return new OkObjectResult(requestResult);
	}

	public async Task<ArfBlocksRequestResult> OperateMiddlewareRequest(EndpointModel endpoint, IRequestModel payload)
	{
		return await OperateByEndpoint(endpoint, payload);
	}

	// For Internal Requests
	public async Task<ArfBlocksRequestResult> OperateInternalRequest<T>(IRequestModel payload = null, EndpointContext parentContext = null) where T : class
	{
		var endpoint = this.GetTypeByRefencedType<T>();
		return await OperateByEndpoint(endpoint, payload, parentContext);
	}

	// For HTTP Requests
	public async Task<ArfBlocksRequestResult> OperateEvent<T>(IRequestModel payload = null) where T : class
	{
		var endpoint = this.GetTypeByRefencedType<T>();
		return await OperateByEndpoint(endpoint, payload);
	}

	#region Request Operating

	private async Task<ArfBlocksRequestResult> RunPreOperate(EndpointModel endpoint, IRequestModel payload, EndpointContext parentContext)
	{
		if (CommandQueryRegister.PreOperateEndpoint == null)
			return null;

		if (endpoint.Handler.FullName == CommandQueryRegister.PreOperateEndpoint.Handler.FullName
		  || endpoint.Handler.FullName == CommandQueryRegister.PostOperateEndpoint.Handler.FullName)
			return null;

		var requestPayload = (dynamic)Activator.CreateInstance(CommandQueryRegister.PreOperateEndpoint.RequestModel);
		requestPayload.Endpoint = endpoint;
		requestPayload.Payload = payload;

		return await OperateByEndpoint(CommandQueryRegister.PreOperateEndpoint, requestPayload, parentContext);
	}


	private async Task<ArfBlocksRequestResult> RunPostOperate(EndpointModel endpoint, IRequestModel payload, ArfBlocksRequestResult response, EndpointContext parentContext)
	{
		if (CommandQueryRegister.PostOperateEndpoint == null)
			return null;

		if (endpoint.Handler.FullName == CommandQueryRegister.PreOperateEndpoint.Handler.FullName
		 || endpoint.Handler.FullName == CommandQueryRegister.PostOperateEndpoint.Handler.FullName)
			return null;

		var requestPayload = (dynamic)Activator.CreateInstance(CommandQueryRegister.PostOperateEndpoint.RequestModel);
		requestPayload.Endpoint = endpoint;
		requestPayload.Response = response;
		requestPayload.Request = payload;
		
		return await OperateByEndpoint(CommandQueryRegister.PostOperateEndpoint, requestPayload, parentContext);
	}

	private async Task<ArfBlocksRequestResult> OperateByEndpoint(EndpointModel endpoint, IRequestModel payload, EndpointContext parentContext = null)
	{
		var requestId = Guid.NewGuid();
		var totalDuration = 0;

		EndpointContext context = null;
		if (endpoint.Context != null)
		{
			context = (EndpointContext)Activator.CreateInstance(endpoint.Context, _dependencyProvider);
		}
		else
		{
			context = new EndpointContext();
		}
		context.SetParentContext(parentContext);
		context.SetRequestId(requestId.ToString());

		// Pre Operate
		try
		{
			var preOperateResult = await RunPreOperate(endpoint, payload, context);
			totalDuration += preOperateResult?.DurationMs ?? 0;
		}
		catch (Exception ex)
		{
			System.Console.WriteLine($"\nError in RunPreOperate: {ex.Message} \n");
		}

		var stopWatch = System.Diagnostics.Stopwatch.StartNew();

		// Operate
		ArfBlocksRequestResult result = null;
		try
		{
			// Create a CancellationToken
			var cts = new CancellationTokenSource();
			var cancellationToken = cts.Token;

			// Validate Request
			await OperateValidationPhase(endpoint.Validator, payload, context, cancellationToken);

			// Verify Request
			await OperateVerificationPhase(endpoint.Verificator, payload, context, cancellationToken);

			// Handle Request
			result = await OperateHandlingPhase(endpoint.Handler, endpoint.PreHandler, endpoint.PostHandler, endpoint.DataAccess, payload, context, cancellationToken);
		}
		catch (ArfBlocksCommunicationException exception)
		{
			// do something
			result = ArfBlocksResults.BadRequest(exception.Code, exception.Description);
		}
		catch (ArfBlocksCommunicatorException exception)
		{
			// do something
			result = ArfBlocksResults.BadRequest(exception.Code, exception.Description);
		}
		catch (ArfBlocksRequestHandlerNotFoundException exception)
		{
			// do something
			result = ArfBlocksResults.NotFound(exception.Code, exception.Description);
		}
		catch (ArfBlocksVerificationException exception) // Verification Error
		{
			// do something
			result = ArfBlocksResults.BadRequest(exception.Code, exception.Description);
		}
		catch (ArfBlocksValidationException exception) // Validation Error
		{
			// do something
			result = ArfBlocksResults.BadRequest(exception.Code, exception.Description);
		}
		catch (Exception exception) // CODE Error
		{
			Console.WriteLine(exception.Message);
			Console.WriteLine(exception.StackTrace);

			// do something
			result = ArfBlocksResults.InternalServerError(exception.Message);
		}

		stopWatch.Stop();
		result.DurationMs = Convert.ToInt32(stopWatch.ElapsedMilliseconds);

		// Post Operate
		try
		{
			var postOperateResult = await RunPostOperate(endpoint, payload, result, context);
			totalDuration += postOperateResult?.DurationMs ?? 0;
		}
		catch (Exception ex)
		{
			System.Console.WriteLine($"\nError in RunPostOperate: {ex.Message} \n");
		}

		result.TotalDurationMs = totalDuration + result.DurationMs;
		result.RequestId = requestId.ToString();

		return await Task.FromResult(result);
	}

	#endregion

	#region Request Operating Phases

	private async Task OperateVerificationPhase(Type verificator, IRequestModel model, EndpointContext context, CancellationToken cancellationToken)
	{
		if (verificator != null)
		{
			IRequestVerificator requestVerificator = (IRequestVerificator)Activator.CreateInstance(verificator, _dependencyProvider);
			await requestVerificator.VerificateActor(model, context, cancellationToken);
			await requestVerificator.VerificateDomain(model, context, cancellationToken);
		}
	}

	private async Task OperateValidationPhase(Type validator, IRequestModel model, EndpointContext context, CancellationToken cancellationToken)
	{
		if (validator != null)
		{
			IRequestValidator requestValidator = (IRequestValidator)Activator.CreateInstance(validator, _dependencyProvider);
			requestValidator.ValidateRequestModel(model, context, cancellationToken);
			await requestValidator.ValidateDomain(model, context, cancellationToken);
		}
	}

	private async Task<ArfBlocksRequestResult> OperateHandlingPhase(Type handlerType, Type preHandlerType, Type postHandlerType, Type dataAccess, IRequestModel model, EndpointContext context, CancellationToken cancellationToken)
	{
		ArfBlocksRequestResult result = null;

		object dataAccessInstance = null;
		if (dataAccess != null)
		{
			dataAccessInstance = Activator.CreateInstance(dataAccess, _dependencyProvider);
		}

		// PRE-HANDLER
		if (preHandlerType != null)
		{
			IPreRequestHandler requestHandler;

			if (dataAccessInstance != null)
			{
				requestHandler = (IPreRequestHandler)Activator.CreateInstance(preHandlerType, _dependencyProvider, dataAccessInstance);
			}
			else
			{
				requestHandler = (IPreRequestHandler)Activator.CreateInstance(preHandlerType, _dependencyProvider);
			}

			await requestHandler.Handle(model, context, cancellationToken);
		}

		// HANDLER
		if (handlerType != null)
		{
			IRequestHandler requestHandler;

			if (dataAccessInstance != null)
			{
				requestHandler = (IRequestHandler)Activator.CreateInstance(handlerType, _dependencyProvider, dataAccessInstance);
			}
			else
			{
				requestHandler = (IRequestHandler)Activator.CreateInstance(handlerType, _dependencyProvider);
			}

			result = await requestHandler.Handle(model, context, cancellationToken);
		}
		else
		{
			throw new ArfBlocksRequestHandlerNotFoundException("Request Handler Not Found");
		}

		// POST-HANDLER
		if (postHandlerType != null)
		{
			IPostRequestHandler postRequestHandler;

			if (dataAccessInstance != null)
			{
				postRequestHandler = (IPostRequestHandler)Activator.CreateInstance(postHandlerType, _dependencyProvider, dataAccessInstance);
			}
			else
			{
				postRequestHandler = (IPostRequestHandler)Activator.CreateInstance(postHandlerType, _dependencyProvider);
			}

			await postRequestHandler.Handle(model, result, context, cancellationToken);
		}

		return result;
	}

	#endregion


	#region Helpers

	public ActionResult ConvertRequestResultToActionResult(ArfBlocksRequestResult requestResult)
	{
		// EVALUATE HANDLE'S RESULT
		// if (requestResult.HasError)
		// {
		//     switch (requestResult.StatusCode)
		//     {
		//         case 204:
		//             return new NoContentResult();
		//         case 400:
		//             return new BadRequestObjectResult(requestResult.Error);
		//         case 403:
		//             return new UnauthorizedObjectResult(requestResult.Error);
		//         case 404:
		//             return new NotFoundObjectResult(requestResult.Error);
		//         case 500:
		//             return new ; // this line will change
		//         default:
		//             return new ConflictObjectResult(requestResult.Error);
		//     }
		// }
		// else // Everything is OK
		// {
		//     return new OkObjectResult(requestResult.Payload);
		// }
		return new OkObjectResult(requestResult);
	}

	private Type GetTypeByRefencedType<Treference, Tsearch>()
	{
		string nameSpace = typeof(Treference).Namespace;
		Assembly assembly = typeof(Treference).Assembly;

		List<Type> typelist = assembly.GetTypes()
			.Where(t => String.Equals(t.Namespace, nameSpace, StringComparison.Ordinal))
			.ToList();

		foreach (Type type in typelist)
		{
			if (type.GetInterfaces().Contains(typeof(Tsearch)))
				return type;
		}

		return null;
	}

	private EndpointModel GetTypeByRefencedType<T>()
	{
		string nameSpace = typeof(T).Namespace;
		Assembly assembly = typeof(T).Assembly;

		List<Type> typelist = assembly.GetTypes()
			.Where(t => String.Equals(t.Namespace, nameSpace, StringComparison.Ordinal))
			.ToList();

		var names = CommandQueryRegister.ParseNamespace(nameSpace);
		var attributes = CommandQueryRegister.GetAttributes(typeof(T));

		var endpoint = new EndpointModel()
		{
			ObjectName = names.objectName,
			ActionName = names.actionName,
			EndpointType = CommandQueryRegister.GetEndpointType(nameSpace),
			Handler = typelist.FirstOrDefault(t => t.GetInterfaces().Any(i => i == typeof(IRequestHandler))),
			RequestModel = typelist.FirstOrDefault(t => t.GetInterfaces().Any(i => i == typeof(IRequestModel))),
			ResponseModel = typelist.FirstOrDefault(t => t.GetInterfaces().Any(i => i == typeof(IResponseModel) || i == typeof(IResponseModel<Array>))),
			PreHandler = typelist.FirstOrDefault(t => t.GetInterfaces().Any(i => i == typeof(IPreRequestHandler))),
			PostHandler = typelist.FirstOrDefault(t => t.GetInterfaces().Any(i => i == typeof(IPostRequestHandler))),
			DataAccess = typelist.FirstOrDefault(t => t.GetInterfaces().Any(i => i == typeof(IDataAccess))),
			Validator = typelist.FirstOrDefault(t => t.GetInterfaces().Any(i => i == typeof(IRequestValidator))),
			Verificator = typelist.FirstOrDefault(t => t.GetInterfaces().Any(i => i == typeof(IRequestVerificator))),
			Context = typelist.FirstOrDefault(t => t.GetInterfaces().Any(i => i == typeof(IEndpointContext))),
			IsInternal = attributes.IsInternal,
			IsAuthorize = attributes.IsAuthorize,
			IsAllowAnonymous = attributes.IsAllowAnonymous,
			IsEventHandler = attributes.IsEventHandler,
			IsAuditable = attributes.isAuditableHandler,
			IsTraceable = attributes.isTraceableHandler,
		};

		return endpoint;
	}

	#endregion
}
