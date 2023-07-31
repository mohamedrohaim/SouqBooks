using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;
using Models;
using Models.ViewModel;
using System.Security.Claims;
using Utilities;

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
        public IActionResult Index(string status)
        {
          
            IEnumerable<OrderHeader> orders= new List<OrderHeader>();
            if (string.IsNullOrEmpty(status) || status == "All")
            {
                orders = _unitOfWork.orderHeader.GetAll();

            }
            else
            {
                orders = _unitOfWork.orderHeader.GetAll(
                    filter:o=>o.OrderStatus==status
                    );
            }

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

		public IActionResult UpdateOrderStatus(int id,string status)
		{
            var order = GetOrderData(id);
            order.OrderStatus = status;
            _unitOfWork.orderHeader.Update(order);
            _unitOfWork.Save();
			TempData["success"] = $"order status is {order.OrderStatus}";
			return RedirectToAction("OrderDetails", new { id = id });
		}



        private OrderHeader GetOrderData(int id) {
           
            OrderHeader order = _unitOfWork.orderHeader.GetFirstOrDefault(
			   filter: o => o.Id == id,

				includePropererities: "orderDetails"
				);
			foreach (var product in order.orderDetails)
			{
				product.Product = _unitOfWork.product.GetFirstOrDefault(
					p => p.Id == product.ProductId,
					"category,coverType"
					);
			}

            return order;

		}



	}
}
