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
    [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
           
        }

        public IActionResult Index()
        {
            return View();
        }

        //public async Task<IActionResult> Index(int productPage = 1)
        //{
        //    CompanyVM companyVM = new CompanyVM()
        //    {
        //        Categories = await _unitOfWork.Company.GetAllAsync()
        //    };

        //    var count = companyVM.Categories.Count();
        //    companyVM.Categories = companyVM.Categories.OrderBy(p => p.Name)
        //        .Skip((productPage - 1) * 2).Take(2).ToList();

        //    companyVM.PagingInfo = new PagingInfo
        //    {
        //        CurrentPage = productPage,
        //        ItemsPerPage = 2,
        //        TotalItem = count,
        //        urlParam = "/Admin/Company/Index?productPage=:"
        //    };

        //    return View(companyVM);
        //}

        public IActionResult Upsert(int? id)
        {
            Company company = new Company();
            if (id == null)
            {
                //this is for create
                return View(company);
            }
            //this is for edit
            company = _unitOfWork.Company.Get(id.GetValueOrDefault());
            if (company == null)
            {
                return NotFound();
            }
            return View(company);

         }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Company company)
        {
            if (ModelState.IsValid)
            {
                if (company.Id == 0)
                {
                     _unitOfWork.Company.Add(company);

                }
                else
                {
                    _unitOfWork.Company.Update(company);
                }
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index)); // name of index action
            }
            return View(company);
        }


        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            var allObj =  _unitOfWork.Company.GetAll();
            return Json(new { data = allObj });
        }

        [HttpDelete]
        public  IActionResult Delete(int id)
        {
            var objFromDb =  _unitOfWork.Company.Get(id);
            if (objFromDb == null)
            {
                TempData["Error"] = "Distribütör Silinirken Hata...";
                return Json(new { success = false, message = "silme işlemi sırasında hata oluştu" });
            }

             _unitOfWork.Company.Remove(objFromDb);
            _unitOfWork.Save();

            TempData["Success"] = "Distribütör başarıyla silindi";
            return Json(new { success = true, message = "Silme işlemi başarılı..." });

        }

        #endregion
    }
}
