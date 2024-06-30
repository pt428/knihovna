using Knihovna.Models;
using Knihovna.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;

namespace Knihovna.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        UserManager<AppUser> _userManager;
        SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        //*******************************
        //********* LOGIN START ************
        //*******************************
        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            LoginVM loginVm = new LoginVM();
            loginVm.ReturnUrl = returnUrl;
            return View(loginVm);
        }
        public IActionResult AccessDenied()
        {
            return View();

        }
        //*******************************
        //********* LOGIN END   ************
        //*******************************
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task< IActionResult> Login(LoginVM loginVM)
        {
            if (ModelState.IsValid)
            {
                AppUser? appUser = await _userManager.FindByNameAsync(loginVM.UserName);
                if (appUser == null)
                {

                    ModelState.AddModelError(nameof(loginVM.UserName), "Přihlášení selhalo: špatné jméno nebo heslo");
                }
                else
                {
                    await _signInManager.SignOutAsync();
                    Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(appUser,loginVM.Password,loginVM.RememberMe,false);
                    if (result.Succeeded)
                    {
                        return Redirect(loginVM.ReturnUrl ?? "/");
                    }
                    else
                    {
                        ModelState.AddModelError(nameof(loginVM.UserName), "Přihlášení selhalo: špatné jméno nebo heslo");
                    }
                }
            }
            return View(loginVM);
        }
        //*******************************
        //********* LOGOUT   ************
        //*******************************
        public async Task<IActionResult> Logout()
		{
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
		}
	}
}
