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
	public class ProductRepository : Repository<Product>, IProductRepository
	{
		private readonly ApplicationDbContext _context;

		public ProductRepository(ApplicationDbContext context):base(context)
		{
			_context = context;
		}

		public void Update(Product product)
		{
			var dbProduct = _context.products.FirstOrDefault(p => p.Id == product.Id);
			if (dbProduct != null) { 
				dbProduct.Title= product.Title;
				dbProduct.Description= product.Description;
				dbProduct.Author= product.Author;
				dbProduct.ISBN= product.ISBN;
				dbProduct.CategoryId= product.CategoryId;
				dbProduct.CoverTypeId= product.CoverTypeId;
				dbProduct.Price= product.Price;
				if (product.ImageUrl != null)
				{
					dbProduct.ImageUrl = product.ImageUrl;

				}
			}
		}
	}
}
