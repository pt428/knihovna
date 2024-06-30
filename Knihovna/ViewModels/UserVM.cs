using System.ComponentModel.DataAnnotations;

namespace Knihovna.ViewModels
{
	public class UserVM
	{
		public int Id { get; set; }
		public string? AppUserId { get; set; }
		[Required]
		public string? UserName { get; set; }
		[Required]
		public string? Password { get; set; }
		[Required]
		[RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$",ErrorMessage = "E-mail is not valid")]
		public string? Email { get; set; }

		public string? FirstName { get; set; }
		public string? LastName { get; set; }
		public  string?   RoleNames { get; set; }
		public DateTime DateOfBirth { get; set; }
	}
}
