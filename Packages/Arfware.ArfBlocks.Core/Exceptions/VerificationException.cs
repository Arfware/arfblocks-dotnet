using System;
using System.Collections.Generic;
using System.Text;

namespace Arfware.ArfBlocks.Core.Exceptions
{
	public class ArfBlocksVerificationException : Exception
	{
		public ArfBlocksVerificationException()
		{ }

		public string Code { get; set; }
		public string Description { get; set; }

		public ArfBlocksVerificationException(string code, string description = null) : base(String.Format($"{code} | {description}"))
		{
			Code = code;
			Description = description;
		}
	}
}
