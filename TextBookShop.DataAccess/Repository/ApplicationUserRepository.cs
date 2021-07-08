using TextBookShop.DataAccess.Data;
using TextBookShop.DataAccess.Repository.IRepository;
using TextBookShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TextBookShop.DataAccess.Data.Repository;

namespace TextBookShop.DataAccess.Repository
{
    public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
    {
        private readonly ApplicationDbContext _db;

        public ApplicationUserRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

       
    }
}
