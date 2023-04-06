using System;
using System.Collections.Generic;
using System.Text;

namespace Arfware.ArfBlocks.Core.Exceptions
{
	public class ArfBlocksCommunicatorException : Exception
	{
		public ArfBlocksCommunicatorException()
		{

		}

		public ArfBlocksCommunicatorException(string message) : base(String.Format(message))
		{
		}
	}
}
