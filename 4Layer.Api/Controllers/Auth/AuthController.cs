using MediatR;
using Microsoft.AspNetCore.Mvc;
using _4Layer.Api.Controllers.Auth.DTO;
using _4Layer.Domain.Enums;
using _4Layer.Api.Common.Responses;
using _4Layer.Api.Common.Helper;
using Microsoft.AspNetCore.Authorization;
using _4Layer.Application.Users.LogIn.Commands;
using _4Layer.Application.Users.Me.Query;
using _4Layer.Domain.Common;
using _4Layer.Application.Users.LogIn.Response;
using _4Layer.Application.Users.Refresh.Commands;

namespace _4Layer.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
	private readonly IMediator _mediator;

	public UserController(IMediator mediator)
	{
		_mediator = mediator;
	}

	[HttpPost("signup")]
	[AllowAnonymous]
	public async Task<IActionResult> SignUp(
	[FromBody] SignUpRequest request,
	CancellationToken cancellationToken)
	{
		var command = new CreateUserCommand
		{
			Email = request.Email,
			Password = request.Password,
			PhoneNumber = request.PhoneNumber,
			AvatarUrl = null,
			Role = UserRole.User // set mặc định
		};

		var result = await _mediator.Send(command, cancellationToken);

		return StatusHelper.Success(new
		{
			Id = result
		},
				"Sign up successful", SuccessStatus.Created);
	}

	[HttpPost("login")]
	[AllowAnonymous]
	public async Task<IActionResult> Login(
		[FromBody] LoginRequest request,
		CancellationToken cancellationToken
		)
	{
		var command = new LoginCommand(Email: request.Email, Password: request.Password);
		var result = await _mediator.Send(command, cancellationToken);
		return StatusHelper.Success(result, "Login Success");
	}

	[HttpGet("me")]
	public async Task<IActionResult> GetMe(CancellationToken cancellationToken)
	{
		var result = await _mediator.Send(new GetMeQuery(), cancellationToken);
		return StatusHelper.Success(result, "Get user info success");
	}

	[HttpPost("refresh-token")]
	[AllowAnonymous]
	public async Task<IActionResult> RefreshToken(
		[FromBody] RefreshTokenRequest request,
		CancellationToken cancellationToken)
	{
		var command = new RefreshCommand(request.RefreshToken);
		var result = await _mediator.Send(command, cancellationToken);
		return StatusHelper.Success(result, "Refresh token success");
	}

}