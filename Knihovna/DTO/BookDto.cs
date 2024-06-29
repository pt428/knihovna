using Knihovna.Models;

namespace Knihovna.DTO
{
	public class BookDto
	{
        public int Id { get; set; }
         public string AuthorName { get; set; }
        public string Title { get; set; }
        public string ISBN { get; set; }
        public string Genre { get; set; }
        public string Description { get; set; }
        public string Year { get; set; }
        public bool Reserved { get; set; }
        public bool Borrowed { get; set; }
        public string UserWhoBorrowedId { get; set; }
        public string UserWhoReservedId { get; set; }        
        public string UserWhoBorrowedName { get; set; }
        public string UserWhoReservedName { get; set; }        
        public string UserWhoBorrowedEmail { get; set; }
        public string UserWhoReservedEmail { get; set; }
        public string DateOfReturn { get; set; }
    }
}
