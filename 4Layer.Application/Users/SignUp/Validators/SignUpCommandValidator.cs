using FluentValidation;

public sealed class CreateUserCommandValidator
	: AbstractValidator<CreateUserCommand>
{
	public CreateUserCommandValidator()
	{
		RuleFor(x => x.Email)
			.NotEmpty().WithErrorCode("ALEM01")
			.EmailAddress().WithErrorCode("ALEM02")
			.MaximumLength(256).WithErrorCode("ALEM04");

		RuleFor(x => x.Password)
			.NotEmpty().WithErrorCode("ALEM01")
			.MinimumLength(8).WithErrorCode("ALEM03")
			.Matches(@"[A-Z]").WithErrorCode("ALEM02")
			.Matches(@"[a-z]").WithErrorCode("ALEM02")
			.Matches(@"[0-9]").WithErrorCode("ALEM02");

		RuleFor(x => x.PhoneNumber)
			.Matches(@"^\+?[0-9]{9,15}$")
			.When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber))
			.WithErrorCode("ALEM02");

		RuleFor(x => x.AvatarUrl)
			.Must(BeValidUrl)
			.When(x => !string.IsNullOrWhiteSpace(x.AvatarUrl))
			.WithErrorCode("ALEM02");

		RuleFor(x => x.Role)
			.IsInEnum().WithErrorCode("ALEM02");
	}

	private bool BeValidUrl(string? url)
	{
		return Uri.TryCreate(url, UriKind.Absolute, out _);
	}
}