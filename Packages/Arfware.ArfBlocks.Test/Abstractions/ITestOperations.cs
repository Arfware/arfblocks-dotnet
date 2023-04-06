using System.Threading.Tasks;

namespace Arfware.ArfBlocks.Test.Abstractions
{
	public interface ITestOperations
	{
		Task PreExecuting();
		Task PostExecuting();
	}
}