using System.ComponentModel.DataAnnotations;

namespace _4Layer.Api.Controllers.Auth.DTO
{
	/// <summary>
	/// Thông tin đăng ký tài khoản mới.
	/// </summary>
	public class LoginRequest
	{
		/// <summary>
		/// Email dùng để đăng nhập và xác thực.
		/// </summary>
		///<example>user@example.com</example>
		[Required]
		public string Email { get; set; } = null!;

		/// <summary>
		/// Mật khẩu bảo mật (tối thiểu 8 ký tự).
		///</summary>
		///<example>P@ssw0rd123</example>
		[Required]
		public string Password { get; set; } = null!;
	}
}
