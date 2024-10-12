using System;
using System.Threading;
using System.Threading.Tasks;

namespace Arfware.ArfBlocks.Core.Abstractions
{
	public interface IRequestValidator
	{
		void ValidateRequestModel(IRequestModel payload, IEndpointContext context, CancellationToken cancellationToken);
		Task ValidateDomain(IRequestModel payload, IEndpointContext context, CancellationToken cancellationToken);
	}
}
