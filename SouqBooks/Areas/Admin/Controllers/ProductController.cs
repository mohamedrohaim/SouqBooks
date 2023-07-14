
using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models;
using Models.ViewModel;

namespace SouqBooks.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork unitOfWork,IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
      

        public IActionResult Index()
        {

            IEnumerable<Product> products = _unitOfWork.product.GetAll();
            return View(products);
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

            if (id == null || id == 0)
            {
                //create product
                ViewBag.categoryList = productVM.CatecoryList;
                ViewBag.coverType = productVM.CoverTypeList;
                return View(productVM);
            }
            else
            {
                //Update product
               
                return View(productVM);
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductViewModel productViewModel, IFormFile? file) {
            if (ModelState.IsValid) {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null) { 
                 string fileName=Guid.NewGuid().ToString();
                    var uploads =Path.Combine(wwwRootPath, @"Images\Products");
                    var extention = Path.GetExtension(file.FileName);
                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extention), FileMode.Create)) {
                     file.CopyTo(fileStreams);
                    }
                    productViewModel.product.ImageUrl = @"\Images\Products\" + fileName + extention;



				}
                _unitOfWork.product.Add(productViewModel.product);
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));

            }
            return View(productViewModel);
        }

        public IActionResult Edit(int? id)
        {
            if (id != null)
            {
                var product = _unitOfWork.product.GetFirstOrDefault(c => c.Id == id);
                if (product != null)
                {
                    return View(product);
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
        public IActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.product.Update(product);
                _unitOfWork.Save();
                TempData["success"] = "product updated successfully";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(product);
            }
        }


        public IActionResult Delete(int id)
        {
            if (id != null)
            {
                var product = _unitOfWork.product.GetFirstOrDefault(c => c.Id == id);
                if (product != null)
                {
                    return View(product);
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
        public IActionResult Delete(Product product)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.product.Delete(product);
                _unitOfWork.Save();
                TempData["success"] = "product deleted successfully";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(product);
            }
        }

    }
}
