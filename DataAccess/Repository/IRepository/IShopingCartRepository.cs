using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.IRepository
{
	public interface IShopingCartRepository : IRepository<ShopingCart>
	{
		void Update(ShopingCart category);
	}
}
