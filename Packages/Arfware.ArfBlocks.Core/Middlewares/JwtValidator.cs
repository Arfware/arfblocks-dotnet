
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

namespace Arfware.ArfBlocks.Core
{
	public class JwtValidator
	{
		public static bool Validate(HttpContext httpContext, UseRequestHandlersOptions.JwtAuthorizationOptionsModel options)
		{
			var parameters = new TokenValidationParameters
			{
				ValidAudience = options.Audience,
				ValidateIssuerSigningKey = true,
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(options.Secret)),
				ValidateAudience = true,
				ValidateLifetime = true,
				ValidateIssuer = false,
			};

			var handler = new JwtSecurityTokenHandler();
			handler.InboundClaimTypeMap.Clear();

			var jwt = httpContext.Request.Headers["Authorization"].ToString();

			var isValid = false;

			if (!string.IsNullOrEmpty(jwt))
			{
				jwt = jwt.Split(' ')[1];
				System.Console.WriteLine(jwt);

				try
				{
					var user = handler.ValidateToken(jwt, parameters, out var _);
					System.Console.WriteLine($"{user.ToString()}");
					isValid = true;
				}
				catch (SecurityTokenInvalidLifetimeException)
				{
					System.Console.WriteLine("xxxxxxxxxxx: Token Lifetime Not Valid");
				}
				catch (SecurityTokenExpiredException)
				{
					System.Console.WriteLine("xxxxxxxxxxx: Token Expired");
				}
				catch (SecurityTokenDecryptionFailedException)
				{
					System.Console.WriteLine("xxxxxxxxxxx: Decryption Not Valid");
				}
				catch (SecurityTokenInvalidIssuerException)
				{
					System.Console.WriteLine("xxxxxxxxxxx: Issuer Not Valid");
				}
				catch (SecurityTokenInvalidSignatureException)
				{
					System.Console.WriteLine("xxxxxxxxxxx: Signature Not Valid");
				}
				catch (SecurityTokenInvalidTypeException)
				{
					System.Console.WriteLine("xxxxxxxxxxx: Type in Header Not Valid");
				}
				catch (SecurityTokenDecompressionFailedException)
				{
					System.Console.WriteLine("xxxxxxxxxxx: DeCompression Failed");
				}
				catch (Exception e)
				{
					System.Console.WriteLine(e.Message);
					System.Console.WriteLine(e.StackTrace);
				}
			}

			return isValid;

		}
	}
}