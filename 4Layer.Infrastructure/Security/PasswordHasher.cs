using System.Security.Cryptography;
using System.Text;
using Konscious.Security.Cryptography;
using _4Layer.Application.Abstractions.Security;

namespace _4Layer.Infrastructure.Security
{
	public class PasswordHasher : IPasswordHasher
	{
		public string Hash(string password)
		{
			var salt = RandomNumberGenerator.GetBytes(16);

			var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password))
			{
				Salt = salt,
				DegreeOfParallelism = 1,
				MemorySize = 65536,
				Iterations = 3
			};

			var hash = argon2.GetBytes(32);

			return Convert.ToBase64String(salt) + "." +
				   Convert.ToBase64String(hash);
		}

		public bool Verify(string password, string storedHash)
		{
			try
			{
				var parts = storedHash.Split('.');
				var salt = Convert.FromBase64String(parts[0]);
				var hash = Convert.FromBase64String(parts[1]);

				var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password))
				{
					Salt = salt,
					DegreeOfParallelism = 1,
					MemorySize = 65536,
					Iterations = 3
				};

				var computedHash = argon2.GetBytes(32);

				return CryptographicOperations.FixedTimeEquals(hash, computedHash);
			}
			catch
			{
				return false;
			}
		}
	}
}