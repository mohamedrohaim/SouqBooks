using DataAccess.Repository.IRepository;
using SouqBooks.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly ApplicationDbContext _context;
		public UnitOfWork(ApplicationDbContext context)
		{
			_context = context;
			this.category = new CategoryRepository(_context);
			this.coverType= new CoverTypeRepository(_context);
			this.product=new ProductRepository(_context);

		}

		public ICategoryRepository category { get; private set; }

		public ICoverTypeRepository coverType { get; private set; }
		public IProductRepository product { get; private set; }

		public void Save()
		{
			_context.SaveChanges();
			
		}
	}
}
