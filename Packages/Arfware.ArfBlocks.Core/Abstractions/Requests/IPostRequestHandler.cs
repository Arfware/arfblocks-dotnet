using System;
using System.Threading;
using System.Threading.Tasks;

namespace Arfware.ArfBlocks.Core.Abstractions
{
	public interface IPostRequestHandler
	{
		Task Handle(IRequestModel payload, ArfBlocksRequestResult response, CancellationToken cancellationToken);
	}
}
