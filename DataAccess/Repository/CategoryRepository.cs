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
	public class CategoryRepository : Repository<Category> , ICategoryRepository
	{
		private readonly ApplicationDbContext _context;
		public CategoryRepository(ApplicationDbContext context):base(context)
		{
			_context = context;
		}

		public void Save()
		{
			_context.SaveChanges();	
		}

		public void Update(Category category)
		{
			_context.Update(category);
		}
	}
}
