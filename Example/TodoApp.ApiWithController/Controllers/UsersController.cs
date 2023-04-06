using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Arfware.ArfBlocks.Core;
using Microsoft.AspNetCore.Authorization;

namespace TodoApp.ApiWithController.Controllers
{
	[ApiController]
	[Route("[controller]")]
	[Produces("application/json")]
	public class UsersController : ControllerBase
	{
		private readonly ArfBlocksRequestOperator _requestOperator;

		public UsersController(ArfBlocksDependencyProvider dependencyProvider)
		{
			_requestOperator = new ArfBlocksRequestOperator(dependencyProvider);
		}

		// QUERIES

		/// <summary>
		/// Get All Users
		/// </summary>
		/// <response code="200">Returns All Users in Response.Payload</response>
		[AllowAnonymous]
		[HttpGet("[action]")]
		public async Task<ActionResult<List<Application.RequestHandlers.Users.Queries.All.ResponseModel>>> All()
		{
			return await _requestOperator.OperateHttpRequest<Application.RequestHandlers.Users.Queries.All.Handler>(null);
		}


		/// <summary>
		/// Get Current User
		/// </summary>
		/// <response code="200">Returns Current User Information in Response.Payload</response>
		[HttpGet("[action]")]
		public async Task<ActionResult<List<Application.RequestHandlers.Users.Queries.Me.ResponseModel>>> Me()
		{
			return await _requestOperator.OperateHttpRequest<Application.RequestHandlers.Users.Queries.Me.Handler>(null);
		}


		// COMMANDS 

		/// <summary>
		/// Login
		/// </summary>
		/// <response code="200">Returns Logined User Information and JWT Token in Response.Payload</response>
		[AllowAnonymous]
		[HttpPost("[action]")]
		public async Task<ActionResult<List<Application.RequestHandlers.Users.Commands.Login.ResponseModel>>> Login(Application.RequestHandlers.Users.Commands.Login.RequestModel payload)
		{
			return await _requestOperator.OperateHttpRequest<Application.RequestHandlers.Users.Commands.Login.Handler>(payload);
		}
	}
}
