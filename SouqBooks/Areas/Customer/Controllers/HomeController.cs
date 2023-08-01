using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Models;
using Models.ViewModel;
using SouqBooks.Utilities;
using System.Diagnostics;
using System.Security.Claims;
using static NuGet.Packaging.PackagingConstants;

namespace SouqBooks.Areas.Customer.Controllers
{

    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;
       
      
        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
           
        }

        [HttpGet]
        public IActionResult Index(string? search)
        {
            IEnumerable<Product> products;
            if (search != null)
            {
                products = _unitOfWork.product.GetAll(
                    filter: p =>
                    p.Title.Contains(search) ||
                    p.Author.Contains(search) ||
                    p.category.Name.Contains(search) ||
                    p.ISBN.Contains(search) 
                    

                    );
            }
            var producs = _unitOfWork.product.GetAll(includePropererities: "category,coverType");

            return View(producs);
        }

       
        public IActionResult Details(int productId)
        {
            ShoppingCart cart = new ShoppingCart() {
             product= _unitOfWork.product.GetFirstOrDefault(filter: p => p.Id == productId,includePropererities:"category,coverType"),
             Count=1,
             ProductId=productId,
            };
            if (cart.product != null)
            {
                
                return View(cart);
            }
            else { 
            return NotFound();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Details(ShoppingCart shoppingCart) {
            var claimsIdentity=(ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            shoppingCart.ApplicationUserId = claim.Value;
            
            ShoppingCart shoppingCartFromDb = _unitOfWork.shopingCart.GetFirstOrDefault(
               filter: (s=>s.ApplicationUserId == claim.Value && s.ProductId==shoppingCart.ProductId)
                );
            if (!ModelState.IsValid) {
                ShoppingCart cart = new ShoppingCart()
                {
                    product = _unitOfWork.product.GetFirstOrDefault(filter: p => p.Id == shoppingCart.ProductId, includePropererities: "category,coverType"),
                    Count = 1,
                    ProductId = shoppingCart.ProductId
                };

                return View(cart);

            }

            if (shoppingCartFromDb == null)
            {
                return CreateShoppingCartItem(shoppingCart);
            }
            else {
                shoppingCartFromDb.Count = _unitOfWork.shopingCart.IncrementCount(shoppingCartFromDb,shoppingCart.Count);
                return UpdateShoppingCartItem(shoppingCartFromDb);
            }
           }

        public IActionResult CreateShoppingCartItem(ShoppingCart shoppingCart)
        {
            try
            {
                _unitOfWork.shopingCart.Add(shoppingCart);
                _unitOfWork.Save();
                TempData["success"] = "Added To Sopping Cart Successfully";
            }catch (Exception ex)
            {
                TempData["error"] = ex.Message;
            }
            return RedirectToAction(nameof(Index));
        }
           
        public IActionResult UpdateShoppingCartItem(ShoppingCart shoppingCart)
        {
          
            try
            {
                _unitOfWork.shopingCart.Update(shoppingCart);
                _unitOfWork.Save();
                TempData["success"] = "Shopping Cart Updated Successfully";
            }catch (Exception ex)
            {
                TempData["error"] = ex.Message;
            }
            return RedirectToAction(nameof(Index));
        }
               
            
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}