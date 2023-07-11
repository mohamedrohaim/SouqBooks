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
	public class CoverTypeRepository : Repository<CoverType>, ICoverTypeRepository
	{
		private readonly ApplicationDbContext _context;

		public CoverTypeRepository(ApplicationDbContext context):base(context)
		{
			_context = context;
		}

		public void Update(CoverType coverType)
		{
			_context.coverTypes.Update(coverType);
		}
	}
}
