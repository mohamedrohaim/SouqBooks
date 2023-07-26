using DataAccess.Repository.IRepository;
using Models;
using SouqBooks.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
	public class OrderDetailsRepository : Repository<OrderDetails> , IOrderDtailsRepository
    {
		private readonly ApplicationDbContext _context;
		public OrderDetailsRepository(ApplicationDbContext context):base(context)
		{
			_context = context;
		}

	

		public void Update(OrderDetails orderDetails)
		{
			_context.Update(orderDetails);
		}
	}
}
