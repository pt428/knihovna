using Knihovna.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Globalization;

namespace Knihovna.Controllers
{
    
    public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private UserManager<AppUser> _userManager;


		public HomeController(ILogger<HomeController> logger,UserManager<AppUser> userManager )
		{
			_logger = logger;
			_userManager = userManager;
		}
        //*******************************
        //********* INDEX   ************
        //*******************************
        [Authorize]
        public async Task< IActionResult> Index()
		{
			AppUser? user = await _userManager.GetUserAsync(HttpContext.User);
			//string? message =   user.UserName;

			return View("Index" );
		}
        //*******************************
        //********* PRIVACY   ************
        //*******************************
        public IActionResult Privacy()
		{
			return View();
		}
        //*******************************
        //********* ERROR   ************
        //*******************************
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
