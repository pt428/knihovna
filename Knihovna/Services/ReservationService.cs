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
				if (roleNames.Contains("Admin") || roleNames.Contains("Admin"))
                {
					bookDtos.Add(modelToDto(book));
				}
				else if ( appUser.Id == book.UserWhoReservedId)
				{
					bookDtos.Add(modelToDto(book));
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
