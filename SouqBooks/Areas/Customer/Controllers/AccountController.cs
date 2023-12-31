﻿using DataAccess.Repository.IRepository;
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
        private ILogger<AccountController> _logger;


		public AccountController(
            IUnitOfWork unitOfWork,
            ImageUploader imageUploader,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<AccountController>logger)
        {
            _unitOfWork = unitOfWork;
            _imageUploader = imageUploader;
            _userManager = userManager;
            _signInManger = signInManager;
            _logger = logger;

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
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var confirmatioLink = Url.Action("ConfirmEmail", "Account", new { userId=user.Id,token=token }, Request.Scheme);
                    var email = new Email() {
                    Reciver=user.Email,
                    Subject="Confirm Email",
                    Body= "Click the link below to confirm your email address:\n" +
					$"<a href='{confirmatioLink}'>Confirm Email</a>"
					};
                    EmailSettings.SendEmail(email);
                    
                    return View(nameof(checkConfirmationEmail));
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
        public IActionResult checkConfirmationEmail()
        {
            return View();
        }

		public async Task<IActionResult> ConfirmEmail(string userId, string token)
		{
			
			var user = await _userManager.FindByIdAsync(userId);

			if (user == null)
			{
                return NotFound();
			}

			var result = await _userManager.ConfirmEmailAsync(user, token);

			if (result.Succeeded)
			{
				return View();
			}
			else
			{
                return BadRequest();
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
                        if (!user.EmailConfirmed) {
							ModelState.AddModelError("", "Please confirm your email address to log in.");
                            return View(model);
						}
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

        public IActionResult LoginPath()
        {
            TempData["error"] = "You Have To Login!..";
            return View(nameof(Login));
        }
        
        public IActionResult AccessDeniedPath()
        {
            TempData["error"] = "Access Denied Path You Dont Have Permitions!..";
            return View(nameof(Login));
        }


        public IActionResult ForgetPassword() {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendEmail(ForgetPasswordViewModel model) {
            if (ModelState.IsValid) {
                ApplicationUser user=await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var token=await _userManager.GeneratePasswordResetTokenAsync(user);
                    var resetPasswordLink = Url.Action("ResetPassword", "Account", new
                    {
                        Email= model.Email,
                        Token=token
                    },Request.Scheme);
                    var email = new Email() {
                    Subject="Reset Password",
                    Reciver= model.Email,
					Body = $"Hello {user.FirstName} {user.LastName},\n\nYou have requested to reset your password. Please click the link below to proceed:\n\n{resetPasswordLink}\n\nThank you!\nSouqBooks"

					};
                    EmailSettings.SendEmail(email);
                    return RedirectToAction(nameof(CheckYourEmai));

                }
                ModelState.AddModelError(string.Empty,"Email Not Found"); 
               
            }
             return View("ForgetPassword", model);
        }


        public IActionResult CheckYourEmai()
        {
            return View();
        }

        
        public IActionResult ResetPassword(string email,string token)
        {
            TempData["token"] = token;
            TempData["email"] = email;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            var token = TempData["token"] as string;
			var email=TempData["email"] as string;
            var user=await _userManager.FindByEmailAsync(email);
            if (ModelState.IsValid)
            {
                var result=await _userManager.ResetPasswordAsync(user, token, model.NewPassword);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Login));
                }
                else {
                foreach(var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }

        


    }
}

