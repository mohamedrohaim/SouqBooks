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
                return RedirectToAction(nameof(Index));
            }
            else { 
            return View(category);
            }
        }
    }
}
