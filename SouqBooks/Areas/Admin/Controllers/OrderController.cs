using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;
using Models;
using Models.ViewModel;
using System.Security.Claims;

namespace SouqBooks.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Employee")]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        

        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        // display orders
        public IActionResult Index()
        {
          
            IEnumerable<OrderHeader> orders= new List<OrderHeader>();
            orders = _unitOfWork.orderHeader.GetAll();

            return View(orders);
        }

        public IActionResult OrderDetails(int id) {

            OrderHeader order = _unitOfWork.orderHeader.GetFirstOrDefault(
               filter:o=>o.Id==id,

                includePropererities: "orderDetails"
                );
            foreach (var product in order.orderDetails) {
                product.Product = _unitOfWork.product.GetFirstOrDefault(
                    p=>p.Id==product.ProductId,
                    "category,coverType"
                    );
            }

            return View(order);
        
        }



    }
}
