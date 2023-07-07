using Microsoft.AspNetCore.Mvc;
using SouqBooks.Data;
using SouqBooks.Models;

namespace SouqBooks.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;
        public CategoryController(ApplicationDbContext context)
        {
            _context= context;
        }

        public IActionResult Index()
        {

            IEnumerable<Category> categories = _context.categories.ToList();
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
                _context.categories.Add(category);
                _context.SaveChanges();
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
                var category= _context.categories.FirstOrDefault(c => c.Id == id);
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
                _context.categories.Update(category);
                _context.SaveChanges();
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
                var category = _context.categories.FirstOrDefault(c => c.Id == id);
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
                _context.categories.Remove(category);
                _context.SaveChanges();
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
