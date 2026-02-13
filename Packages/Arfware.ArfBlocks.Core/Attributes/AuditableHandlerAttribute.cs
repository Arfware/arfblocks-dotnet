using System;

namespace Arfware.ArfBlocks.Core.Attributes
{
	public class AuditableHandlerAttribute : System.Attribute
	{
		public AuditableHandlerAttribute()
		{ }
		public string SourceRef { get; set; }
	}
}