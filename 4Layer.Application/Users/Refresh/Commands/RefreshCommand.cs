using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _4Layer.Application.Users.LogIn.Response;
using MediatR;

namespace _4Layer.Application.Users.Refresh.Commands
{
	public record RefreshCommand(string RefreshToken) : IRequest<LoginResult>;

}
