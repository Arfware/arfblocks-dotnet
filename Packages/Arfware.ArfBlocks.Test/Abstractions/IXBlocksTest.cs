using System.Threading.Tasks;
using Arfware.ArfBlocks.Core;

namespace Arfware.ArfBlocks.Test.Abstractions
{
	public interface IArfBlocksTest
	{
		void SetDependencies(ArfBlocksDependencyProvider dependencyProvider);
		Task SetActor();
		Task PrepareTest();
		Task RunTest();
	}
}