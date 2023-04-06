using Arfware.ArfBlocks.Core.Abstractions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using TodoApp.Infrastructure.RelationalDB;
using TodoApp.Domain.Entities;
using System.Text;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace TodoApp.Infrastructure.Services
{
	public class JwtService : IJwtService
	{
		public static readonly string Secret = "5V-6MZad=*+9qITp!%Ee7?a5!qQm2p4rHe%Ejg2*J4@EZ#@U67";
		public static string Audience = "TodoApp.com.tr";
		private readonly int ExpirationDayCount = 3;

		public string GenerateJwt(User user)
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes(JwtService.Secret);

			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Audience = JwtService.Audience,
				IssuedAt = DateTime.Now,
				Subject = new ClaimsIdentity(new Claim[]
				{
					new Claim("nameid", user.Id.ToString()),
					new Claim("given_name", user.Email.ToString()),
					new Claim("unique_name", $"{user.FirstName} {user.LastName}"),
				}),

				Expires = DateTime.UtcNow.AddDays(this.ExpirationDayCount),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			};

			var token = tokenHandler.CreateToken(tokenDescriptor);

			return tokenHandler.WriteToken(token);
		}

		public int GetExpirationDayCount()
		{
			return this.ExpirationDayCount;
		}
	}
}
