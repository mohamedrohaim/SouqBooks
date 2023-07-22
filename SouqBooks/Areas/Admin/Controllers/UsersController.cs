using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.ViewModel;
using System.Collections.Generic;

namespace SouqBooks.Areas.Admin.Controllers
{
    [Area("Admin")]
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



        private IEnumerable<UserViewModel> MapApplicationUsersToUserViewModels(IEnumerable<ApplicationUser> applicationUser)
        {
            var viewModels = new List<UserViewModel>();


            foreach (var user in applicationUser)
            {
                var viewModel = new UserViewModel
                {
                    Id= user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Address= user.Address,
                    UserRoles = _userManager.GetRolesAsync(user).Result.ToList()
                };

                viewModels.Add(viewModel);
            }

            return viewModels;
        }




    }
}
