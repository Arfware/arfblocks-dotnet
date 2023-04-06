using System;

namespace Arfware.ArfBlocks.Core.Models
{

	public class ApplicationDataModel
	{
		public Type DependencyProviderType { get; set; }
		public Type ConfigurationClassType { get; set; }
	}

	public class EndpointModel
	{
		public string ObjectName { get; set; }
		public string ActionName { get; set; }
		public Type RequestModel { get; set; }
		public Type ResponseModel { get; set; }
		public Type Handler { get; set; }
		public Type PreHandler { get; set; }
		public Type PostHandler { get; set; }
		public Type DataAccess { get; set; }
		public Type Validator { get; set; }
		public Type Verificator { get; set; }
		public EndpointTypes EndpointType { get; set; }
		public bool IsResponseModelArray { get; set; }
		public bool IsInternal { get; set; } // Handler Marked as InternalAttribute
		public bool IsAuthorize { get; set; } // Handler Marked as AuthorizeAttribute
		public bool IsAllowAnonymous { get; set; } // Handler Marked as AllowAnonymousAttribute

		public enum EndpointTypes
		{
			Command,
			Query,
		}
	}
}