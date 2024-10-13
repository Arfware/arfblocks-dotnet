using System;

namespace Arfware.ArfBlocks.Core.Contexts
{
	internal interface IEndpointContext
	{
	}

	public class EndpointContext : IEndpointContext
	{
		public EndpointContext()
		{ }

		private EndpointContext _parentContext = null;
		public EndpointContext GetParentContext()
		{
			return this._parentContext;
		}
		internal void SetParentContext(EndpointContext context)
		{
			this._parentContext = context;
		}

		// Is internal call 
		private bool _isInternalCall = false;
		public bool CheckIsInternalCall()
		{
			return this._isInternalCall;
		}
		internal void SetIsInternalCall(bool isInternalCall)
		{
			this._isInternalCall = isInternalCall;
		}

		// Request id and parent request id
		private string _requestId = null;
		public string GetRequestId()
		{
			return this._requestId;
		}
		public void SetRequestId(string requestId)
		{
			this._requestId = requestId;
		}
		public string GetParentRequestId()
		{
			return this._parentContext?.GetRequestId() ?? null;
		}
	}
}