
using Acsight.DataAccess.Repository.IRepository;
using Acsight.Models;
using Acsight.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Net;
using System.Net.Http.Json;
using System.Text;

namespace Acsight.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        IUnitOfWork _unitOfWork;
        
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var objCategories  = await _unitOfWork.Category.GetAllAsync();
            if (objCategories==null)
            {
                TempData["error"] = "An error occured while getting the Categories, please check API settings";
                return RedirectToAction("Index","Home");
            }
            return View(objCategories);
        }
        //GET
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Category obj)
        {
            if (ModelState.IsValid)
            {
                var result = await _unitOfWork.Category.PostAsync(obj);
                if (result)
                    TempData["success"] = "Category has been created successfully";
                else
                    TempData["error"] = "An error occured while adding the Category, please check API settings";

                return RedirectToAction("Index");
            }
            return View(obj);
        }
        //Post


        //GET
        public async Task<IActionResult>  Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var categoryFromDb = await _unitOfWork.Category.GetAsync(id);

            if (categoryFromDb == null)
            {
                return NotFound();
            }

            return View(categoryFromDb);
        }
        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Category obj)
        {

            if (ModelState.IsValid)
            {
                var result = await _unitOfWork.Category.UpdateAsync(obj);
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

            var categoryFromDb = await _unitOfWork.Category.GetAsync(id);

            if (categoryFromDb == null)
            {
                return NotFound();
            }

            return View(categoryFromDb);
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

            var categoryFromDb = await _unitOfWork.Category.DeleteAsync(id);

            if (categoryFromDb == null)
            {
                TempData["error"] = "An error occured while updating the Category, please check API settings";
                return NotFound();
            }
            TempData["success"] = "Category has been deleted successfully";
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> List(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var categoryFromDb = await _unitOfWork.Category.GetAsync(id);

            if (categoryFromDb == null)
            {
                return NotFound();
            }
            ViewBag.ProductList = await _unitOfWork.Category.ProductListAsync(id);

            return View(categoryFromDb);
        }
    }
}
