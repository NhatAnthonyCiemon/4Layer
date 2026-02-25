using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace _4Layer.Api.Controllers.Auth.DTO;

/// <summary>
/// Thông tin đăng ký tài khoản mới.
/// </summary>
public sealed class SignUpRequest
{
	/// <summary>
	/// Địa chỉ Email dùng để đăng nhập và xác thực.
	/// </summary>
	/// <example>user@example.com</example>
	[Required]
	public string Email { get; init; } = null!;

	/// <summary>
	/// Mật khẩu bảo mật (tối thiểu 8 ký tự).
	/// </summary>
	/// <example>P@ssw0rd123</example>
	[Required]
	public string Password { get; init; } = null!;

	/// <summary>
	/// Số điện thoại liên lạc (tùy chọn).
	/// </summary>
	/// <example>0901234567</example>
	public string? PhoneNumber { get; init; }

	/// <summary>
	/// Đường dẫn ảnh đại diện (URL).
	/// </summary>
	/// <example>https://example.com/avatars/user.jpg</example>
	public string? AvatarUrl { get; init; }
}