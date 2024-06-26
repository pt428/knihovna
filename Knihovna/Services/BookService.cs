using Knihovna.DTO;
using Knihovna.Models;
using Microsoft.EntityFrameworkCore;

namespace Knihovna.Services
{
	public class BookService
	{
		private ApplicationDbContext _dbContext;
		public BookService(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}
		public async Task<IEnumerable<BookDto>> GetAllAsync()
		{
			var allBooks = await _dbContext.Books.ToListAsync();
			var bookDtos = new List<BookDto>();
			foreach (var book in allBooks)
			{

				bookDtos.Add(modelToDto(book));
			}
			return bookDtos;
		}
		public async Task  CreateAsync(BookDto bookDto)
		{
			await _dbContext.Books.AddAsync(DtoToModel(bookDto));
			await _dbContext.SaveChangesAsync();
		}
		public async Task<BookDto> EditAsync(BookDto bookDto)
		{
			_dbContext.Update(DtoToModel( bookDto));
			await _dbContext.SaveChangesAsync();
			return bookDto;
		}
		public async Task  DeleteAsync(int id)
		{
			var bookToDelete = await _dbContext.Books.FirstOrDefaultAsync(x => x.Id == id);
			  _dbContext.Remove(bookToDelete);
		await	_dbContext.SaveChangesAsync();
			 
		}
		public async Task<BookDto> GetByIdAsync(int id)
		{
			 var book = await _dbContext.Books.FirstOrDefaultAsync(x => x.Id == id);
			return modelToDto(book);

		}

		private BookDto modelToDto(Book book)
		{
			 
			return new BookDto()
			{
				Id = book.Id,
				AuthorName = book.AuthorName,
				Title = book.Title,
				ISBN = book.ISBN,
				Genre = book.Genre,
				Description = book.Description
			};
		}
		private Book  DtoToModel(BookDto bookDto)
		{
			
			return new Book()
			{
				Id = bookDto.Id,
				AuthorName =  bookDto.AuthorName,
				Title = bookDto.Title,
				ISBN = bookDto.ISBN,
				Genre = bookDto.Genre,
				Description = bookDto.Description
			};
		}
	}
}
