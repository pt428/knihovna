using Knihovna.DTO;
using Knihovna.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Knihovna.Services
{
	public class BookService
	{
		private ApplicationDbContext _dbContext;
		private UserManager<AppUser> _userManager;
		public BookService(ApplicationDbContext dbContext, UserManager<AppUser> userManager)
		{
			_dbContext = dbContext;
			_userManager = userManager;
		}
		//*******************************
		//********* CREATE   ************
		//*******************************
		public async Task CreateAsync(BookDto bookDto)
		{
			await _dbContext.Books.AddAsync(DtoToModel(bookDto));
			await _dbContext.SaveChangesAsync();
		}
		//*******************************
		//********* READ  ************
		//*******************************
		public async Task<IEnumerable<BookDto>> GetAllAsync()
		{
			var allBooks = await _dbContext.Books.Include(x => x.UserWhoBorrowed).ToListAsync();
			var bookDtos = new List<BookDto>();
			foreach (var book in allBooks)
			{

				bookDtos.Add(modelToDto(book));
			}
			return bookDtos;
		}
		//*******************************
		//********* UPDATE  ************
		//*******************************
		public async Task<BookDto> EditAsync(BookDto bookDto)
		{
			_dbContext.Update(DtoToModel(bookDto));
			await _dbContext.SaveChangesAsync();
			return bookDto;
		}
		//*******************************
		//********* DELETE  ************
		//*******************************
		public async Task DeleteAsync(int id)
		{
			var bookToDelete = await _dbContext.Books.FirstOrDefaultAsync(x => x.Id == id);
			_dbContext.Remove(bookToDelete);
			await _dbContext.SaveChangesAsync();

		}
		//*******************************
		//********* GET BY ID  ************
		//*******************************
		public async Task<BookDto> GetByIdAsync(int id)
		{
			var book = await _dbContext.Books.FirstOrDefaultAsync(x => x.Id == id);
			return modelToDto(book);

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
				Year = book.Year,
				UserWhoBorrowedId = book.UserWhoBorrowed.Id ?? ""
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
				Borrowed = bookDto.Borrowed,
				UserWhoBorrowed = _userManager.Users.FirstOrDefault(x => x.Id == bookDto.UserWhoBorrowedId.ToString()) ?? new AppUser()
			};
		}
		//*******************************
		//********* RESERVATION   ************
		//*******************************
		public async Task ReservationAsync(int id, AppUser appUser)
		{
			Book bookToReservation = await _dbContext.Books.FirstOrDefaultAsync(x => x.Id == id);
			bookToReservation.Reserved = true;

			_dbContext.Update(bookToReservation);
			await _dbContext.SaveChangesAsync();
		}
		//*******************************
		//********* BORROW   ************
		//*******************************
		public async Task BorrowAsync(int id, AppUser appUser)
		{
			Book bookToBorrow = await _dbContext.Books.FirstOrDefaultAsync(x => x.Id == id);
			bookToBorrow.Borrowed = true;
			bookToBorrow.UserWhoBorrowed = appUser;
			_dbContext.Update(bookToBorrow);
			await _dbContext.SaveChangesAsync();
		}
	}
}
