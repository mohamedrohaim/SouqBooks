using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Models;
using Models.ViewModel;
using Stripe.Radar;
using System.Data;

namespace SouqBooks.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]

    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManger;
        private readonly UserManager<ApplicationUser> _userManager;
        public RolesController(RoleManager<IdentityRole> roleManager,UserManager<ApplicationUser> userManager)
        {
            _roleManger= roleManager;
            _userManager= userManager;
        }


        public IActionResult Index()
        {
            IEnumerable<IdentityRole> roles=_roleManger.Roles.ToList();
            return View(roles);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RoleViewModel roleViewModel)
        {
            if (ModelState.IsValid)
            {
                IdentityRole identityRole = new IdentityRole() {
                    Name = roleViewModel.RoleName,
                };
                
                   var result= await _roleManger.CreateAsync(identityRole);
                if (result.Succeeded)
                {
                    TempData["success"] = "Role created successfully";
                    return RedirectToAction(nameof(Index));
                }
                else {
                    foreach (var error in result.Errors) {
                        ModelState.AddModelError("", error.Description); 
                    }
                    return View(roleViewModel); 
                }
                

            }
            return View(roleViewModel);
        }


        
        public async Task<ActionResult> RoleUsers(string role) {

            var users =await _userManager.GetUsersInRoleAsync(role);

            return View(users);
        }
    }
}
