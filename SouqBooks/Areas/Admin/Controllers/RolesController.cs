using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Models.ViewModel;

namespace SouqBooks.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManger;
        public RolesController(RoleManager<IdentityRole> roleManager)
        {
            _roleManger= roleManager;
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
    }
}
