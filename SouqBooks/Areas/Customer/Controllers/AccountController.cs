using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.ViewModel;
using SouqBooks.DataAccess.Data;
using SouqBooks.Utilities;

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
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel,IFormFile file) {
            if (ModelState.IsValid)
            {
                registerViewModel.ProfileimageUrl = _imageUploader.UploadImage(file, "Users");
                var user=mappRegisterViewModelToApplicationUsers(registerViewModel);
                var result=await _userManager.CreateAsync(user,registerViewModel.Password);
                if (result.Succeeded)
                {
                    return View();
                }
                else {
                    foreach (var error in result.Errors) {
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



        private ApplicationUser mappRegisterViewModelToApplicationUsers(RegisterViewModel model) {
            var user = new ApplicationUser
            {
                Email = model.Email,
                UserName = model.Email.Split('@')[0].ToString(),
                FirstName = model.FirstName,
                LastName = model.LastName,
                Address = model.Address,
                ProfileimageUrl =model.ProfileimageUrl,
                IsAgree= model.IsAgree,

            };
            return user;
        }
    }
}
