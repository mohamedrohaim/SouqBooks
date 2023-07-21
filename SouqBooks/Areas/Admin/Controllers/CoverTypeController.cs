using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using SouqBooks.DataAccess.Data;

namespace SouqBooks.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class CoverTypeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CoverTypeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {

            IEnumerable<CoverType> coverTypes = _unitOfWork.coverType.GetAll();
            return View(coverTypes);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CoverType coverType)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.coverType.Add(coverType);
                _unitOfWork.Save();
                TempData["success"] = "cover type created successfully";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(coverType);
            }
        }

        public IActionResult Edit(int? id)
        {
            if (id != null)
            {
                var coverType = _unitOfWork.coverType.GetFirstOrDefault(c => c.Id == id);
                if (coverType != null)
                {
                    return View(coverType);
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
        public IActionResult Edit(CoverType coverType)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.coverType.Update(coverType);
                _unitOfWork.Save();
                TempData["success"] = "cover type updated successfully";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(coverType);
            }
        }


        public IActionResult Delete(int id)
        {
            if (id != null)
            {
                var coverType = _unitOfWork.coverType.GetFirstOrDefault(c => c.Id == id);
                if (coverType != null)
                {
                    return View(coverType);
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
        public IActionResult Delete(CoverType coverType)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.coverType.Delete(coverType);
                _unitOfWork.Save();
                TempData["success"] = "cover type deleted successfully";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(coverType);
            }
        }

    }
}
