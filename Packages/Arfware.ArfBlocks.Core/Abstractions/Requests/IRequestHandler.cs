using System;
using System.Threading;
using System.Threading.Tasks;
using Arfware.ArfBlocks.Core.Contexts;

namespace Arfware.ArfBlocks.Core.Abstractions
{
	public interface IRequestHandler
	{
		Task<ArfBlocksRequestResult> Handle(IRequestModel payload, EndpointContext context, CancellationToken cancellationToken);
	}
}
