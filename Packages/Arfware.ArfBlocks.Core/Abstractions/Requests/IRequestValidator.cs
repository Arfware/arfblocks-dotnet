using System;
using System.Threading;
using System.Threading.Tasks;

namespace Arfware.ArfBlocks.Core.Abstractions
{
	public interface IRequestValidator
	{
		void ValidateRequestModel(IRequestModel payload, CancellationToken cancellationToken);
		Task ValidateDomain(IRequestModel payload, CancellationToken cancellationToken);
	}
}
