﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.IRepository
{
	public interface IUnitOfWork
	{
		ICategoryRepository category { get; }
		ICoverTypeRepository coverType { get; }
		IProductRepository product { get; }
		void Save();
	}
}