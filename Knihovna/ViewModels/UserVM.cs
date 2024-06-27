using System.ComponentModel.DataAnnotations;

namespace Knihovna.ViewModels
{
	public class UserVM
	{
		public int Id { get; set; }
		[Required]
		public string UserName { get; set; }
		[Required]
		public string Password { get; set; }
		[Required]
		[RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$",ErrorMessage = "E-mail is not valid")]
		public string Email { get; set; }
	}
}
