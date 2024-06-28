using Knihovna.Models;
using Knihovna.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Knihovna.Controllers
{
	public class ReservationController : Controller
	{
		private ReservationService _reservationService;
		private UserManager<AppUser> _userManager;
		public ReservationController(ReservationService reservationService, UserManager<AppUser> userManager)
		{
			_reservationService = reservationService;
			_userManager = userManager;
		}

		//*******************************
		//********* INDEX   ************
		//*******************************
		public async Task<IActionResult> Index()
		{
			AppUser user = await _userManager.GetUserAsync(HttpContext.User);
			var allBooks = await _reservationService.GetAllAsync(user);
			return View(allBooks);
		}
	}
}
