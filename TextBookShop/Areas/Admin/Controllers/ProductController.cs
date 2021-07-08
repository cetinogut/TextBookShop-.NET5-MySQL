using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TextBookShop.DataAccess.Repository.IRepository;
using TextBookShop.Models;
using TextBookShop.Models.ViewModels;
using TextBookShop.Utilities;

namespace TextBookShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment; // added for image uploads

        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
           
        }

        public IActionResult Index()
        {
            return View(); // returning empty view because using JS to fill the datatable
        }

        //public async Task<IActionResult> Index(int productPage = 1)
        //{
        //    ProductVM productVM = new ProductVM()
        //    {
        //        Categories = await _unitOfWork.Product.GetAllAsync()
        //    };

        //    var count = productVM.Categories.Count();
        //    productVM.Categories = productVM.Categories.OrderBy(p => p.Name)
        //        .Skip((productPage - 1) * 2).Take(2).ToList();

        //    productVM.PagingInfo = new PagingInfo
        //    {
        //        CurrentPage = productPage,
        //        ItemsPerPage = 2,
        //        TotalItem = count,
        //        urlParam = "/Admin/Product/Index?productPage=:"
        //    };

        //    return View(productVM);
        //}

        public IActionResult Upsert(int? id)
        {
            IEnumerable<Category> CatList = _unitOfWork.Category.GetAll();
            ProductVM productVM = new ProductVM()
            {
                Product = new Product(),
                CategoryList = CatList.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
                CourseTypeList = _unitOfWork.CourseType.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                })
            };
            if (id == null)
            {
                //this is for create
                return View(productVM);
            }
            //this is for edit
            productVM.Product = _unitOfWork.Product.Get(id.GetValueOrDefault());
            if (productVM.Product == null)
            {
                return NotFound();
            }
            return View(productVM);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM productVM)
        {
            if (ModelState.IsValid)
            {
                string webRootPath = _hostEnvironment.WebRootPath;
                var files = HttpContext.Request.Form.Files;
                if (files.Count > 0) // image file(s) uploaded
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(webRootPath, @"images\products");
                    var extenstion = Path.GetExtension(files[0].FileName);

                    if (productVM.Product.ImageUrl != null)  //this is an edit and we need to remove old image
                    {
                        var imagePath = Path.Combine(webRootPath, productVM.Product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                        }
                    }
                    using (var filesStreams = new FileStream(Path.Combine(uploads, fileName + extenstion), FileMode.Create))
                    {
                        files[0].CopyTo(filesStreams);
                    }
                    productVM.Product.ImageUrl = @"\images\products\" + fileName + extenstion;
                }
                else
                {
                    //update and the user is not changing the image
                    if (productVM.Product.Id != 0)
                    {
                        Product objFromDb = _unitOfWork.Product.Get(productVM.Product.Id);
                        productVM.Product.ImageUrl = objFromDb.ImageUrl; // keep the image URL from the database same.
                    }
                }


                if (productVM.Product.Id == 0) // new product is to added
                {
                    _unitOfWork.Product.Add(productVM.Product);

                }
                else
                {
                    _unitOfWork.Product.Update(productVM.Product); // existing product is updated
                }
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            else  // model state is not valid
            {
                IEnumerable<Category> CatList = _unitOfWork.Category.GetAll();
                productVM.CategoryList = CatList.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });
                productVM.CourseTypeList = _unitOfWork.CourseType.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });
                if (productVM.Product.Id != 0) // this is an update, otherwise show empty formç
                {
                    productVM.Product = _unitOfWork.Product.Get(productVM.Product.Id);
                }
            }
            return View(productVM);
        }


        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            var allObj =  _unitOfWork.Product.GetAll(includeProperties: "Category,CourseType");
            return Json(new { data = allObj });
        }

        [HttpDelete]
        public  IActionResult Delete(int id)
        {
            var objFromDb =  _unitOfWork.Product.Get(id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Silerken hata oluştu!!!" });
            }
            string webRootPath = _hostEnvironment.WebRootPath;
            var imagePath = Path.Combine(webRootPath, objFromDb.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }
            _unitOfWork.Product.Remove(objFromDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Silme işlemi tamamlandı" });


        }

        #endregion
    }
}
