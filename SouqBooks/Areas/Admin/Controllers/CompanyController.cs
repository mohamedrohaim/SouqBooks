using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models;
using SouqBooks.DataAccess.Data;

namespace SouqBooks.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Employee")]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        
        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {

            IEnumerable<Company> companies = _unitOfWork.company.GetAll();

            return View(companies);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Company company)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.company.Add(company);
                _unitOfWork.Save();
                TempData["success"] = "Company created successfully";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(company);
            }
        }

        public IActionResult Edit(int? id)
        {
            if (id != null)
            {
                // var Company= _context.categories.FirstOrDefault(c => c.Id == id);
                var Company = _unitOfWork.company.GetFirstOrDefault(c => c.Id == id);
                if (Company != null)
                {
                    return View(Company);
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
        public IActionResult Edit(Company company)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.company.Update(company);
                _unitOfWork.Save();
                TempData["success"] = "Company updated successfully";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(company);
            }
        }


        public IActionResult Delete(int id)
        {
            if (id != null)
            {
                var Company = _unitOfWork.company.GetFirstOrDefault(c => c.Id == id);
                if (Company != null)
                {
                    return View(Company);
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
        public IActionResult Delete(Company company)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.company.Delete(company);
                _unitOfWork.Save();
                TempData["success"] = "Company deleted successfully";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(company);
            }
        }

    }
}
