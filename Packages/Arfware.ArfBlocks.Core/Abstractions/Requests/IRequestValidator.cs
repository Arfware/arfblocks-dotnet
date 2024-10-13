using System;
using System.Threading;
using System.Threading.Tasks;
using Arfware.ArfBlocks.Core.Contexts;

namespace Arfware.ArfBlocks.Core.Abstractions
{
	public interface IRequestValidator
	{
		void ValidateRequestModel(IRequestModel payload, EndpointContext context, CancellationToken cancellationToken);
		Task ValidateDomain(IRequestModel payload, EndpointContext context, CancellationToken cancellationToken);
	}
}
