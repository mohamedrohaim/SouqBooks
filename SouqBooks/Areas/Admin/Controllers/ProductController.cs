
using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models;
using Models.ViewModel;
using SouqBooks.Utilities;
using System.Security.Claims;

namespace SouqBooks.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Employee")]

    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private IWebHostEnvironment _webHostEnvironment;
		private readonly ImageUploader _imageUploader;

		public ProductController(IUnitOfWork unitOfWork,IWebHostEnvironment webHostEnvironment,ImageUploader imageUploader)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
            _imageUploader = imageUploader;
        }
      

        public IActionResult Index()
        {

          
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            ProductViewModel productVM = new() { 
             product=new(),
             CatecoryList= _unitOfWork.category.GetAll()
            .Select(
            p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = p.Name,
            }),
             CoverTypeList= _unitOfWork.coverType.GetAll()
            .Select(
            p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = p.Name,
            }),
            
        };
            //projection using select
            ViewBag.categoryList = productVM.CatecoryList;
            ViewBag.coverType = productVM.CoverTypeList;
            if (id == null || id == 0)
            {
                return View(productVM);
            }
            else
            {
                productVM.product = _unitOfWork.product.GetFirstOrDefault(p => p.Id==id);
                return View(productVM);
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ProductViewModel productViewModel, IFormFile? file) {
            if (ModelState.IsValid) {
               
                productViewModel.product.ImageUrl = _imageUploader.UploadImage(file,"Products");
                try
                {
					_unitOfWork.product.Add(productViewModel.product);
					_unitOfWork.Save();
				}
                catch(Exception error) {
					TempData["error"] = error.Message;
                    return View("Upsert",productViewModel);
				}

			}
            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ProductViewModel productViewModel,IFormFile? file)
        {
			if (ModelState.IsValid )
			{
               
                if (file != null ) {
                    _imageUploader.DeleteFile(productViewModel.product.ImageUrl);
					productViewModel.product.ImageUrl = _imageUploader.UploadImage(file, "Products");
                }
                try {
					_unitOfWork.product.Update(productViewModel.product);
					_unitOfWork.Save();
					TempData["success"] = "product updated successfully";
					return RedirectToAction(nameof(Index));
				}
				catch (Exception error)
				{
					TempData["error"] = error.Message;
					return View("Upsert",productViewModel);
				}
			}
			else
			{
				return View("Upsert",productViewModel);
			}
		}


        public IActionResult Delete(int id)
        {
            if (id != null)
            {
				var prductViewModel = new ProductViewModel()
                {
                    product = _unitOfWork.product.GetFirstOrDefault(filter:c => c.Id == id,includePropererities: "category,coverType"),
                    
                };
				ViewBag.categoryList = prductViewModel.CatecoryList;
				ViewBag.coverType = prductViewModel.CoverTypeList;
				if (prductViewModel.product != null)
                {
                    return View(prductViewModel);
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
        public IActionResult Delete(ProductViewModel productViewModel)
        {
            if (ModelState.IsValid)
            {
                try {
					_unitOfWork.product.Delete(productViewModel.product);
					_unitOfWork.Save();
					TempData["success"] = "product deleted successfully";
					return RedirectToAction(nameof(Index));
				}
				catch (Exception error)
				{
					TempData["error"] = error.Message;
					return View( productViewModel);
				}
			}
            else
            {
                return View(productViewModel);
            }
        }

        #region Api Calls

        [HttpGet]
        public IActionResult GetAll() {

            var producs = _unitOfWork.product.GetAll(includePropererities: "category,coverType");
            return Json(new { data=producs});
        }

      


		#endregion

	}

}
