using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.ViewModel;
using System.Collections.Generic;
using System.Data;

namespace SouqBooks.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public UsersController(UserManager<ApplicationUser> userManager)
        {
            _userManager= userManager;
        }
        public IActionResult Index()
        {
            IEnumerable<ApplicationUser> applicationUsers = _userManager.Users.ToList();
            IEnumerable<UserViewModel> userViewModels = MapApplicationUsersToUserViewModels(applicationUsers);
            return View(userViewModels);
        }

        public async Task<IActionResult> ManagUsereRoles(string id) {
            if (id != null) {
                var user=await _userManager.FindByIdAsync(id);
                if (user != null)
                {
                    var userViewModel = mappApplicationToViewModelUser(user);
                    return View(userViewModel);
                }
                else { 
                            return NotFound(); 

                }
            }
            return NotFound();

        
        }
        [HttpPost]
        public async Task<ActionResult> ManagUsereRoles(ApplicationUser userViewModel, List<string> selectedRoles)
        {
            var user = await _userManager.FindByIdAsync(userViewModel.Id);
            if (user != null)
            {
                try
                {
                    var userRoles = await _userManager.GetRolesAsync(user);
                    await _userManager.RemoveFromRolesAsync(user, userRoles);
                    await _userManager.AddToRolesAsync(user, selectedRoles);
                }
                catch(Exception ex)
                {
                    TempData["error"] = ex.Message;
                    return View(userViewModel);
                }
            }
            else
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }






        private IEnumerable<UserViewModel> MapApplicationUsersToUserViewModels(IEnumerable<ApplicationUser> applicationUser)
        {
            var viewModels = new List<UserViewModel>();


            foreach (var user in applicationUser)
            {
                
                viewModels.Add(mappApplicationToViewModelUser(user));
            }

            return viewModels;
        }
        private UserViewModel mappApplicationToViewModelUser(ApplicationUser user) {
            var viewModel = new UserViewModel
            {
                Id = user.Id,
                ProfileimageUrl= user.ProfileimageUrl,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Address = user.Address,
                UserRoles = _userManager.GetRolesAsync(user).Result.ToList()
            };
            return viewModel;
        }




    }
}
