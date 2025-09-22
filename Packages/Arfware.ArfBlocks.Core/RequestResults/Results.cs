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

		public static ArfBlocksRequestResult NotFound(string code, string description = null)
		{
			return new ArfBlocksRequestResult()
			{
				StatusCode = 404,
				HasError = true,
				Error = new ArfBlocksRequestResultError()
				{
					Code = code,
					Message = code,
					Description = description,
				}
			};
		}

		public static ArfBlocksRequestResult BadRequest(string code, string description = null)
		{
			return new ArfBlocksRequestResult()
			{
				StatusCode = 400,
				HasError = true,
				Error = new ArfBlocksRequestResultError()
				{
					Code = code,
					Message = code,
					Description = description,
				}
			};
		}

		public static ArfBlocksRequestResult InternalServerError(string code, string description = null)
		{
			Console.WriteLine("-----------------------------------------------------");
			Console.WriteLine($"/Internal Server Error: {code}");
			Console.WriteLine("-----------------------------------------------------");

			return new ArfBlocksRequestResult()
			{
				StatusCode = 500,
				HasError = true,
				Error = new ArfBlocksRequestResultError()
				{
					Code = code,
					Message = code,
					Description = description,
				}
			};
		}

		public static ArfBlocksRequestResult UnAuthorized(string code, string description = null)
		{
			return new ArfBlocksRequestResult()
			{
				StatusCode = 401,
				HasError = true,
				Error = new ArfBlocksRequestResultError()
				{
					Code = code,
					Message = code,
					Description = description,
				}
			};
		}

		public static ArfBlocksRequestResult Forbidden(string code, string description = null)
		{
			return new ArfBlocksRequestResult()
			{
				StatusCode = 403,
				HasError = true,
				Error = new ArfBlocksRequestResultError()
				{
					Code = code,
					Message = code,
					Description = description,
				}
			};
		}
	}
}
