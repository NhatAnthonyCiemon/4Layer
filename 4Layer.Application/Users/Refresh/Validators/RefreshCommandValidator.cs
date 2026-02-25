using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using _4Layer.Application.Users.Refresh.Commands;

namespace _4Layer.Application.Users.Refresh.Validators
{
	public class RefreshCommandValidator : AbstractValidator<RefreshCommand>
	{
		public RefreshCommandValidator()
		{
			RuleFor(x => x.RefreshToken)
				.NotEmpty().WithErrorCode("ALEM01");
		}
	}
}
