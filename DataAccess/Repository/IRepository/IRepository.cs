using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.IRepository
{
	public interface IRepository<T> where T : class
	{
		T GetFirstOrDefault(Expression<Func<T,bool>>filter,string? includePropererities=null);
		IEnumerable<T> GetAll(string? includePropererities=null);
		void Add(T entity);
		void Delete(T entity);



		

	}
}
