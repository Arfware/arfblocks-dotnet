using Arfware.ArfBlocks.Core;
using Arfware.ArfBlocks.Core.Abstractions;
using Arfware.ArfBlocks.Core.Exceptions;
using Arfware.ArfBlocks.Core.Models;
using Arfware.ArfBlocks.Core.RequestResults;
using Arfware.ArfBlocks.Core.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Arfware.ArfBlocks.Core
{

	public class UseRequestHandlersOptions
	{
		public List<string> SkipUrls { get; set; }
		public AuthorizationPolicies AuthorizationPolicy { get; set; }
		public AuthorizationTypes AuthorizationType { get; set; }

		public JwtAuthorizationOptionsModel JwtAuthorizationOptions { get; set; }

		public class JwtAuthorizationOptionsModel
		{
			public string Audience { get; set; }
			public string Secret { get; set; }
		}

		public enum AuthorizationTypes
		{
			None,
			Jwt,
		}

		public enum AuthorizationPolicies
		{
			AssumeAllAllowAnonymous,
			AssumeAllAuthorized,
		}
	}


	public static class UseArfBlocksRequestHandlersMiddleware
	{
		// public static IApplicationBuilder UseRequestHandlers(this IApplicationBuilder app,
		// 																   Action<UseRequestHandlersOptions> options)
		// {
		// 	// var opt = new UseRequestHandlersOptions();
		// 	// options(opt);
		// 	// app.Use(async (context, next) =>
		// 	// {
		// 	// 	System.Console.WriteLine("CALLED");
		// 	// 	var redde = new HandlerMiddleware();
		// 	// 	await redde.InvokeMiddleware(context);
		// 	// });

		// 	return app;
		// }

		public static IApplicationBuilder UseArfBlocksRequestHandlers(this IApplicationBuilder app,
																		   Action<UseRequestHandlersOptions> options)
		{
			var opt = new UseRequestHandlersOptions()
			{
				AuthorizationPolicy = UseRequestHandlersOptions.AuthorizationPolicies.AssumeAllAllowAnonymous,
				AuthorizationType = UseRequestHandlersOptions.AuthorizationTypes.None,
				SkipUrls = new List<string>(),
			};

			options(opt);

			ValidateOptions(opt);

			var dependencyProvider = (ArfBlocksDependencyProvider)app.ApplicationServices.GetService(typeof(ArfBlocksDependencyProvider));
			var _requestOperator = new ArfBlocksRequestOperator(dependencyProvider);


			app.Use(async (context, next) =>
				{
					if (context.Request.Path.HasValue && opt.SkipUrls.Any(s => context.Request.Path.Value.StartsWith(s)))
					{
						await next();
					}
					else
					{
						var middleware = new HandlerMiddleware();
						await middleware.InvokeMiddleware(context, _requestOperator, opt);
					}
				});

			return app;
		}

		private static void ValidateOptions(UseRequestHandlersOptions options)
		{
			if (options.SkipUrls == null)
				throw new Exception("SkipUrls must contains string list or must be empty list");

			if (options.AuthorizationType == UseRequestHandlersOptions.AuthorizationTypes.Jwt && options.JwtAuthorizationOptions == null)
				throw new Exception("Authorization Type Selected as Jwt but Jwt Authorization Options Not Specified. Use 'options.JwtAuthorizationOptions = new UseRequestHandlersOptions.JwtAuthorizationOptionsModel(){...'");
		}
	}

	public class HandlerMiddleware
	{
		public HandlerMiddleware()
		{

		}

		public async Task InvokeMiddleware(HttpContext httpContext, ArfBlocksRequestOperator requestOperator, UseRequestHandlersOptions options)
		{
			string requestBodyAsString = await GetRequestBody(httpContext);

			var path = httpContext.Request.Path;

			if (GlobalSettings.CanLogDebug())
			{
				System.Console.WriteLine("---------------------------------------------------------------------------");
				System.Console.WriteLine($"Path: {httpContext.Request.Path}");
			}

			Stopwatch sw = Stopwatch.StartNew();
			var responseBodyAsString = "";
			var responseCode = 200;

			if (path.HasValue)
			{
				var pathParts = path.Value.Split('/');
				var actionName = pathParts[pathParts.Length - 1];
				var objectName = pathParts[pathParts.Length - 2];

				if (GlobalSettings.CanLogDebug())
				{
					var infoLog = $"Requested For Object: '{objectName}' and Action: '{actionName}'";
					System.Console.WriteLine(infoLog);
				}

				// TODO: Change this error responses as ArfBlocksRequestResult
				var endpoint = CommandQueryRegister.GetEndpointByObjectAndAction(objectName, actionName);
				if (endpoint == null)
				{
					responseBodyAsString = "{\"error\":\"" + $"ArfBlocks Middleware: No Handler Found for Object: '{objectName}' Action: '{actionName}'" + "\"}";
				}
				else if (endpoint.IsInternal)
				{
					responseBodyAsString = "{\"error\":\"" + $"ArfBlocks Middleware: This Handler Marked as Internal: '{objectName}' Action: '{actionName}'" + "\"}";
				}
				else
				{
					// Authorization Check
					var isAuthorizationPassed = this.CheckAuthorization(httpContext, endpoint, options);
					if (isAuthorizationPassed)
					{
						var jsonSerializerOptions = new JsonSerializerOptions()
						{
							PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
						};

						var requestPayload = string.IsNullOrEmpty(requestBodyAsString) || endpoint.RequestModel == null ? null : JsonSerializer.Deserialize(requestBodyAsString, endpoint.RequestModel, jsonSerializerOptions);

						var responsePayload = await requestOperator.OperateMiddlewareRequest(endpoint, (IRequestModel)requestPayload);

						// Set Response Code
						string resultAsString = responsePayload.StatusCode == 200 ? "Succedded" : "Failed";
						responsePayload.Code = $"{endpoint.ObjectName}_{endpoint.ActionName}_{resultAsString}";

						responseBodyAsString = JsonSerializer.Serialize(responsePayload, jsonSerializerOptions);

						if (GlobalSettings.CanLogDebug())
						{
							System.Console.WriteLine("Request Payload:");
							System.Console.WriteLine(string.IsNullOrEmpty(requestBodyAsString) ? "__EMPTY__" : requestBodyAsString);
							System.Console.WriteLine("Response Payload:");
							System.Console.WriteLine(responseBodyAsString);
						}
					}
					else
					{
						responseBodyAsString = "{\"error\":\"" + $"ArfBlocks Middleware: 401 Authorization Error for: '{objectName}' Action: '{actionName}'" + "\"}";
						responseCode = 401;
					}
				}
			}

			// await using var responseBody = new MemoryStream();
			// httpContext.Response.Body = responseBody;

			sw.Stop();

			if (GlobalSettings.CanLogDebug())
			{
				System.Console.WriteLine($"Response Time: {sw.Elapsed.TotalMilliseconds} ms");
			}

			httpContext.Response.StatusCode = responseCode;
			httpContext.Response.ContentType = "application/json";
			await httpContext.Response.WriteAsync(responseBodyAsString);
		}

		private bool CheckAuthorization(HttpContext httpContext, EndpointModel endpoint, UseRequestHandlersOptions options)
		{
			// Authorization Check
			var isAuthorizationPassed = false;
			if (options.AuthorizationType == UseRequestHandlersOptions.AuthorizationTypes.None)
			{
				isAuthorizationPassed = true;
			}
			else
			{
				if (endpoint.IsAllowAnonymous)
				{
					isAuthorizationPassed = true;
				}
				else if (endpoint.IsAuthorize || options.AuthorizationPolicy == UseRequestHandlersOptions.AuthorizationPolicies.AssumeAllAuthorized)
				{
					switch (options.AuthorizationType)
					{
						case UseRequestHandlersOptions.AuthorizationTypes.Jwt:
							isAuthorizationPassed = JwtValidator.Validate(httpContext, options.JwtAuthorizationOptions);
							break;

						default:
							throw new Exception($"Authorization Type Not Handled: {options.AuthorizationType}");
					}
				}
				else
				{
					isAuthorizationPassed = true;
				}
			}

			return isAuthorizationPassed;
		}

		private static string ReadStreamInChunks(Stream stream)
		{
			const int readChunkBufferLength = 4096;

			stream.Seek(0, SeekOrigin.Begin);

			using var textWriter = new StringWriter();
			using var reader = new StreamReader(stream, Encoding.UTF8);

			var readChunk = new char[readChunkBufferLength];
			int readChunkLength;

			do
			{
				readChunkLength = reader.ReadBlock(readChunk,
												   0,
												   readChunkBufferLength);
				textWriter.Write(readChunk, 0, readChunkLength);

			} while (readChunkLength > 0);

			return textWriter.ToString();
		}

		private async Task<string> GetRequestBody(HttpContext context)
		{
			context.Request.EnableBuffering();

			var memoryStreamManager = new MemoryStream();
			// await using var requestStream = _recyclableMemoryStreamManager.GetStream();
			await context.Request.Body.CopyToAsync(memoryStreamManager);

			string reqBody = ReadStreamInChunks(memoryStreamManager);

			context.Request.Body.Seek(0, SeekOrigin.Begin);

			return reqBody;
		}
	}
}