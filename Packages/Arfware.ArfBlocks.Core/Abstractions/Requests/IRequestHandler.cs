using System;
using System.Threading;
using System.Threading.Tasks;

namespace Arfware.ArfBlocks.Core.Abstractions
{
	public interface IRequestHandler
	{
		Task<ArfBlocksRequestResult> Handle(IRequestModel payload, IEndpointContext context, CancellationToken cancellationToken);
	}
}
