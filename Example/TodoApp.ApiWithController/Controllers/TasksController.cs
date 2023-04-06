using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Arfware.ArfBlocks.Core;

namespace TodoApp.ApiWithController.Controllers
{
	[ApiController]
	[Route("[controller]")]
	[Produces("application/json")]
	public class TasksController : ControllerBase
	{
		private readonly ArfBlocksRequestOperator _requestOperator;

		public TasksController(ArfBlocksDependencyProvider dependencyProvider)
		{
			_requestOperator = new ArfBlocksRequestOperator(dependencyProvider);
		}

		// QUERIES

		/// <summary>
		/// Get All Tasks
		/// </summary>
		/// <response code="200">Returns All Tasks in Response.Payload</response>
		[HttpGet("[action]")]
		public async Task<ActionResult<List<Application.RequestHandlers.Tasks.Queries.All.ResponseModel>>> All()
		{
			return await _requestOperator.OperateHttpRequest<Application.RequestHandlers.Tasks.Queries.All.Handler>(null);
		}


		/// <summary>
		/// Get Pending Tasks
		/// </summary>
		/// <response code="200">Returns Pending Tasks in Response.Payload</response>
		[HttpGet("[action]")]
		public async Task<ActionResult<List<Application.RequestHandlers.Tasks.Queries.Pendings.ResponseModel>>> Pendings()
		{
			return await _requestOperator.OperateHttpRequest<Application.RequestHandlers.Tasks.Queries.Pendings.Handler>(null);
		}


		/// <summary>
		/// Get My Tasks
		/// </summary>
		/// <response code="200">Returns User's Tasks in Response.Payload</response>
		[HttpGet("[action]")]
		public async Task<ActionResult<List<Application.RequestHandlers.Tasks.Queries.MyTasks.ResponseModel>>> MyTasks()
		{
			return await _requestOperator.OperateHttpRequest<Application.RequestHandlers.Tasks.Queries.MyTasks.Handler>(null);
		}


		/// <summary>
		/// Get Task By Id
		/// </summary>
		/// <response code="200">Returns Task in Response.Payload</response>
		[HttpGet("{id}")]
		public async Task<ActionResult<Application.RequestHandlers.Tasks.Queries.Detail.ResponseModel>> Get(Guid id)
		{
			var payload = new Application.RequestHandlers.Tasks.Queries.Detail.RequestModel() { TaskId = id };
			return await _requestOperator.OperateHttpRequest<Application.RequestHandlers.Tasks.Queries.Detail.Handler>(payload);
		}

		// COMMANDS 

		/// <summary>
		/// Create New Task
		/// </summary>
		/// <response code="200">Returns Created Task in Response.Payload</response>
		[HttpPost("[action]")]
		public async Task<ActionResult<List<Application.RequestHandlers.Tasks.Commands.Create.ResponseModel>>> Create(Application.RequestHandlers.Tasks.Commands.Create.RequestModel payload)
		{
			return await _requestOperator.OperateHttpRequest<Application.RequestHandlers.Tasks.Commands.Create.Handler>(payload);
		}


		/// <summary>
		/// Update Task 
		/// </summary>
		/// <response code="200">Returns Updated Task  in Response.Payload</response>
		[HttpPut("[action]")]
		public async Task<ActionResult<List<Application.RequestHandlers.Tasks.Commands.Update.ResponseModel>>> Update(Application.RequestHandlers.Tasks.Commands.Update.RequestModel payload)
		{
			return await _requestOperator.OperateHttpRequest<Application.RequestHandlers.Tasks.Commands.Update.Handler>(payload);
		}


		/// <summary>
		/// Delete Task
		/// </summary>
		/// <response code="200">Returns Delete Task's Id in Response.Payload</response>
		[HttpDelete("[action]/{id}")]
		public async Task<ActionResult<List<Application.RequestHandlers.Tasks.Commands.Delete.ResponseModel>>> Delete(Guid Id)
		{
			var payload = new Application.RequestHandlers.Tasks.Commands.Delete.RequestModel() { TaskId = Id };
			return await _requestOperator.OperateHttpRequest<Application.RequestHandlers.Tasks.Commands.Delete.Handler>(payload);
		}


		/// <summary>
		/// Approve Task
		/// </summary>
		/// <response code="200">Returns Approved Task Meta Data in Response.Payload</response>
		[HttpPost("[action]")]
		public async Task<ActionResult<List<Application.RequestHandlers.Tasks.Commands.Complete.ResponseModel>>> Complete(Application.RequestHandlers.Tasks.Commands.Complete.RequestModel payload)
		{
			return await _requestOperator.OperateHttpRequest<Application.RequestHandlers.Tasks.Commands.Complete.Handler>(payload);
		}


		/// <summary>
		/// Reject Task
		/// </summary>
		/// <response code="200">Returns Rejected Task Meta Data in Response.Payload</response>
		[HttpPost("[action]")]
		public async Task<ActionResult<List<Application.RequestHandlers.Tasks.Commands.Reject.ResponseModel>>> Reject(Application.RequestHandlers.Tasks.Commands.Reject.RequestModel payload)
		{
			return await _requestOperator.OperateHttpRequest<Application.RequestHandlers.Tasks.Commands.Reject.Handler>(payload);
		}
	}
}
