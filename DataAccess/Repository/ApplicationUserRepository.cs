using DataAccess.Repository.IRepository;
using Models;
using SouqBooks.DataAccess.Data;


namespace DataAccess.Repository
{
	public class ApplicationUserRepository : Repository<ApplicationUser> , IApplicationUserRepository
    {
		private readonly ApplicationDbContext _context;
		public ApplicationUserRepository(ApplicationDbContext context):base(context)
		{
			_context = context;
		}

    }
}
