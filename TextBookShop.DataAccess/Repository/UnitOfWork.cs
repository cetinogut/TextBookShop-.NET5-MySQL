using TextBookShop.DataAccess.Data;
using TextBookShop.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Text;
using TextBookShop.Models;

namespace TextBookShop.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Category = new CategoryRepository(_db);
            CourseType = new CourseTypeRepository(_db);
            Company = new CompanyRepository(_db);
            Product = new ProductRepository(_db);
            
            SP_Call = new SP_Call(_db);
            
        }

        public IApplicationUserRepository ApplicationUser { get; private set; }
        public ICategoryRepository Category { get; private set; }
        public ICompanyRepository Company { get; private set; }
        public IProductRepository Product { get; private set; }
        public ICourseTypeRepository CourseType { get; private set; }
        
        public ISP_Call SP_Call { get; private set; }

        public void Dispose()
        {
            _db.Dispose();
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
