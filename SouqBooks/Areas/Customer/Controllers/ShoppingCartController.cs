using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.ViewModel;
using System.Security.Claims;

namespace SouqBooks.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class ShoppingCartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ShoppingCartViewModel shoppingCartViewModel { get; set; }
        public ShoppingCartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork= unitOfWork;
        }


        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            shoppingCartViewModel = new ShoppingCartViewModel() {
                ShoppingCartItems = _unitOfWork.shopingCart.GetAll(
                    s=>s.ApplicationUserId==claim.Value,
                    includePropererities:"product"
                    ),
                OrderHeader=new OrderHeader(){ }
            };
            double TotalPrice = 0;
            foreach (var item in shoppingCartViewModel.ShoppingCartItems) {
                item.PriceBasedOnCount = _unitOfWork.shopingCart.GetPriceOfOrderBasedOnQuantity(item.Count, item.product.Price);
                TotalPrice += item.PriceBasedOnCount;
            }
            shoppingCartViewModel.OrderHeader.OrderTotal= TotalPrice;
            
            return View(shoppingCartViewModel);
        }
        public IActionResult Summary()
        {
            double TotalPrice = 0;
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            shoppingCartViewModel = new ShoppingCartViewModel()
            {
                ShoppingCartItems = _unitOfWork.shopingCart.GetAll(
                    s => s.ApplicationUserId == claim.Value,
                    includePropererities: "product"
                    ),
                OrderHeader = new OrderHeader() { }
            };

            var user = _unitOfWork.applicationUser.GetFirstOrDefault(
                o=>o.Id==claim.Value
                );
            shoppingCartViewModel.OrderHeader.ApplicationUser = user;
            shoppingCartViewModel.OrderHeader.Name = $"{user.FirstName} {user.LastName}";
            shoppingCartViewModel.OrderHeader.Address = user.Address;
            shoppingCartViewModel.OrderHeader.PhoneNumber = user.PhoneNumber;
                


            foreach (var item in shoppingCartViewModel.ShoppingCartItems)
            {
                item.PriceBasedOnCount = _unitOfWork.shopingCart.GetPriceOfOrderBasedOnQuantity(item.Count, item.product.Price);
                TotalPrice += item.PriceBasedOnCount;
            }
            shoppingCartViewModel.OrderHeader.OrderTotal = TotalPrice;

            return View(shoppingCartViewModel);

            return View();
        }

        public IActionResult incrementOrderCount(int CartId) {
            var cart=_unitOfWork.shopingCart.GetFirstOrDefault(c=>c.Id==CartId);
            if (cart != null && cart.Count <500)
            {
                cart.Count++;
                _unitOfWork.shopingCart.Update(cart);
                _unitOfWork.Save();
                ViewData["success"] = $"count Updated Successfully";

            }
            else {
                ViewData["error"] = $"count Not Updated ";
            }
            return RedirectToAction(nameof(Index));


        }
        public IActionResult decrementOrderCount(int CartId) {
            var cart=_unitOfWork.shopingCart.GetFirstOrDefault(c=>c.Id==CartId);
            if (cart != null && cart.Count>1)
            {
                cart.Count--;
                _unitOfWork.shopingCart.Update(cart);
                _unitOfWork.Save();
                ViewData["success"] = $"count Updated Successfully";

            }
            else {
                ViewData["error"] = $"count Not Updated ";
            }
            return RedirectToAction(nameof(Index));


        }





    }
}
