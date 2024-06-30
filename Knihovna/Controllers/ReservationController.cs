using Knihovna.Models;
using Knihovna.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Knihovna.Controllers
{
	[Authorize]
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
			AppUser? user = await _userManager.GetUserAsync(HttpContext.User);
			if (user != null)
			{
				var allBooks = await _reservationService.GetAllAsync(user);
				return View(allBooks);
			}
			return View();
		}
	}
}
