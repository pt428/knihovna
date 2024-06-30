using Knihovna.DTO;
using Knihovna.Migrations;
using Knihovna.Models;
using Knihovna.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;


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

			Book book = DtoToModel(bookDto);
			EntityEntry e = await _dbContext.Books.AddAsync(book);
			await _dbContext.SaveChangesAsync();
		}
		//*******************************
		//********* READ  ************
		//*******************************
		public async Task<IEnumerable<BookDto>> GetAllAsync(BookParametrs bookParametrs)
		{
			IQueryable<Book> allBooks;
			//var allBooks = await _dbContext.Books.ToListAsync();
			if (bookParametrs.OrderBy == "author")
			{
				allBooks = _dbContext.Books.OrderBy(x => x.AuthorName);
			}
			else if (bookParametrs.OrderBy == "genre")
			{
				allBooks = _dbContext.Books.OrderBy(x => x.Genre);
			}
			else if (bookParametrs.OrderBy == "year")
			{
				allBooks = _dbContext.Books.OrderBy(x => x.Year);
			}
			else if (bookParametrs.OrderBy == "isbn")
			{
				allBooks = _dbContext.Books.OrderBy(x => x.ISBN);
			}
			else { allBooks = _dbContext.Books.OrderBy(x => x.Title); }

			if (bookParametrs.AuthorName != null)
			{
				allBooks = allBooks.Where(b => b.AuthorName == bookParametrs.AuthorName);
			}
			if (bookParametrs.Title != null)
			{
				allBooks = allBooks.Where(b => b.Title == bookParametrs.Title);
			}
			if (bookParametrs.Genre != null)
			{
				allBooks = allBooks.Where(b => b.Genre == bookParametrs.Genre);
			}
			if (bookParametrs.Year != null)
			{
				allBooks = allBooks.Where(b => b.Year == bookParametrs.Year);
			}
			if (bookParametrs.ISBN != null)
			{
				allBooks = allBooks.Where(b => b.ISBN == bookParametrs.ISBN);
			}

			var bookDtos = new List<BookDto>();

			foreach (var book in allBooks)
			{
				BookDto bookDto = modelToDto(book);
				var UserWhoBorrowed = await _dbContext.LibraryUsers.Include(x => x.AppUser).FirstOrDefaultAsync(x => x.AppUser != null && x.AppUser.Id == book.UserWhoBorrowedId);
				if (UserWhoBorrowed?.AppUser != null)
				{
					bookDto.UserWhoBorrowedName = UserWhoBorrowed is not null ? UserWhoBorrowed.FirstName + " " + UserWhoBorrowed.LastName : "";
					bookDto.UserWhoBorrowedEmail = UserWhoBorrowed is not null ? UserWhoBorrowed.AppUser.Email : "";
				}
				var UserWhoReserved = await _dbContext.LibraryUsers.Include(x => x.AppUser).FirstOrDefaultAsync(x => x.AppUser != null && x.AppUser.Id == book.UserWhoReservedId);
				if (UserWhoReserved?.AppUser != null)
				{
					bookDto.UserWhoReservedName = UserWhoReserved is not null ? UserWhoReserved.FirstName + " " + UserWhoReserved.LastName : "";
					bookDto.UserWhoReservedEmail = UserWhoReserved is not null ? UserWhoReserved.AppUser.Email : "";
				}
				bookDtos.Add(bookDto);

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
			if (bookToDelete != null)
			{
				_dbContext.Remove(bookToDelete);
				await _dbContext.SaveChangesAsync();
			}
		}
		//*******************************
		//********* GET BY ID  ************
		//*******************************
		public async Task<BookDto> GetByIdAsync(int id)
		{
			var book = await _dbContext.Books.FirstOrDefaultAsync(x => x.Id == id);
			if (book != null)
			{
				return modelToDto(book);
			}
			return new BookDto();
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
				DateOfReturn = book.DateOfReturn,
				Year = book.Year,
	 			UserWhoBorrowedId = book.UserWhoBorrowedId,
				UserWhoReservedId = book.UserWhoReservedId,
				UserWhoBorrowedName = "",
				UserWhoReservedName = "",
				UserWhoBorrowedEmail = "",
				UserWhoReservedEmail = ""
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
				DateOfReturn = bookDto.DateOfReturn ?? "",
				Reserved = bookDto.Reserved,
				Borrowed = bookDto.Borrowed,
				UserWhoBorrowedId = bookDto.UserWhoBorrowedId ?? "",
				UserWhoReservedId = bookDto.UserWhoReservedId ?? "",

			};
		}
		//*******************************
		//********* RESERVATION   ************
		//*******************************
		public async Task ReservationAsync(int id, AppUser appUser)
		{
			Book? bookToReservation = await _dbContext.Books.FirstOrDefaultAsync(x => x.Id == id);
			if (bookToReservation != null)
			{
				bookToReservation.Reserved = true;
				bookToReservation.UserWhoReservedId = appUser.Id;
				_dbContext.Update(bookToReservation);
				await _dbContext.SaveChangesAsync();
			}
		}
		//*******************************
		//********* RESERVATION   ************
		//*******************************
		public async Task ReservationCancelAsync(int id)
		{
			Book? bookToReservation = await _dbContext.Books.FirstOrDefaultAsync(x => x.Id == id);
			if (bookToReservation != null)
			{
				bookToReservation.Reserved = false;
				bookToReservation.UserWhoReservedId = "";
				_dbContext.Update(bookToReservation);
				await _dbContext.SaveChangesAsync();
			}

		}
		//*******************************
		//********* BORROW   ************
		//*******************************
		public async Task BorrowAsync(int id)
		{
			Book? bookToBorrow = await _dbContext.Books.FirstOrDefaultAsync(x => x.Id == id);
			if (bookToBorrow != null)
			{
				bookToBorrow.Borrowed = true;
				bookToBorrow.UserWhoBorrowedId = bookToBorrow.UserWhoReservedId;
				bookToBorrow.Reserved = false;
				bookToBorrow.UserWhoReservedId = "";
				bookToBorrow.DateOfReturn = DateTime.Now.AddDays(30).ToString("dd.MM.yyyy");

				_dbContext.Update(bookToBorrow);
				await _dbContext.SaveChangesAsync();
			}

		}
		//*******************************
		//********* BORROW CANCEL  ************
		//*******************************
		public async Task BorrowCancelAsync(int id)
		{
			Book? bookToBorrow = await _dbContext.Books.FirstOrDefaultAsync(x => x.Id == id);
			if (bookToBorrow != null)
			{
				bookToBorrow.Borrowed = false;
				bookToBorrow.UserWhoBorrowedId = "";
				bookToBorrow.DateOfReturn = "";
				_dbContext.Update(bookToBorrow);
				await _dbContext.SaveChangesAsync();
			}
		}
	}
}
