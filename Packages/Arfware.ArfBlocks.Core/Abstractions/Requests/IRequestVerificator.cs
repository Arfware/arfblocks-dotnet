using System;
using System.Threading;
using System.Threading.Tasks;
using Arfware.ArfBlocks.Core.Contexts;

namespace Arfware.ArfBlocks.Core.Abstractions
{
	public interface IRequestVerificator
	{
		Task VerificateActor(IRequestModel payload, EndpointContext context, CancellationToken cancellationToken);
		Task VerificateDomain(IRequestModel payload, EndpointContext context, CancellationToken cancellationToken);
	}
}
