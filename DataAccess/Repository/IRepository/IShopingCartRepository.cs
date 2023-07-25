using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.IRepository
{
	public interface IShopingCartRepository : IRepository<ShoppingCart>
	{
		void Update(ShoppingCart category);
		int IncrementCount(ShoppingCart shoppingCart, int count);

		double GetPriceOfOrderBasedOnQuantity(int quantity, double price);
	}
}
