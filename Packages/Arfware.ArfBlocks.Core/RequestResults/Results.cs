using System;
using System.Linq;
using System.Collections.Generic;
using Arfware.ArfBlocks.Core.Abstractions;

namespace Arfware.ArfBlocks.Core.RequestResults
{
	public class ArfBlocksResults
	{
		public static ArfBlocksRequestResult Success(IResponseModel payload)
		{
			return new ArfBlocksRequestResult()
			{
				StatusCode = 200,
				Payload = payload,
				Code = null,
				Page = null,
				HasError = false,
				IsPayloadArray = false,
				Error = null
			};
		}

		public static ArfBlocksRequestResult Success(IEnumerable<IResponseModel<Array>> payload, object page = null)
		{
			return new ArfBlocksRequestResult()
			{
				StatusCode = 200,
				Payload = payload,
				Code = null,
				Page = page,
				HasError = false,
				IsPayloadArray = true,
				Error = null
			};
		}

		public static ArfBlocksRequestResult NotFound(string message)
		{
			return new ArfBlocksRequestResult()
			{
				StatusCode = 404,
				HasError = true,
				Error = new ArfBlocksRequestResultError()
				{
					Message = message
				}
			};
		}

		public static ArfBlocksRequestResult BadRequest(string message)
		{
			return new ArfBlocksRequestResult()
			{
				StatusCode = 400,
				HasError = true,
				Error = new ArfBlocksRequestResultError()
				{
					Message = message
				}
			};
		}

		public static ArfBlocksRequestResult InternalServerError(string message)
		{
			Console.WriteLine("-----------------------------------------------------");
			Console.WriteLine($"/Internal Server Error: {message}");
			Console.WriteLine("-----------------------------------------------------");

			return new ArfBlocksRequestResult()
			{
				StatusCode = 500,
				HasError = true,
				Error = new ArfBlocksRequestResultError()
				{
					Message = message
				}
			};
		}

		public static ArfBlocksRequestResult UnAuthorized(string message)
		{
			return new ArfBlocksRequestResult()
			{
				StatusCode = 401,
				HasError = true,
				Error = new ArfBlocksRequestResultError()
				{
					Message = message
				}
			};
		}

		public static ArfBlocksRequestResult Forbidden(string message)
		{
			return new ArfBlocksRequestResult()
			{
				StatusCode = 403,
				HasError = true,
				Error = new ArfBlocksRequestResultError()
				{
					Message = message
				}
			};
		}
	}
}
