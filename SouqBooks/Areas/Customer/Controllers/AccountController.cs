using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.ViewModel;
using SouqBooks.Utilities;
using Utilities;

namespace SouqBooks.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class AccountController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ImageUploader _imageUploader;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManger;

        public AccountController(
            IUnitOfWork unitOfWork,
            ImageUploader imageUploader,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _unitOfWork = unitOfWork;
            _imageUploader = imageUploader;
            _userManager = userManager;
            _signInManger = signInManager;

        }
        #region Register
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                registerViewModel.ProfileimageUrl = _imageUploader.UploadImage(file, "Users");
                var user = mappRegisterViewModelToApplicationUsers(registerViewModel);
                var result = await _userManager.CreateAsync(user, registerViewModel.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, StaticDetails.UserRole);
                    return View(nameof(Login));
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        TempData["error"] = error.Description;
                    }
                    return View(registerViewModel);
                }



            }
            else
            {
                return View(registerViewModel);
            }

        }



        private ApplicationUser mappRegisterViewModelToApplicationUsers(RegisterViewModel model)
        {
            var user = new ApplicationUser
            {
                Email = model.Email,
                UserName = model.Email.Split('@')[0].ToString(),
                FirstName = model.FirstName,
                LastName = model.LastName,
                Address = model.Address,
                ProfileimageUrl = model.ProfileimageUrl,
                IsAgree = model.IsAgree,

            };
            return user;
        }
        #endregion

        #region Login
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    bool flag = await _userManager.CheckPasswordAsync(user, model.Password);
                    if (flag)
                    {
                        var result = await _signInManger.PasswordSignInAsync(user, model.Password, model.IsAgree, false);
                        if (result.Succeeded)
                            return RedirectToAction("Index", "Home");
                        else
                            return View(model);

                    }
                    ModelState.AddModelError(string.Empty, "password is not correct");
                    return View(model);
                }
                TempData["error"] = "Email Is Not Existing";
                return View(model);
            }
            else
                return View(model);
        }
        #endregion

        #region SignOut

        public new async Task<IActionResult> Logout()
        {
            await _signInManger.SignOutAsync();

            return RedirectToAction(nameof(Login));

        }

        #endregion


    }
}

