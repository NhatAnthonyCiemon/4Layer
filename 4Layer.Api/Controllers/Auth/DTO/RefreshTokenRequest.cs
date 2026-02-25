using System.ComponentModel.DataAnnotations;

namespace _4Layer.Api.Controllers.Auth.DTO
{
    /// <summary>
    /// Yêu cầu làm mới token truy cập.
    /// </summary>
    public class RefreshTokenRequest
    {
        /// <summary>
        /// Token làm mới được cấp khi đăng nhập.
        /// </summary>
        /// <example>eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...</example>
        [Required]
        public string RefreshToken { get; set; } = null!;
    }
}