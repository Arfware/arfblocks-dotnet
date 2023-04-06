using System;
using Arfware.ArfBlocks.Core.Abstractions;
using FluentValidation;
using TodoApp.Domain.Entities;

namespace TodoApp.Application.RequestHandlers.Tasks.Commands.Complete
{
	public class ResponseModel : IResponseModel
	{
		public Guid TaskId { get; set; }
		public TodoTaskStatus Status { get; set; }
	}

	public class RequestModel : IRequestModel
	{
		public Guid TaskId { get; set; }
		public string Message { get; set; }
	}



}