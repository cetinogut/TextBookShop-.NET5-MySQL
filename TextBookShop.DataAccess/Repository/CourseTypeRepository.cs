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
    public class CourseTypeRepository : Repository<CourseType>, ICourseTypeRepository
    {
        private readonly ApplicationDbContext _db;

        public CourseTypeRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(CourseType courseType)
        {
            var objFromDb = _db.CourseTypes.FirstOrDefault(s => s.Id == courseType.Id);
            if (objFromDb != null)
            {
                objFromDb.Name = courseType.Name;
               
            }
        }
    }
}
