using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.ViewModel;
using System.Security.Claims;
using Utilities;

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

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Summary")]

        public IActionResult SummaryPost(ShoppingCartViewModel shoppingCartViewModel)
        {
            double TotalPrice = 0;
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var cart = new ShoppingCartViewModel()
            {
                ShoppingCartItems = _unitOfWork.shopingCart.GetAll(
                    s => s.ApplicationUserId == claim.Value,
                    includePropererities: "product"
                    ),

				OrderHeader = new OrderHeader() { }
			};
            cart.OrderHeader.PaymentStatus = StaticDetails.PaymentStautsPending;
            cart.OrderHeader.OrderStatus = StaticDetails.StautsPending;
            cart.OrderHeader.ApplicationUserId = claim.Value;
            cart.OrderHeader.OrderDate = DateTime.Now;
            cart.OrderHeader.Address = shoppingCartViewModel.OrderHeader.Address;
            cart.OrderHeader.PhoneNumber = shoppingCartViewModel.OrderHeader.PhoneNumber;
            cart.OrderHeader.Name = shoppingCartViewModel.OrderHeader.Name;


            

            foreach (var item in cart.ShoppingCartItems)
            {
                item.PriceBasedOnCount = _unitOfWork.shopingCart.GetPriceOfOrderBasedOnQuantity(item.Count, item.product.Price);
                TotalPrice += item.PriceBasedOnCount;
            }
            cart.OrderHeader.OrderTotal = TotalPrice;

            _unitOfWork.orderHeader.Add(cart.OrderHeader);
            _unitOfWork.Save();

            foreach (var item in cart.ShoppingCartItems)
            {
                OrderDetails orderDetails = new OrderDetails() {
                
                    ProductId= item.ProductId,
                    OrderId= cart.OrderHeader.Id,
                    Count= item.Count,
                    Price=item.PriceBasedOnCount
                };
                _unitOfWork.orderDtails.Add(orderDetails);
                _unitOfWork.Save();
            }
            _unitOfWork.shopingCart.RemoveRange(cart.ShoppingCartItems);
            _unitOfWork.Save();

            ViewData["success"] = "Order Submitted Successfully";
                return RedirectToAction(nameof(Index));

            
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
