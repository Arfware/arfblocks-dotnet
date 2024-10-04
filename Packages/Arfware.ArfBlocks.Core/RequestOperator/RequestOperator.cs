using Arfware.ArfBlocks.Core;
using Arfware.ArfBlocks.Core.Abstractions;
using Arfware.ArfBlocks.Core.Exceptions;
using Arfware.ArfBlocks.Core.Models;
using Arfware.ArfBlocks.Core.RequestResults;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Arfware.ArfBlocks.Core
{
	public class ArfBlocksRequestOperator
	{
		ArfBlocksDependencyProvider _dependencyProvider;
		public ArfBlocksRequestOperator(ArfBlocksDependencyProvider dependencyProvider)
		{
			_dependencyProvider = dependencyProvider;
		}

		private async Task RunPreOperate(EndpointModel endpoint, IRequestModel payload)
		{
			if (CommandQueryRegister.PreOperateEndpoint == null)
				return;

			var requestPayload = (dynamic)Activator.CreateInstance(CommandQueryRegister.PreOperateEndpoint.RequestModel);
			requestPayload.Endpoint = endpoint;
			requestPayload.Payload = payload;

			await OperateByEndpoint(CommandQueryRegister.PreOperateEndpoint, requestPayload);
		}


		private async Task RunPostOperate(EndpointModel endpoint, ArfBlocksRequestResult response)
		{
			if (CommandQueryRegister.PostOperateEndpoint == null)
				return;

			var requestPayload = (dynamic)Activator.CreateInstance(CommandQueryRegister.PostOperateEndpoint.RequestModel);
			requestPayload.Endpoint = endpoint;
			requestPayload.Response = response;

			await OperateByEndpoint(CommandQueryRegister.PostOperateEndpoint, requestPayload);
		}

		// For HTTP Requests
		public async Task<ActionResult> OperateHttpRequest<T>(IRequestModel payload = null) where T : class
		{
			var endpoint = this.GetTypeByRefencedType<T>();

			await RunPreOperate(endpoint, payload);

			ArfBlocksRequestResult requestResult = await OperateByEndpoint(endpoint, payload);

			await RunPostOperate(endpoint, requestResult);

			return ConvertRequestResultToActionResult(requestResult);
		}

		public async Task<ArfBlocksRequestResult> OperateMiddlewareRequest(EndpointModel endpoint, IRequestModel payload)
		{
			System.Console.WriteLine(payload.GetType());

			await RunPreOperate(endpoint, payload);

			ArfBlocksRequestResult requestResult = await OperateByEndpoint(endpoint, payload);

			await RunPostOperate(endpoint, requestResult);

			return requestResult;
		}

		// For Internal Requests
		public async Task<ArfBlocksRequestResult> OperateInternalRequest<T>(IRequestModel payload = null) where T : class
		{
			var endpoint = this.GetTypeByRefencedType<T>();

			await RunPreOperate(endpoint, payload);

			ArfBlocksRequestResult requestResult = await OperateByEndpoint(endpoint, payload);

			await RunPostOperate(endpoint, requestResult);

			return requestResult;
		}

		// For HTTP Requests
		public async Task<ArfBlocksRequestResult> OperateEvent<T>(IRequestModel payload = null) where T : class
		{
			var endpoint = this.GetTypeByRefencedType<T>();

			await RunPreOperate(endpoint, payload);

			ArfBlocksRequestResult requestResult = await OperateByEndpoint(endpoint, payload);

			await RunPostOperate(endpoint, requestResult);

			return requestResult;
		}

		#region Request Operating

		private async Task<ArfBlocksRequestResult> OperateByEndpoint(EndpointModel endpoint, IRequestModel payload)
		{
			try
			{
				// Create a CancellationToken
				var cts = new CancellationTokenSource();
				var cancellationToken = cts.Token;

				// Validate Request
				await OperateValidationPhase(endpoint.Validator, payload, cancellationToken);

				// Verify Request
				await OperateVerificationPhase(endpoint.Verificator, payload, cancellationToken);

				// Handle Request
				var result = await OperateHandlingPhase(endpoint.Handler, endpoint.PreHandler, endpoint.PostHandler, endpoint.DataAccess, payload, cancellationToken);

				return await Task.FromResult(result);
			}
			catch (ArfBlocksRequestHandlerNotFoundException exception)
			{
				// do something
				return ArfBlocksResults.NotFound(exception.Message);
			}
			catch (ArfBlocksVerificationException exception) // Verification Error
			{
				// do something
				return ArfBlocksResults.BadRequest(exception.Message);
			}
			catch (ArfBlocksValidationException exception) // Validation Error
			{
				// do something
				return ArfBlocksResults.BadRequest(exception.Message);
			}
			catch (Exception exception) // CODE Error
			{
				Console.WriteLine(exception.Message);
				Console.WriteLine(exception.StackTrace);

				// do something
				return ArfBlocksResults.InternalServerError(exception.Message);
			}
		}

		#endregion

		#region Request Operating Phases

		private async Task OperateVerificationPhase(Type verificator, IRequestModel model, CancellationToken cancellationToken)
		{
			if (verificator != null)
			{
				IRequestVerificator requestVerificator = (IRequestVerificator)Activator.CreateInstance(verificator, _dependencyProvider);
				await requestVerificator.VerificateActor(model, cancellationToken);
				await requestVerificator.VerificateDomain(model, cancellationToken);
			}
		}

		private async Task OperateValidationPhase(Type validator, IRequestModel model, CancellationToken cancellationToken)
		{
			if (validator != null)
			{
				IRequestValidator requestValidator = (IRequestValidator)Activator.CreateInstance(validator, _dependencyProvider);
				requestValidator.ValidateRequestModel(model, cancellationToken);
				await requestValidator.ValidateDomain(model, cancellationToken);
			}
		}

		private async Task<ArfBlocksRequestResult> OperateHandlingPhase(Type handlerType, Type preHandlerType, Type postHandlerType, Type dataAccess, IRequestModel model, CancellationToken cancellationToken)
		{
			object dataAccessInstance = null;
			ArfBlocksRequestResult result = null;

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

				await requestHandler.Handle(model, cancellationToken);
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

				result = await requestHandler.Handle(model, cancellationToken);
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

				await postRequestHandler.Handle(model, result, cancellationToken);
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

			var endpoint = new EndpointModel()
			{
				RequestModel = typelist.FirstOrDefault(t => t.GetInterfaces().Any(i => i == typeof(IRequestModel))),
				ResponseModel = typelist.FirstOrDefault(t => t.GetInterfaces().Any(i => i == typeof(IResponseModel) || i == typeof(IResponseModel<Array>))),
				Handler = typelist.FirstOrDefault(t => t.GetInterfaces().Any(i => i == typeof(IRequestHandler))),
				PreHandler = typelist.FirstOrDefault(t => t.GetInterfaces().Any(i => i == typeof(IPreRequestHandler))),
				PostHandler = typelist.FirstOrDefault(t => t.GetInterfaces().Any(i => i == typeof(IPostRequestHandler))),
				DataAccess = typelist.FirstOrDefault(t => t.GetInterfaces().Any(i => i == typeof(IDataAccess))),
				Validator = typelist.FirstOrDefault(t => t.GetInterfaces().Any(i => i == typeof(IRequestValidator))),
				Verificator = typelist.FirstOrDefault(t => t.GetInterfaces().Any(i => i == typeof(IRequestVerificator))),
			};

			return endpoint;
		}

		#endregion
	}
}
