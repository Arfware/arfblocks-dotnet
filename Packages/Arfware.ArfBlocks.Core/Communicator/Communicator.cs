using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Arfware.ArfBlocks.Core.Abstractions;

namespace Arfware.ArfBlocks.Core
{
	public class ArfBlocksCommunicator
	{
		private readonly ArfBlocksRequestOperator _requestOperator;

		public ArfBlocksCommunicator(ArfBlocksDependencyProvider dependencyProvider)
		{
			_requestOperator = new ArfBlocksRequestOperator(dependencyProvider);
		}

		public async Task<ArfBlocksRequestResult> CommunicateDirect<T>(IRequestModel payload) where T : class
		{
			return await _requestOperator.OperateInternalRequest<T>(payload);
		}

		public async Task<ArfBlocksRequestResult> CommunicateViaHttp(string definedRoute, object payload)
		{
			return await Task.FromResult<ArfBlocksRequestResult>(null);
		}
	}
}
