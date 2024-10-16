using System;
using System.Threading.Tasks;
using System.Collections;
using Arfware.ArfBlocks.Core.Abstractions;
using Arfware.ArfBlocks.Core.RequestResults;
using Arfware.ArfBlocks.Core;
using Microsoft.AspNetCore.Http;
using BusinessModules.Management.Infrastructure.Services;
using TodoApp.Infrastructure.Services;
using System.Threading;
using Microsoft.AspNetCore.Authorization;
using Arfware.ArfBlocks.Core.Attributes;
using Arfware.ArfBlocks.Core.Models;
using System.Text.Json;
using System.Net;

namespace TodoApp.Application.DefaultHandlers.Operators.Commands.PostOperate;

[AllowAnonymousHandler]
public class Handler : IRequestHandler
{
	private readonly DataAccess dataAccessLayer;
	private readonly CurrentClientService _clientService;

	public Handler(ArfBlocksDependencyProvider dependencyProvider, object dataAccess)
	{
		dataAccessLayer = (DataAccess)dataAccess;
		_clientService = dependencyProvider.GetInstance<CurrentClientService>();
	}

	public async Task<ArfBlocksRequestResult> Handle(IRequestModel payload, EndpointContext context, CancellationToken cancellationToken)
	{
		var mapper = new Mapper();
		var requestPayload = (RequestModel)payload;

		var currentUserId = Guid.Empty;//_clientService.GetCurrentUserId();
		var currentUserIp = _clientService.GetIpAdress();
		await ProcessData(requestPayload.Endpoint, requestPayload.Response, context, currentUserId, currentUserIp);

		// Map to Response Model
		var mappedResponseModel = mapper.MapToResponseModel();
		return ArfBlocksResults.Success(mappedResponseModel);
	}

	public async Task ProcessData(EndpointModel endpoint, ArfBlocksRequestResult response, EndpointContext context, Guid currentUserId, IPAddress ipAddress)
	{
		// NOP:
		await Task.CompletedTask;

		System.Console.WriteLine("\n\n--------------------");
		System.Console.WriteLine($"Request Id: {context.GetRequestId()}");
		System.Console.WriteLine($"Parent Request Id: {context.GetParentRequestId()}");
		System.Console.WriteLine($"Object: {endpoint.ObjectName} | Action: {endpoint.ActionName} | Type: {endpoint.EndpointType}");
		System.Console.WriteLine($"Status Code: {response.StatusCode} | ResponseCode: {response.Code} | Has Error: {response.HasError} | ErrorCode?: {response.Error?.Message}");
		System.Console.WriteLine($"User ID: {currentUserId}");
		System.Console.WriteLine($"User IP: {ipAddress}");
		System.Console.WriteLine(JsonSerializer.Serialize(response.Payload, new JsonSerializerOptions() { WriteIndented = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase }));
		System.Console.WriteLine("--------------------\n\n");
	}
}
