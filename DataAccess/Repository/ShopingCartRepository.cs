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
            if (shoppingCart.Count <= 500)
            {
                shoppingCart.Count += count;
            }

           return shoppingCart.Count;

        }

        public void Update(ShoppingCart shopingCart)
		{
			_context.Update(shopingCart);
		}

       

        public double GetPriceOfOrderBasedOnQuantity(int quantity, double price)
        {
            return quantity*price;
        }

        public void RemoveRange(IEnumerable<ShoppingCart> carts)
        {
           _context.RemoveRange(carts);
        }
    }
}
