using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _4Layer.Application.Users.Me.Response;
using MediatR;
using _4Layer.Application.Abstractions.Security;
using _4Layer.Application.Users.Me.Query;
using _4Layer.Domain.Repositories;
using _4Layer.Domain.Common;

namespace _4Layer.Application.Users.Me.Handler
{
	public class GetMeQueryHandler: IRequestHandler<GetMeQuery, GetMeResponse>
	{
		private readonly ICurrentUser _currentUser;
		private readonly IUserRepository _userRepository;
		public GetMeQueryHandler(ICurrentUser currentUser, IUserRepository userRepository)
		{
			_currentUser = currentUser;
			_userRepository = userRepository;
		}

		public async Task<GetMeResponse> Handle(GetMeQuery request, CancellationToken cancellationToken)
		{
			var user = await _userRepository.GetUserById(_currentUser.UserId, cancellationToken);
			if (user == null)
			{
				throw new AppError("User not found.", ErrorStatus.NotFound);
			}
			return new GetMeResponse
			{
				Id = user.Id,
				Email = user.Email,
				Role = (int)user.Role,
				AvatarUrl = user.AvatarUrl,
				PhoneNumber = user.PhoneNumber
			};
		}
	}
}
