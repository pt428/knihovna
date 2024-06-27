using Knihovna.DTO;
using Knihovna.Models;
using Microsoft.EntityFrameworkCore;

namespace Knihovna.Services
{
	public class ReservationService
	{
		private ApplicationDbContext _dbContext;

		public ReservationService(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}
		//*******************************
		//********* READ  ************
		//*******************************
		public async Task<IEnumerable<BookDto>> GetAllAsync()
		{
			var allBooks = await _dbContext.Books.Where(x => x.Reserved == true).ToListAsync();
			var bookDtos = new List<BookDto>();
			foreach (var book in allBooks)
			{

				bookDtos.Add(modelToDto(book));
			}
			return bookDtos;
		}
		//*******************************
		//********* MODEL TO DTO  ************
		//*******************************
		private BookDto modelToDto(Book book)
		{
			return new BookDto()
			{
				Id = book.Id,
				AuthorName = book.AuthorName,
				Title = book.Title,
				ISBN = book.ISBN,
				Genre = book.Genre,
				Description = book.Description,
				Borrowed = book.Borrowed,
				Reserved = book.Reserved,
				Year = book.Year
			};
		}
		//*******************************
		//********* DTO TO MODEL  ************
		//*******************************
		private Book DtoToModel(BookDto bookDto)
		{

			return new Book()
			{
				Id = bookDto.Id,
				AuthorName = bookDto.AuthorName,
				Title = bookDto.Title,
				ISBN = bookDto.ISBN,
				Genre = bookDto.Genre,
				Description = bookDto.Description,
				Year = bookDto.Year,
				Reserved = bookDto.Reserved,
				Borrowed = bookDto.Borrowed
			};
		}
	}
}
