using DataAccess.Repository.IRepository;
using Models;
using SouqBooks.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
	public class ApplicationUserRepository : Repository<Category> , IApplicationUserRepository
    {
		private readonly ApplicationDbContext _context;
		public ApplicationUserRepository(ApplicationDbContext context):base(context)
		{
			_context = context;
		}

        public void Add(ApplicationUser entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(ApplicationUser entity)
        {
            throw new NotImplementedException();
        }

        public ApplicationUser GetFirstOrDefault(Expression<Func<ApplicationUser, bool>> filter, string? includePropererities = null)
        {
            throw new NotImplementedException();
        }

        IEnumerable<ApplicationUser> IRepository<ApplicationUser>.GetAll(Expression<Func<ApplicationUser, bool>> filter,string? includePropererities)
        {
            throw new NotImplementedException();
        }
    }
}
