using System;
using System.Threading;
using System.Threading.Tasks;
using Arfware.ArfBlocks.Core.Contexts;

namespace Arfware.ArfBlocks.Core.Abstractions
{
	public interface IPostRequestHandler
	{
		Task Handle(IRequestModel payload, ArfBlocksRequestResult response, EndpointContext context, CancellationToken cancellationToken);
	}
}
