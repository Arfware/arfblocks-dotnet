using System;
using System.Collections.Generic;
using System.Text;

namespace Arfware.ArfBlocks.Core.Exceptions
{
	public class ArfBlocksVerificationException : Exception
	{
		public ArfBlocksVerificationException()
		{

		}

		public ArfBlocksVerificationException(string message) : base(String.Format(message))
		{
		}
	}

}
