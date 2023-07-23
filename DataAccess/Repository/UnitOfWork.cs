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
			this.company= new CompanyRepository(_context);
			this.shopingCart=new ShopingCartRepository(_context);
			this.applicationUser=new ApplicationUserRepository(_context);
			
		}

		public ICategoryRepository category { get; private set; }

		public ICoverTypeRepository coverType { get; private set; }
		public IProductRepository product { get; private set; }

        public ICompanyRepository company { get; private set; }
        public IShopingCartRepository shopingCart { get; private set; }
		public IApplicationUserRepository applicationUser { get; private set; }


        public void Save()
		{
			_context.SaveChanges();
			
		}
	}
}
