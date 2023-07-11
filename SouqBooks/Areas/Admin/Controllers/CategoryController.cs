using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Models;
using SouqBooks.DataAccess.Data;

namespace SouqBooks.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {

            IEnumerable<Category> categories = _unitOfWork.category.GetAll();
            return View(categories);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.category.Add(category);
                _unitOfWork.Save();
                TempData["success"] = "category created successfully";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(category);
            }
        }

        public IActionResult Edit(int? id)
        {
            if (id != null)
            {
                // var category= _context.categories.FirstOrDefault(c => c.Id == id);
                var category = _unitOfWork.category.GetFirstOrDefault(c => c.Id == id);
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
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.category.Update(category);
                _unitOfWork.Save();
                TempData["success"] = "category updated successfully";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(category);
            }
        }


        public IActionResult Delete(int id)
        {
            if (id != null)
            {
                var category = _unitOfWork.category.GetFirstOrDefault(c => c.Id == id);
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
                _unitOfWork.category.Delete(category);
                _unitOfWork.Save();
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
