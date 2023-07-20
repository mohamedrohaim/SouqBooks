using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models;
using SouqBooks.Utilities;
using System.Diagnostics;

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
        public IActionResult Index()
        {
            var producs = _unitOfWork.product.GetAll(includePropererities: "category,coverType");

            return View(producs);
        }

        public IActionResult Details(int id)
        {
            ShoppingCart cart = new ShoppingCart() {
             product= _unitOfWork.product.GetFirstOrDefault(filter: p => p.Id == id, "category,coverType"),
             Count=1
            };
            if (cart.product != null)
            {
                return View(cart);
            }
            else { 
            return NotFound();
            }
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