using Knihovna.DTO;
using Knihovna.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Knihovna.Services
{
	public class BorrowedService
	{
		private ApplicationDbContext _dbContext;
		private UserManager<AppUser> _userManager;
		private RoleManager<IdentityRole> _roleManager;
		public BorrowedService(ApplicationDbContext dbContext,UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
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
			IdentityRole identityRole = await _roleManager.FindByNameAsync("Čtenář");
			bool isCtenar = await _userManager.IsInRoleAsync(appUser, identityRole.Name);
			var allBooks = await _dbContext.Books.Where(x=>x.Borrowed==true).ToListAsync();
			var bookDtos = new List<BookDto>();
			foreach (var book in allBooks)
			{
				if (isCtenar && appUser.Id == book.UserWhoBorrowedId)
				{
					bookDtos.Add(modelToDto(book));
				}
				else if (!isCtenar)
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
				Year = book.Year,
				Reserved = book.Reserved,
				Borrowed = book.Borrowed,
				UserWhoReservedId = book.UserWhoReservedId,
				UserWhoBorrowedId = book.UserWhoBorrowedId
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
				UserWhoReservedId= bookDto.UserWhoReservedId,
				UserWhoBorrowedId= bookDto.UserWhoBorrowedId
			};
		}
	}
}
