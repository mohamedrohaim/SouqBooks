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
        public IActionResult Index(string status,string? search)
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
            if (search != null)
            {
                orders = _unitOfWork.orderHeader.GetAll(
                    filter:o=>
                    o.OrderStatus.Contains(search) ||
                    o.Name.Contains(search) ||
                    o.Address.Contains(search) ||
                    o.PhoneNumber.Contains(search)

                    );
            }

            int  Inprogress=0, InTransit = 0,cancelled = 0,completed = 0;
            double priceInprogress = 0, priceCompleted = 0, priceInTransmit = 0, priceCancelled = 0;
            foreach (var order in orders) {
                if (order.OrderStatus == StaticDetails.StatusaInProgress)
                {
                    Inprogress++;
					priceInprogress += order.OrderTotal;
                }
                else if (order.OrderStatus == StaticDetails.StatusInTransit) {
					InTransit++;
					priceInTransmit += order.OrderTotal;
                }else if (order.OrderStatus == StaticDetails.StatusCompleted)
                {
                    completed++;
                    priceCompleted += order.OrderTotal;
                }else if (order.OrderStatus==StaticDetails.StatusCanceled) {
                    cancelled++;
                    priceCancelled += order.OrderTotal;
                }
            }
            ViewBag.search = search;
            ViewBag.Inprogress = Inprogress;
            ViewBag.priceInprogress = priceInprogress;

            ViewBag.InTransit = InTransit;
            ViewBag.priceInTransmit = priceInTransmit;

            ViewBag.completed = completed;
            ViewBag.priceCompleted = priceCompleted;

			ViewBag.cancelled = cancelled;
			ViewBag.priceCancelled = priceCancelled;
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

		public IActionResult UpdateOrderStatus(int id,string status,string? carierName)
		{
            var order = GetOrderData(id);
            order.OrderStatus = status;
            if (status == StaticDetails.StatusInTransit && carierName != null) {
            order.Carrier=carierName;
            }
            _unitOfWork.orderHeader.Update(order);
            _unitOfWork.Save();
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
