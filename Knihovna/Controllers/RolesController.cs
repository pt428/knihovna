using Knihovna.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Knihovna.Controllers
{
    public class RolesController : Controller
    {
        private RoleManager<IdentityRole> _roleManager;
        private UserManager<AppUser> _userManager;

        public RolesController(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }
        //*******************************
        //********* INDEX   ************
        //*******************************
        public IActionResult Index()
        {
            return View(_roleManager.Roles);
        }
        //*******************************
        //********* CREATE END   ************
        //*******************************        
        public IActionResult Create() => View();
        //*******************************
        //********* CREATE END   ************
        //*******************************
        [HttpPost]
        public async Task< IActionResult> Create([Required] string name)
        {
            if (ModelState.IsValid)
            {
                IdentityResult identityResult = await _roleManager.CreateAsync(new IdentityRole { Name = name });

                if (identityResult.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    Errors(identityResult);
                }
            }
            return View();
        }
        //*******************************
        //********* UPDATE START  ************
        //*******************************
        public async Task<IActionResult> Edit(string id)
        {
            IdentityRole identityRole = await _roleManager.FindByIdAsync(id);
            List<AppUser> members = new List<AppUser>();
            List<AppUser> nonMembers = new List<AppUser>();
            foreach (AppUser appUser in _userManager.Users)
            {
                var list = await _userManager.IsInRoleAsync(appUser, identityRole.Name)?members:nonMembers;
                list.Add(appUser);

            }
            return View(new RoleEdit()
            {
                Role = identityRole,
                Members = members,
                NonMembers = nonMembers
            });
        }
        //*******************************
        //********* UPDATE END  ************
        //*******************************
        [HttpPost]
        public async Task<IActionResult> Edit(RoleModification roleModification)
        {
            IdentityResult identityResult;
            if (ModelState.IsValid)
            {
                foreach (string userId in roleModification.AddIds ?? new string[] { })
                {
                    AppUser user = await _userManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        identityResult = await _userManager.AddToRoleAsync(user, roleModification.RoleName);
                        if (!identityResult.Succeeded)
                        {
                            Errors(identityResult);
                        }
                    }

                }
                foreach (string userId in roleModification.DeleteIds ?? new string[] { })
                {
                    AppUser user = await _userManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        identityResult = await _userManager.RemoveFromRoleAsync(user, roleModification.RoleName);
                        if (!identityResult.Succeeded)
                        {
                            Errors(identityResult);

                        }
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return await Edit(roleModification.RoleId);
            } 
        }
        //*******************************
        //********* DELETE   ************
        //*******************************
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            IdentityRole identityRole = await _roleManager.FindByIdAsync(id);
            if (identityRole != null)
            {
                IdentityResult identityResult = await _roleManager.DeleteAsync(identityRole);
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
                ModelState.AddModelError("", "Nenalezeno");
            }
            return View("Index",_roleManager.Roles);
        }

        //*******************************
        //********* ERRORS   ************
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
