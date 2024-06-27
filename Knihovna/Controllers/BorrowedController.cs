using Knihovna.Services;
using Microsoft.AspNetCore.Mvc;

namespace Knihovna.Controllers
{
	public class BorrowedController : Controller
	{
		private BorrowedService _borrowedService;

		public BorrowedController(BorrowedService borrowedService)
		{
			_borrowedService = borrowedService;
		}

		//*******************************
		//********* INDEX   ************
		//*******************************
		public async Task<IActionResult> Index()
		{
			var allBooks = await _borrowedService.GetAllAsync();
			return View(allBooks);
		}
	}
}
