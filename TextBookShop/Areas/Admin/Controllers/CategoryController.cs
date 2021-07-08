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
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
           
        }

        public IActionResult Index()
        {
            return View();
        }

        //public async Task<IActionResult> Index(int productPage = 1)
        //{
        //    CategoryVM categoryVM = new CategoryVM()
        //    {
        //        Categories = await _unitOfWork.Category.GetAllAsync()
        //    };

        //    var count = categoryVM.Categories.Count();
        //    categoryVM.Categories = categoryVM.Categories.OrderBy(p => p.Name)
        //        .Skip((productPage - 1) * 2).Take(2).ToList();

        //    categoryVM.PagingInfo = new PagingInfo
        //    {
        //        CurrentPage = productPage,
        //        ItemsPerPage = 2,
        //        TotalItem = count,
        //        urlParam = "/Admin/Category/Index?productPage=:"
        //    };

        //    return View(categoryVM);
        //}

        public IActionResult Upsert(int? id)
        {
            Category category = new Category();
            if (id == null)
            {
                //this is for create
                return View(category);
            }
            //this is for edit
            category = _unitOfWork.Category.Get(id.GetValueOrDefault());
            if (category == null)
            {
                return NotFound();
            }
            return View(category);

         }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Category category)
        {
            if (ModelState.IsValid)
            {
                if (category.Id == 0)
                {
                     _unitOfWork.Category.Add(category);

                }
                else
                {
                    _unitOfWork.Category.Update(category);
                }
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index)); // name of index action
            }
            return View(category);
        }


        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            var allObj =  _unitOfWork.Category.GetAll();
            return Json(new { data = allObj });
        }

        [HttpDelete]
        public  IActionResult Delete(int id)
        {
            var objFromDb =  _unitOfWork.Category.Get(id);
            if (objFromDb == null)
            {
                TempData["Error"] = "Kategori Silinirken Hata...";
                return Json(new { success = false, message = "silme işlemi sırasında hata oluştu" });
            }

             _unitOfWork.Category.Remove(objFromDb);
            _unitOfWork.Save();

            TempData["Success"] = "Kategori başarıyla silindi";
            return Json(new { success = true, message = "Silme işlemi başarılı..." });

        }

        #endregion
    }
}
