using Knihovna.DTO;
using Knihovna.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Knihovna.Services
{
	public class ReservationService
	{
		private ApplicationDbContext _dbContext;
		private UserManager<AppUser> _userManager;
		private RoleManager<IdentityRole> _roleManager;
		public ReservationService(ApplicationDbContext dbContext,UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
		{
			_dbContext = dbContext;
			_userManager = userManager;
			_roleManager = roleManager;
		}
		//*******************************
		//********* READ  ************
		//*******************************
		public async Task<IEnumerable<BookDto>> GetAllAsync(AppUser appUser)
		{
			 
			var roles = await _userManager.GetRolesAsync(appUser);
            string roleNames = string.Join(", ", roles);            
			var allBooks = await _dbContext.Books.Where(x => x.Reserved == true).ToListAsync();
			var bookDtos = new List<BookDto>();
			foreach (var book in allBooks)
			{
				var UserWhoReserved = await _dbContext.LibraryUsers.Include(x => x.AppUser).FirstOrDefaultAsync(x => x.AppUser != null && x.AppUser.Id == book.UserWhoReservedId);
				BookDto bookDto = modelToDto(book);
				bookDto.UserWhoReservedName = UserWhoReserved is not null ? UserWhoReserved.FirstName + " " + UserWhoReserved.LastName : "";
				if (UserWhoReserved?.AppUser != null)
				{
				bookDto.UserWhoReservedEmail = UserWhoReserved is not null ? UserWhoReserved.AppUser.Email : "";
				}
				if (roleNames.Contains("Admin") || roleNames.Contains("Knihovník"))
                {
					bookDtos.Add(bookDto);
				}
				else if ( appUser.Id == book.UserWhoReservedId)
				{
					bookDtos.Add(bookDto);
				}			 
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
				Year = book.Year,
				Reserved = book.Reserved,
				Borrowed = book.Borrowed,
				DateOfReturn = book.DateOfReturn,
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
				UserWhoReservedId = bookDto.UserWhoReservedId,
				UserWhoBorrowedId = bookDto.UserWhoBorrowedId
			};
		}
	}
}
