using System;
using System.Collections.Generic;
using System.Text;

namespace Arfware.ArfBlocks.Core.Exceptions
{
	public class ArfBlocksRequestHandlerNotFoundException : Exception
	{
		public ArfBlocksRequestHandlerNotFoundException()
		{ }

		public ArfBlocksRequestHandlerNotFoundException(string message) : base(string.Format(message))
		{ }
	}
}
