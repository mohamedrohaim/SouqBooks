using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Models;
using SouqBooks.DataAccess.Data;

namespace SouqBooks.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _context;
        public CategoryController(ICategoryRepository context)
        {
            _context= context;
        }

        public IActionResult Index()
        {

            IEnumerable<Category> categories = _context.GetAll();
            return View(categories);
        }

        public IActionResult Create() {
        return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category) {
            if (ModelState.IsValid)
            {
                _context.Add(category);
                _context.Save();
                TempData["success"] = "category created successfully";
                return RedirectToAction(nameof(Index));
            }
            else { 
            return View(category);
            }
        }

        public IActionResult Edit(int? id)
        {
            if (id != null) {
                // var category= _context.categories.FirstOrDefault(c => c.Id == id);
                var category = _context.GetFirstOrDefault(c=>c.Id==id);
                if (category != null)
                {
                    return View(category);
                }
                else { 
                return NotFound();
                }
            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid )
            {
                _context.Update(category);
                _context.Save();
                TempData["success"] = "category updated successfully";
                return RedirectToAction(nameof(Index));
            }
            else { 
            return View(category);
            }
        }


        public IActionResult Delete(int id)
        {
            if (id != null)
            {
                var category = _context.GetFirstOrDefault(c => c.Id == id);
				if (category != null)
                {
                    return View(category);
                }
                else
                {
                    return NotFound();
                }
            }
            return NotFound();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Delete(category);
                _context.Save();
                TempData["success"] = "category deleted successfully";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(category);
            }
        }

    }
}
