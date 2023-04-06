using System;
using System.Collections.Generic;
using System.Text;

namespace Arfware.ArfBlocks.Core.Exceptions
{
	public class ArfBlocksValidationException : Exception
	{
		public ArfBlocksValidationException()
		{

		}

		public ArfBlocksValidationException(string message) : base(String.Format(message))
		{
		}
	}
}
