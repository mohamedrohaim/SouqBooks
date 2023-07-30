using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.ViewModel;
using Stripe.Checkout;
using System.Reflection.Metadata.Ecma335;
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
                OrderDetails orderDetails = new OrderDetails()
                {

                    ProductId = item.ProductId,
                    OrderId = cart.OrderHeader.Id,
                    Count = item.Count,
                    Price = item.PriceBasedOnCount
                };
                _unitOfWork.orderDtails.Add(orderDetails);
                _unitOfWork.Save();
            }
            _unitOfWork.shopingCart.RemoveRange(cart.ShoppingCartItems);
            _unitOfWork.Save();

            var domain = "https://localhost:44333/";
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> {
                "card",
                },
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                SuccessUrl = domain+$"Customer/ShoppingCart/orderConfirmation?id={cart.OrderHeader.Id}",
                CancelUrl = domain+$"Customer/ShoppingCart/Index",
			};

            foreach (var item in cart.ShoppingCartItems)
            {
                var settionItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions()
                    {

                        UnitAmount = (long)(item.product.Price*100),
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.product.Title,
                        },

                    },
                    Quantity = item.Count,


                };

                options.LineItems.Add(settionItem);

            }
            var servise = new SessionService();
            Session session = servise.Create(options);
            _unitOfWork.orderHeader.UpdateStripePayment(cart.OrderHeader.Id,session.Id,session.PaymentIntentId);
            _unitOfWork.Save();
            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);

        }
        public IActionResult orderConfirmation(int id)
        {
            OrderHeader orderHeader=_unitOfWork.orderHeader.GetFirstOrDefault(x => x.Id == id);
			orderHeader.PaymentDueDate = DateTime.Now.AddDays(5);
			var servise = new SessionService();
            Session session = servise.Get(orderHeader.SessionId);

            //stripe status
            if (session.PaymentStatus.ToLower() == "paid")
            {
                _unitOfWork.orderHeader.UpdateStatus(id, StaticDetails.StautsPending, session.PaymentIntentId, StaticDetails.PaymentStatusaApproved);
            }
            else {
				_unitOfWork.orderHeader.UpdateStatus(id, StaticDetails.StautsPending, session.PaymentIntentId, StaticDetails.PaymentStautsPending);
				
			}
            
            _unitOfWork.Save();
            List<ShoppingCart> shoppingCarts=_unitOfWork.shopingCart.GetAll(
                u=>u.ApplicationUserId==orderHeader.ApplicationUserId.ToString()
                ).ToList();
            _unitOfWork.shopingCart.RemoveRange(shoppingCarts);
			return View(orderHeader);
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
