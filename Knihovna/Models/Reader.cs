namespace Knihovna.Models
{
	public class Reader
	{
		public int Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public DateTime DateOfBirth { get; set; }
		public List<Book> BorrowedBooks { get; set; }
		public List<Book> ReservedBooks { get; set; }
	}
}
