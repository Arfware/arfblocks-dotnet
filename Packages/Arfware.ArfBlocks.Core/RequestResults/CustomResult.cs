using System;
using System.Collections.Generic;
using System.Text;

namespace Arfware.ArfBlocks.Core
{
	public class ArfBlocksRequestResult
	{
		public int StatusCode { get; set; }
		public bool HasError { get; set; }
		public string Code { get; set; }
		public string RequestId { get; set; }
		public string ParentRequestId { get; set; }
		public int DurationMs { get; set; }
		public int TotalDurationMs { get; set; }
		public object Page { get; set; }
		public ArfBlocksRequestResultError Error { get; set; }
		public bool IsPayloadArray { get; set; }
		public object Payload { get; set; }
	}

	public class ArfBlocksRequestResultError
	{
		public string Code { get; set; }
		public string Message { get; set; }
		public string StackTrace { get; set; }
	}
}
