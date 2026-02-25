using _4Layer.Application.Abstractions.Security;
using _4Layer.Domain.Common;
using _4Layer.Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;


namespace _4Layer.Infrastructure.Security
{
	public class JwtSettings
	{
		public string SecretKey { get; set; } = null!;
		public string Issuer { get; set; } = null!;
		public string Audience { get; set; } = null!;
		public int ExpiryMinutes { get; set; }
		public int RefreshTokenExpiryDays { get; set; }
	}
	public class JwtTokenGenerator : IJwtTokenGenerator
	{
		private readonly JwtSettings _jwtSettings;
		public JwtTokenGenerator(IOptions<JwtSettings> jwtSettings)
		{
			_jwtSettings = jwtSettings.Value;
		}
		public string GenerateToken(User user)
		{
			return GenerateJwtToken(user, _jwtSettings.ExpiryMinutes);
		}
		public RefreshTokenResult GenerateRefreshToken(User user)
		{
			int refreshExpiryMinutes = _jwtSettings.RefreshTokenExpiryDays * 24 * 60;
			var token = GenerateJwtToken(user, refreshExpiryMinutes);
			var expiryDate = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpiryDays);

			return new RefreshTokenResult(Token: token, ExpiryDate: expiryDate);
		}
		private string GenerateJwtToken(User user, int expiryMinutes)
		{
			var signingCredentials = new SigningCredentials(
				new SymmetricSecurityKey(
					Encoding.UTF8.GetBytes(_jwtSettings.SecretKey)),
				SecurityAlgorithms.HmacSha256);

			var claims = new[]
			{
				new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
				new Claim(ClaimTypes.Email, user.Email),
				new Claim(ClaimTypes.Role, user.Role.ToString()),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
			};

			var expiryTime = DateTime.UtcNow.AddMinutes(expiryMinutes);

			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(claims),
				Expires = expiryTime,
				Issuer = _jwtSettings.Issuer,
				Audience = _jwtSettings.Audience,
				SigningCredentials = signingCredentials
			};

			var tokenHandler = new JwtSecurityTokenHandler();
			var securityToken = tokenHandler.CreateToken(tokenDescriptor);
			var token = tokenHandler.WriteToken(securityToken);

			return token;
		}
		public Guid GetUserIdFromToken(string token)
		{
			try
			{
				// Log received token payload
				var parts = token.Split('.');
				if (parts.Length == 3)
				{
					var payload = parts[1];
					var padding = 4 - payload.Length % 4;
					if (padding != 4) payload += new string('=', padding);
					var payloadBytes = Convert.FromBase64String(payload);
					var payloadJson = Encoding.UTF8.GetString(payloadBytes);
					Console.WriteLine($"[VAL] Received Token Payload: {payloadJson}");
				}

				Console.WriteLine($"[VAL] Settings - ValidIssuer: '{_jwtSettings.Issuer}', ValidAudience: '{_jwtSettings.Audience}'");

				var tokenHandler = new JwtSecurityTokenHandler();

				var validationParameters = new TokenValidationParameters
				{
					ValidateIssuer = false, // Disable for now
					ValidateAudience = false, // Disable for now
					ValidateLifetime = true,
					RequireExpirationTime = false,
					ValidateIssuerSigningKey = true,

					ValidIssuer = _jwtSettings.Issuer,
					ValidAudience = _jwtSettings.Audience,

					IssuerSigningKey = new SymmetricSecurityKey(
										Encoding.UTF8.GetBytes(_jwtSettings.SecretKey)
					),

					ClockSkew = TimeSpan.FromSeconds(10)
				};

				Console.WriteLine($"[VAL] Starting validation...");
				var principal = tokenHandler.ValidateToken(
					token,
					validationParameters,
					out SecurityToken validatedToken);

				Console.WriteLine($"[VAL] Token validated successfully");

				var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier);

				if (userIdClaim == null)
				{
					throw new AppError("Invalid token: User ID claim is missing.", ErrorStatus.Unauthorized);
				}
				Console.WriteLine($"[VAL] Extracted User ID Claim: {userIdClaim.Value}");
				return Guid.Parse(userIdClaim.Value);
			}
			catch (SecurityTokenExpiredException ex)
			{
				throw new AppError($"Token has expired. {ex.Message}", ErrorStatus.Unauthorized);
			}
			catch (SecurityTokenInvalidSignatureException ex)
			{
				throw new AppError($"Token signature is invalid. {ex.Message}", ErrorStatus.Unauthorized);
			}
			catch (SecurityTokenException ex)
			{
				throw new AppError($"Token validation failed: {ex.Message}", ErrorStatus.Unauthorized);
			}
			catch (Exception ex)
			{
				throw new AppError($"Error validating token: {ex.Message}", ErrorStatus.Unauthorized);
			}
		}
	}
}
