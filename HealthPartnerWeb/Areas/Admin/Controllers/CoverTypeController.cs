using HealthPartner.Data;
using HealthPartner.Data.Repository.IRepository;
using HealthPartner.Model;
using Microsoft.AspNetCore.Mvc;

namespace HealthPartnerWeb.Controllers
{
    [Area("Admin")]
    public class CoverTypeController : Controller
    {
        private readonly IUnitOfWork _UnitOfWork;
        public CoverTypeController(IUnitOfWork UnitOfWork)
        {
            _UnitOfWork = UnitOfWork;
        }
        public IActionResult Index()
        {
            IEnumerable<CoverType> objCoverType = _UnitOfWork.CoverType.GetAll();
            return View(objCoverType);
        }

        //GET
        public IActionResult Create()
        {
            
            return View();
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CoverType obj)
        {
            if (ModelState.IsValid)
            {
                _UnitOfWork.CoverType.Add(obj);
                _UnitOfWork.Save();
                TempData["Success"] = "CoverType created successfully";
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
            var CoverTypefromDB = _UnitOfWork.CoverType.GetFirstOrDefault(u=>u.Id==id);
            
            if (CoverTypefromDB == null)
            {
                return NotFound();
            }
            
            return View(CoverTypefromDB);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CoverType obj)
        {
            if (ModelState.IsValid)
            {
                _UnitOfWork.CoverType.Update(obj);
                _UnitOfWork.Save();
                TempData["Success"] = "CoverType updated successfully";
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
            var CoverTypefromDB = _UnitOfWork.CoverType.GetFirstOrDefault(u=>u.Id==id);

            if (CoverTypefromDB == null)
            {
                return NotFound();
            }

            return View(CoverTypefromDB);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var obj = _UnitOfWork.CoverType.GetFirstOrDefault(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
                _UnitOfWork.CoverType.Remove(obj);
                _UnitOfWork.Save();
                TempData["Success"] = "Cover Type deleted successfully";
            return RedirectToAction("Index");
            
            
        }
    }
}
