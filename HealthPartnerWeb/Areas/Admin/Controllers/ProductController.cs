using HealthPartner.Data;
using HealthPartner.Data.Repository.IRepository;
using HealthPartner.Model;
using HealthPartner.Model.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HealthPartnerWeb.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _UnitOfWork;
        private readonly IWebHostEnvironment _HostEnvironment;
        public ProductController(IUnitOfWork UnitOfWork, IWebHostEnvironment HostEnvironment)
        {
            _UnitOfWork = UnitOfWork;
            _HostEnvironment = HostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }
        
        //GET
        public IActionResult Upsert(int? id)
        {
            ProductVM ProductVM = new()
            {
                Product = new(),
                CategoryList = _UnitOfWork.Category.GetAll().Select(i =>new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),                                
                CoverTypeList = _UnitOfWork.CoverType.GetAll().Select(i=>new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                })
            };
            if (id == null || id == 0)
            {
                //Create
                return View(ProductVM);
            }
            else
            {
                ProductVM.Product = _UnitOfWork.Product.GetFirstOrDefault(i => i.Id == id);
                return View(ProductVM);
                //update
            }
            
            return View();
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM obj,IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _HostEnvironment.WebRootPath;
                if (file != null)
                { 
                    string fileName=Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwRootPath, @"images\Product");
                    var extension = Path.GetExtension(file.FileName);
                    if(obj.Product.ImageUrl != null)
                    {
                        var OldImagePath = Path.Combine(wwwRootPath, obj.Product.ImageUrl.TrimStart('\\'));
                        if(System.IO.File.Exists(OldImagePath))
                        {
                            System.IO.File.Delete(OldImagePath);
                        }
                    }
                    using (var filestream= new FileStream(Path.Combine(uploads,fileName+extension),FileMode.Create))
                    {
                        file.CopyTo(filestream);
                    }
                    obj.Product.ImageUrl = @"\images\Product\" + fileName + extension;
                }
                if (obj.Product.Id == 0)
                {
                  _UnitOfWork.Product.Add(obj.Product);
                    TempData["Success"] = "Category Created successfully";
                }
                else
                {
                    _UnitOfWork.Product.Update(obj.Product);
                    TempData["Success"] = "Category updated successfully";
                }
                
                _UnitOfWork.Save();
                
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        #region API CALLS
        [HttpGet]
        public IActionResult Getall()
        {
            var ProductList = _UnitOfWork.Product.GetAll(includeProperties:"Category,CoverType");
            return Json(new { data = ProductList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var obj = _UnitOfWork.Product.GetFirstOrDefault(u => u.Id == id);
            if (obj == null)
            {
                return Json(new {success=false ,message="Error while deleting"});
            }
            var OldImagePath = Path.Combine(_HostEnvironment.WebRootPath, obj.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(OldImagePath))
            {
                System.IO.File.Delete(OldImagePath);
            }
            _UnitOfWork.Product.Remove(obj);
            _UnitOfWork.Save();
            return Json(new { success = true, message = "Product deleted successfully" });

        }
        #endregion
    }


}
