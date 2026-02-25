using _4Layer.Application.Users.LogIn.Commands;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4Layer.Application.Users.LogIn.Validators
{
	public sealed class LoginCommandValidator : AbstractValidator<LoginCommand>
	{
		public LoginCommandValidator()
		{
			RuleFor(x => x.Email)
			.NotEmpty().WithErrorCode("ALEM01")
			.EmailAddress().WithErrorCode("ALEM02")
			.MaximumLength(256).WithErrorCode("ALEM04");

			RuleFor(x => x.Password)
				.NotEmpty().WithErrorCode("ALEM01");
		}
	}
}
