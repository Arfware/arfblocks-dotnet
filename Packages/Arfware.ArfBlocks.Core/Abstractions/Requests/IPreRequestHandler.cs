using System;
using System.Threading;
using System.Threading.Tasks;

namespace Arfware.ArfBlocks.Core.Abstractions
{
	public interface IPreRequestHandler
	{
		Task Handle(IRequestModel payload, CancellationToken cancellationToken);
	}
}
