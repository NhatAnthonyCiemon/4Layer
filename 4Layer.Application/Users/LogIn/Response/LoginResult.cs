using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4Layer.Application.Users.LogIn.Response
{
	public record LoginResult(string AccessToken, string RefreshToken, Guid Id);
}
