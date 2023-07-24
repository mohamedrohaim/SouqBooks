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
	public class ShopingCartRepository : Repository<ShoppingCart>, IShopingCartRepository
    {
		private readonly ApplicationDbContext _context;
		public ShopingCartRepository(ApplicationDbContext context):base(context)
		{
			_context = context;
		}

        public int IncrementCount(ShoppingCart shoppingCart, int count)
        {
           return shoppingCart.Count+count;
        }

        public void Update(ShoppingCart shopingCart)
		{
			_context.Update(shopingCart);
		}
	}
}
