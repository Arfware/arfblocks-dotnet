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
	public class DepartmentsController : ControllerBase
	{
		private readonly ArfBlocksRequestOperator _requestOperator;

		public DepartmentsController(ArfBlocksDependencyProvider dependencyProvider)
		{
			_requestOperator = new ArfBlocksRequestOperator(dependencyProvider);
		}

		// QUERIES

		/// <summary>
		/// Get All Departments
		/// </summary>
		/// <response code="200">Returns All Departments in Response.Payload</response>
		[HttpGet("[action]")]
		public async Task<ActionResult<List<Application.RequestHandlers.Departments.Queries.All.ResponseModel>>> All()
		{
			return await _requestOperator.OperateHttpRequest<Application.RequestHandlers.Departments.Queries.All.Handler>(null);
		}
	}
}
