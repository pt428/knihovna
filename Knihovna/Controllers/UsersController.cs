using Knihovna.Models;
using Knihovna.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Knihovna.Controllers
{
    public class UsersController : Controller
    {
        private UserManager<AppUser> _userManager;
        private IPasswordHasher<AppUser> _passwordHasher;

        public UsersController(UserManager<AppUser> userManager, IPasswordHasher<AppUser> passwordHasher)
        {
            _userManager = userManager;
            _passwordHasher = passwordHasher;
        }
        //*******************************
        //********* INDEX   ************
        //*******************************
        public IActionResult Index()
        {
            return View(_userManager.Users);
        }
        //*******************************
        //********* CREATE START  ************
        //*******************************
        public ViewResult Create() => View();
        //*******************************
        //********* CREATE END  ************
        //*******************************
        [HttpPost]
        public async Task<IActionResult> Create(UserVM userVM)
        {
            if (ModelState.IsValid)
            {
                AppUser appUser = new AppUser()
                {
                    UserName = userVM.UserName,
                    Email = userVM.Email,

                };
                IdentityResult result = await _userManager.CreateAsync(appUser, userVM.Password);
                if ((result.Succeeded))
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(userVM);
        }
        //*******************************
        //********* UPDATE START   ************
        //*******************************
        public async Task<IActionResult> Edit(string id)
        {
            AppUser appUserToEdit = await _userManager.FindByIdAsync(id);
            if (appUserToEdit == null)
            {
                return View("Notfound");
            }
            return View(appUserToEdit);
        }
        //*******************************
        //********* UPDATE END   ************
        //*******************************
        [HttpPost]
        public async Task<IActionResult> EditAsync(string id, string email, string password)
        {
            AppUser appUser = await _userManager.FindByIdAsync(id);
            if (appUser != null)
            {
                appUser.Email = email;
                appUser.PasswordHash = _passwordHasher.HashPassword(appUser, password);
                IdentityResult identityResult = await _userManager.UpdateAsync(appUser);

                if (identityResult.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    Errors(identityResult);
                }
            }
            else
            {
                ModelState.AddModelError("", "Uživatel nenalezen");
            }
            return View(appUser);
        }
        //*******************************
        //********* DELETE   ************
        //*******************************
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            AppUser appUser = await _userManager.FindByIdAsync(id);
            if (appUser != null)
            {
                IdentityResult identityResult = await _userManager.DeleteAsync(appUser);
                if (identityResult.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else { Errors(identityResult); }

            }
            else
            {
                ModelState.AddModelError("", "Uživatel nenalezen");
            }
            return View("Index",_userManager.Users);
        }
        //*******************************
        //********* ADD ERRORS   ************
        //*******************************
        private void Errors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }
    }
}