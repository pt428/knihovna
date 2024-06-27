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
        private IPasswordValidator<AppUser> _passwordValidator;
        private RoleManager<IdentityRole> _roleManager;

        public UsersController(UserManager<AppUser> userManager, IPasswordHasher<AppUser> passwordHasher, IPasswordValidator<AppUser> passwordValidator, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _passwordHasher = passwordHasher;
            _passwordValidator = passwordValidator;
            _roleManager = roleManager;
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
                IdentityRole identityRole = await _roleManager.FindByNameAsync("Čtenář");
                if (identityRole == null)
                {
                    IdentityResult identityResult = await _roleManager.CreateAsync(new IdentityRole { Name = "Čtenář" });
                    if (identityResult == null)
                    {
                       Errors(identityResult);
                    }
                }
                await _userManager.AddToRoleAsync(appUser, "Čtenář");
                if ((result.Succeeded))
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    Errors(result);
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
                IdentityResult validPass=null;
                validPass = await _passwordValidator.ValidateAsync(_userManager,appUser,password);
                if (validPass.Succeeded)
                {

                appUser.PasswordHash = _passwordHasher.HashPassword(appUser, password);
                }
                else
                {
                    Errors(validPass);
                }
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