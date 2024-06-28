namespace Knihovna.Models
{
	public class LibraryUser
	{
		public int Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public DateTime DateOfBirth { get; set; }
		public AppUser AppUser { get; set; }
	}
}
