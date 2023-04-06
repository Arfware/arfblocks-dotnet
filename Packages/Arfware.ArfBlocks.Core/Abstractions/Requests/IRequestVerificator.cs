using System;
using System.Threading;
using System.Threading.Tasks;

namespace Arfware.ArfBlocks.Core.Abstractions
{
	public interface IRequestVerificator
	{
		Task VerificateActor(IRequestModel payload, CancellationToken cancellationToken);
		Task VerificateDomain(IRequestModel payload, CancellationToken cancellationToken);
	}
}
