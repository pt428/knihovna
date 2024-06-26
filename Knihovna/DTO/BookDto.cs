using Knihovna.Models;

namespace Knihovna.DTO
{
	public class BookDto
	{
		public int Id { get; set; }
	//	public Author Author { get; set; }
	public string AuthorName {  get; set; }
		public string Title { get; set; }
		public string ISBN { get; set; }
		public string Genre { get; set; }
		public string Description { get; set; }
	}
}
