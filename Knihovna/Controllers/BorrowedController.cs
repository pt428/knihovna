using Knihovna.Models;
using Knihovna.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Knihovna.Controllers
{
    [Authorize]
    public class BorrowedController : Controller
	{
		private BorrowedService _borrowedService;
		private UserManager<AppUser> _userManager;
		 
		public BorrowedController(BorrowedService borrowedService, UserManager<AppUser> userManager )
		{
			_borrowedService = borrowedService;
			_userManager = userManager;
			 
		}

		//*******************************
		//********* INDEX   ************
		//*******************************
		public async Task<IActionResult> Index()
		{
			AppUser? user = await _userManager.GetUserAsync(HttpContext.User);
            if (user != null)
            {
            var allBooks = await _borrowedService.GetAllAsync(user);
			return View(allBooks);
                
            }
			return View();
		}
	}
}
