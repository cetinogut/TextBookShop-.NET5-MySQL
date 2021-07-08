using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TextBookShop.DataAccess.Repository.IRepository;
using TextBookShop.Models;
using TextBookShop.Utilities;

namespace TextBookShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CourseTypeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CourseTypeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
           
        }

        public IActionResult Index()
        {
            return View();
        }

        //public async Task<IActionResult> Index(int productPage = 1)
        //{
        //    CourseTypeVM courseTypeVM = new CourseTypeVM()
        //    {
        //        Categories = await _unitOfWork.CourseType.GetAllAsync()
        //    };

        //    var count = courseTypeVM.Categories.Count();
        //    courseTypeVM.Categories = courseTypeVM.Categories.OrderBy(p => p.Name)
        //        .Skip((productPage - 1) * 2).Take(2).ToList();

        //    courseTypeVM.PagingInfo = new PagingInfo
        //    {
        //        CurrentPage = productPage,
        //        ItemsPerPage = 2,
        //        TotalItem = count,
        //        urlParam = "/Admin/CourseType/Index?productPage=:"
        //    };

        //    return View(courseTypeVM);
        //}

        public IActionResult Upsert(int? id)
        {
            CourseType courseType = new CourseType();
            if (id == null)
            {
                //this is for create
                return View(courseType);
            }
            //this is for edit
            courseType = _unitOfWork.CourseType.Get(id.GetValueOrDefault());
            if (courseType == null)
            {
                return NotFound();
            }
            return View(courseType);

         }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(CourseType courseType)
        {
            if (ModelState.IsValid)
            {
                if (courseType.Id == 0)
                {
                     _unitOfWork.CourseType.Add(courseType);

                }
                else
                {
                    _unitOfWork.CourseType.Update(courseType);
                }
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index)); // name of index action
            }
            return View(courseType);
        }


        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            var allObj =  _unitOfWork.CourseType.GetAll();
            return Json(new { data = allObj });
        }

        [HttpDelete]
        public  IActionResult Delete(int id)
        {
            var objFromDb =  _unitOfWork.CourseType.Get(id);
            if (objFromDb == null)
            {
                TempData["Error"] = "Sınav Tipi Silinirken Hata...";
                return Json(new { success = false, message = "silme işlemi sırasında hata oluştu" });
            }

             _unitOfWork.CourseType.Remove(objFromDb);
            _unitOfWork.Save();

            TempData["Success"] = "Sınav Tipi başarıyla silindi";
            return Json(new { success = true, message = "Silme işlemi başarılı..." });

        }

        #endregion
    }
}
