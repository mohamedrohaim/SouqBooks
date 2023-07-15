using DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using SouqBooks.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
	public class Repository<T> : IRepository<T> where T : class
	{
		private readonly ApplicationDbContext _context;
		internal DbSet<T> dbSet;

		public Repository(ApplicationDbContext context)
		{
			_context=context;
			this.dbSet=_context.Set<T>();

		}

		public void Add(T entity)
		{
			dbSet.Add(entity);
		}

		public void Delete(T entity)
		{
			dbSet.Remove(entity);
		}

		public IEnumerable<T> GetAll(string? includePropererities = null)
		{
			IQueryable<T> query = dbSet;
			if (includePropererities != null) {
				foreach (var includeProp in includePropererities.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)) { 
				query= query.Include(includeProp);
				}
			}
			return query.ToList();
		}

		public T GetFirstOrDefault(Expression<Func<T, bool>> filter, string? includePropererities = null)
		{
			IQueryable<T> query = dbSet;
            if (includePropererities != null)
            {
                foreach (var includeProp in includePropererities.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            query =query.Where(filter);

			return query.FirstOrDefault();
		}

		
	}
}
