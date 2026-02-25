using MediatR;
using _4Layer.Application.Users.Me.Response;

namespace _4Layer.Application.Users.Me.Query
{
	public record GetMeQuery() : IRequest<GetMeResponse>;
}
