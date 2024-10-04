using System;
using Arfware.ArfBlocks.Core.Abstractions;
using Arfware.ArfBlocks.Core.Models;
using FluentValidation;
using TodoApp.Domain.Entities;

namespace TodoApp.Application.DefaultHandlers.Operators.Commands.PostOperate;

public class ResponseModel : IResponseModel
{
}

public class RequestModel : IRequestModel
{
	public EndpointModel Endpoint { get; set; }
	public ArfBlocksRequestResult Response { get; set; }
}
