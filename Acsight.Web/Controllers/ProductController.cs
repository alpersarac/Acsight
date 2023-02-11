
using Acsight.DataAccess.Repository;
using Acsight.DataAccess.Repository.IRepository;
using Acsight.Models;
using Acsight.Web.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Acsight.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        

        IUnitOfWork _unitOfWork;
        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            var objProducts = await _unitOfWork.Product.GetAllAsync();
            if (objProducts == null)
            {
                TempData["error"] = "An error occured while getting the Products, please check API settings";
                return RedirectToAction("Index", "Home");
            }
            var Temp = await _unitOfWork.Category.GetAllAsync();
            SelectList categorySelectList = new SelectList(Temp, "Id", "Name");
            ViewData["CategoryList"] = categorySelectList;
            return View(objProducts);
        }
        //GET
        public async Task<IActionResult> Create()
        {
            var Temp = await _unitOfWork.Category.GetAllAsync();
            SelectList categorySelectList = new SelectList(Temp, "Id", "Name");

            ViewData["CategoryList"] = categorySelectList;
            return View();
        }
        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product obj)
        {   
            if (ModelState.IsValid)
            {
                var result = await _unitOfWork.Product.PostAsync(obj);
                if (result)
                    TempData["success"] = "Product has been created successfully";
                else
                    TempData["error"] = "An error occured while adding the Product, please check API settings";

                return RedirectToAction("Index");
            }
            return View(obj);

        }

        //GET
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var productTypeFromDb = await _unitOfWork.Product.GetAsync(id);

            if (productTypeFromDb == null)
            {
                return NotFound();
            }
            return View(productTypeFromDb);
        }
        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Product obj)
        {
            if (ModelState.IsValid)
            {
                var result = await _unitOfWork.Product.UpdateAsync(obj);
                if (result)
                    TempData["success"] = "Category has been updated successfully";
                else
                    TempData["error"] = "An error occured while updating the Category, please check API settings";


                return RedirectToAction("Index");
            }
            return View(obj);

        }

        //GET
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var productFromDb = await _unitOfWork.Product.GetAsync(id);

            if (productFromDb == null)
            {
                return NotFound();
            }

            return View(productFromDb);
        }
        //Post
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePOST(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var productFromDb = await _unitOfWork.Product.DeleteAsync(id);

            if (productFromDb == null)
            {
                TempData["error"] = "An error occured while updating the Category, please check API settings";
                return NotFound();
            }
            TempData["success"] = "Category has been deleted successfully";
            return RedirectToAction("Index");

        }
        [HttpPost]
        public async Task<IActionResult> UpdateProductDynamically(int CategoryId,int ProductId, string ProductName)
        {
            if (ProductName.Length>32)
            {
                TempData["error"] = "Product Name Length can't be longer than 20 character.";
            }
            else
            {
                var Product = await _unitOfWork.Product.UpdateProductDynamically(ProductId, CategoryId, ProductName);
            }
            
            
            return Json("");
        }
    }
}
