using System;
using System.Collections.Generic;
using System.Text;

namespace Arfware.ArfBlocks.Core.Exceptions
{
	public class ArfBlocksHandlerException : Exception
	{
		public ArfBlocksHandlerException()
		{

		}

		public ArfBlocksHandlerException(string message) : base(String.Format(message))
		{
		}
	}
}
