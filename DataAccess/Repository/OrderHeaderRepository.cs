using DataAccess.Repository.IRepository;
using Models;
using Models.ViewModel;
using SouqBooks.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
	public class OrderHeaderRepository : Repository<OrderHeader> , IOrderHeaderRepository
    {
		private readonly ApplicationDbContext _context;
		public OrderHeaderRepository(ApplicationDbContext context):base(context)
		{
			_context = context;
		}

		public void Update(OrderHeader orderHeader)
		{
			_context.Update(orderHeader);
		}

        public void UpdateStatus(int id, string orderStatus, string? paymentStatus = null)
        {
           var orderFromDatabase= _context.orderHeaders.FirstOrDefault(o => o.Id == id);
			if (orderFromDatabase != null)
			{
				orderFromDatabase.OrderStatus= orderStatus;
				if (paymentStatus!= null) {
					orderFromDatabase.PaymentStatus= paymentStatus;
				}
				
			}
        }
    }
}
