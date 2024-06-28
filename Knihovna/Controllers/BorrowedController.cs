using Knihovna.Models;
using Knihovna.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Knihovna.Controllers
{
	public class BorrowedController : Controller
	{
		private BorrowedService _borrowedService;
		private UserManager<AppUser> _userManager;
		private RoleManager<IdentityRole> _roleManager;
		public BorrowedController(BorrowedService borrowedService, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
		{
			_borrowedService = borrowedService;
			_userManager = userManager;
			_roleManager = roleManager;
		}

		//*******************************
		//********* INDEX   ************
		//*******************************
		public async Task<IActionResult> Index()
		{
			AppUser user = await _userManager.GetUserAsync(HttpContext.User);
			var allBooks = await _borrowedService.GetAllAsync(user);
			return View(allBooks);
		}
	}
}
