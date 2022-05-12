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
        public ProductController(IUnitOfWork UnitOfWork)
        {
            _UnitOfWork = UnitOfWork;
        }
        public IActionResult Index()
        {
            IEnumerable<Product> ProductList= _UnitOfWork.Product.GetAll();
            return View(ProductList);
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
                //update
            }
            
            return View();
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM obj,IFormFile file)
        {
            
            if (ModelState.IsValid)
            {
                //_UnitOfWork.Category.Update(obj);
                _UnitOfWork.Save();
                TempData["Success"] = "Category updated successfully";
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        //GET
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var Categoryfromdb = _UnitOfWork.Category.GetFirstOrDefault(u=>u.Id==id);

            if (Categoryfromdb == null)
            {
                return NotFound();
            }

            return View(Categoryfromdb);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var obj = _UnitOfWork.Category.GetFirstOrDefault(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
                _UnitOfWork.Category.Remove(obj);
                _UnitOfWork.Save();
                TempData["Success"] = "Category deleted successfully";
            return RedirectToAction("Index");
            
            
        }
    }
}
