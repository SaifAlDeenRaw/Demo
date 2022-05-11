using HealthPartner.Data;
using HealthPartner.Data.Repository.IRepository;
using HealthPartner.Model;
using Microsoft.AspNetCore.Mvc;

namespace HealthPartnerWeb.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _UnitOfWork;
        public CategoryController(IUnitOfWork UnitOfWork)
        {
            _UnitOfWork = UnitOfWork;
        }
        public IActionResult Index()
        {
            IEnumerable<Category> objCategoryList= _UnitOfWork.Category.GetAll();
            return View(objCategoryList);
        }

        //GET
        public IActionResult Create()
        {
            
            return View();
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
                {
                ModelState.AddModelError("Name", "Name and Display Order Cannot Exactly Match");
                }
            if (ModelState.IsValid)
            {
                _UnitOfWork.Category.Add(obj);
                _UnitOfWork.Save();
                TempData["Success"] = "Category created successfully";
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        //GET
        public IActionResult Edit(int? id)
        {
            if( id== null || id==0 )
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
        public IActionResult Edit(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("Name", "Name and Display Order Cannot Exactly Match");
            }
            if (ModelState.IsValid)
            {
                _UnitOfWork.Category.Update(obj);
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
