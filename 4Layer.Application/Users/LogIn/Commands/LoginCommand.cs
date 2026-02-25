using _4Layer.Application.Users.LogIn.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4Layer.Application.Users.LogIn.Commands
{
	public record LoginCommand(string Email, string Password) : IRequest<LoginResult>;
}
