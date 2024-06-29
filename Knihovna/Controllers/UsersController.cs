using Knihovna.Models;
using Knihovna.Services;
using Knihovna.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Knihovna.Controllers
{
   
    public class UsersController : Controller
    {
        private UserManager<AppUser> _userManager;
        private IPasswordHasher<AppUser> _passwordHasher;
        private IPasswordValidator<AppUser> _passwordValidator;
        private RoleManager<IdentityRole> _roleManager;
        private LibraryUserService _libraryUserService;

        public UsersController(UserManager<AppUser> userManager, IPasswordHasher<AppUser> passwordHasher, IPasswordValidator<AppUser> passwordValidator, RoleManager<IdentityRole> roleManager, LibraryUserService libraryUserService) { 
            _userManager = userManager;
            _passwordHasher = passwordHasher;
            _passwordValidator = passwordValidator;
            _roleManager = roleManager;
            _libraryUserService = libraryUserService;
            
        }
		//*******************************
		//********* INDEX   ************
		//*******************************
		[Authorize(Roles = "Admin")]
		public async Task< IActionResult> Index()
        {
            var allUsers = await _libraryUserService.GetAllAsync();

			return View(allUsers);
           // return View(_userManager.Users);
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
                    UserName = userVM.Email,
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
                    LibraryUser libraryUser = new LibraryUser()
                    {
                        FirstName = userVM.FirstName,
                        LastName = userVM.LastName,
                        DateOfBirth = userVM.DateOfBirth,
                        AppUser = appUser

                    };
                    await _libraryUserService.CreateAsync(libraryUser);
                    return RedirectToAction("Index");
                }
                else
                {
                    Errors(result);
                }
             
            }
            return View(userVM);
        }
		[Authorize(Roles = "Admin")]
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
		[Authorize(Roles = "Admin")]
		[HttpPost]
        public async Task<IActionResult> EditAsync(string id, string email, string password)
        {
            AppUser appUser = await _userManager.FindByIdAsync(id);
            if (appUser != null)
            {
                appUser.UserName = email;
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
		[Authorize(Roles = "Admin")]
		[HttpPost]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            string appUserId=string.Empty;
            LibraryUser libraryUser = await _libraryUserService.GetByIdAsync(id);
            if (libraryUser != null)
            {
				appUserId= await _libraryUserService.DeleteAsync(id);
			}
            else
            {
				ModelState.AddModelError("", "Uživatel nenalezen");
			}
            AppUser appUser = await _userManager.FindByIdAsync(appUserId);
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
            return RedirectToAction("Index");
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