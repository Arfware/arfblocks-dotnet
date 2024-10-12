using System;
using System.Threading;
using System.Threading.Tasks;

namespace Arfware.ArfBlocks.Core.Abstractions
{
	public interface IRequestVerificator
	{
		Task VerificateActor(IRequestModel payload, IEndpointContext context, CancellationToken cancellationToken);
		Task VerificateDomain(IRequestModel payload, IEndpointContext context, CancellationToken cancellationToken);
	}
}
