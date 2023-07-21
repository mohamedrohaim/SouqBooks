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
	public class CompanyRepository : Repository<Company> , ICompanyRepository
	{
		private readonly ApplicationDbContext _context;
		public CompanyRepository(ApplicationDbContext context):base(context)
		{
			_context = context;
		}

	

		public void Update(Company companyy)
		{
			_context.Update(companyy);
		}
	}
}
