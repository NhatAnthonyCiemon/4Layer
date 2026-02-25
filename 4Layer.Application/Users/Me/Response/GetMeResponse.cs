using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4Layer.Application.Users.Me.Response
{
	public class GetMeResponse
	{
		public Guid Id { get; set; }
		public string Email { get; set; } = null!;
		public int Role { get; set; }
		public string? AvatarUrl { get; set; }
		public string? PhoneNumber { get; set; }
	}
}
