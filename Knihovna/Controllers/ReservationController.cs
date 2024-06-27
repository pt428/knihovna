using Knihovna.Services;
using Microsoft.AspNetCore.Mvc;

namespace Knihovna.Controllers
{
	public class ReservationController : Controller
	{
		private ReservationService _reservationService;

		public ReservationController(ReservationService reservationService)
		{
			_reservationService = reservationService;
		}

		//*******************************
		//********* INDEX   ************
		//*******************************
		public async Task<IActionResult> Index()
		{
			var allBooks = await _reservationService.GetAllAsync();
			return View(allBooks);
		}
	}
}
