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
		public IActionResult SummaryPost(ShoppingCartViewModel shoppingCartViewModel, string payment)
		{
			double totalOrderPrice = 0;
			var claimsIdentity = (ClaimsIdentity)User.Identity;
			var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

			var orderHeader = new OrderHeader
			{
				PaymentStatus = payment == "payNow" ? StaticDetails.PaymentStautsPending : StaticDetails.PaymentStatusPayOnReciving,
				OrderStatus = StaticDetails.StatusaInProgress,
				ApplicationUserId = claim.Value,
				OrderDate = DateTime.Now,
				Address = shoppingCartViewModel.OrderHeader.Address,
				PhoneNumber = shoppingCartViewModel.OrderHeader.PhoneNumber,
				Name = shoppingCartViewModel.OrderHeader.Name
			};

			var shoppingCartItems = _unitOfWork.shopingCart.GetAll(
				s => s.ApplicationUserId == claim.Value,
				includePropererities: "product"
			);

			foreach (var item in shoppingCartItems)
			{
				item.PriceBasedOnCount = _unitOfWork.shopingCart.GetPriceOfOrderBasedOnQuantity(item.Count, item.product.Price);
				totalOrderPrice += item.PriceBasedOnCount;
			}
			orderHeader.OrderTotal = totalOrderPrice;

			_unitOfWork.orderHeader.Add(orderHeader);
			_unitOfWork.Save();

			foreach (var item in shoppingCartItems)
			{
				var orderDetails = new OrderDetails
				{
					ProductId = item.ProductId,
					OrderId = orderHeader.Id,
					Count = item.Count,
					Price = item.PriceBasedOnCount
				};
				_unitOfWork.orderDtails.Add(orderDetails);
				_unitOfWork.Save();
			}

			_unitOfWork.shopingCart.RemoveRange(shoppingCartItems);
			_unitOfWork.Save();

			if (payment == "payNow")
			{
				// Call the payment function
				ProcessPayment(orderHeader);
				return new StatusCodeResult(303);
			}

			return RedirectToAction("OrderConfirmation", "ShoppingCart", new { id = orderHeader.Id });
		}

		private void ProcessPayment(OrderHeader orderHeader)
		{
			var domain = "https://localhost:44333/";
			var options = new SessionCreateOptions
			{
				PaymentMethodTypes = new List<string> { "card" },
				LineItems = new List<SessionLineItemOptions>(),
				Mode = "payment",
				SuccessUrl = domain + $"Customer/ShoppingCart/orderConfirmation?id={orderHeader.Id}",
				CancelUrl = domain + $"Customer/ShoppingCart/Index",
			};

			foreach (var item in orderHeader.orderDetails)
			{
				var sessionItem = new SessionLineItemOptions
				{
					PriceData = new SessionLineItemPriceDataOptions()
					{
						UnitAmount = (long)(item.Price * 100),
						Currency = "usd",
						ProductData = new SessionLineItemPriceDataProductDataOptions
						{
							Name = item.Product.Title,
						},
					},
					Quantity = item.Count,
				};
				options.LineItems.Add(sessionItem);
			}

			var service = new SessionService();
			Session session = service.Create(options);

			_unitOfWork.orderHeader.UpdateStripePayment(orderHeader.Id, session.Id, session.PaymentIntentId);
			_unitOfWork.Save();

			Response.Headers.Add("Location", session.Url);
		}

		public IActionResult orderConfirmation(int id)
        {
            OrderHeader orderHeader=_unitOfWork.orderHeader.GetFirstOrDefault(x => x.Id == id);
            if (orderHeader.PaymentStatus != StaticDetails.PaymentStatusPayOnReciving)
            {
                var servise = new SessionService();
                Session session = servise.Get(orderHeader.SessionId);

                //stripe status
                if (session.PaymentStatus.ToLower() == "paid")
                {
                    _unitOfWork.orderHeader.UpdateStatus(id, StaticDetails.StatusaInProgress, session.PaymentIntentId, StaticDetails.PaymentStatusaApproved);
                }
                else
                {
                    _unitOfWork.orderHeader.UpdateStatus(id, StaticDetails.StatusaInProgress, session.PaymentIntentId, StaticDetails.PaymentStatusRejected);

                }
				_unitOfWork.Save();
			}
            
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

        public IActionResult DeleteCartItem(int CartId)
        {
            var cart = _unitOfWork.shopingCart.GetFirstOrDefault(c => c.Id == CartId);
            if (cart != null)
            {
                _unitOfWork.shopingCart.Delete(cart);
                _unitOfWork.Save();
                ViewData["success"] = "product deleted from cart";

            }
            else
            {
                ViewData["error"] = "product ot found";
            }
            return RedirectToAction(nameof(Index));

        }

        }
}
