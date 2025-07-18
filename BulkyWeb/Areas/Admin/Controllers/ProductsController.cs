﻿using BulkyWeb.Data;
using BulkyWeb.Models;
using BulkyWeb.Models.ViewModel;
using BulkyWeb.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductsController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            List<Products> objProductList = _unitOfWork.Products.GetAll().ToList();
            IEnumerable<SelectListItem> CategoryList = _unitOfWork.Category.
                GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()

                });
            return View(objProductList);
        }
        public IActionResult Upsert(int? id)
        {
            ProductVM productVM = new()
            {
                CategoryList = _unitOfWork.Category.GetAll().
                Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                Products = new Products()
            };
            if (id == null || id == 0)
            {
                //create
                return View(productVM);
            }
            else
            {
                //update
                productVM.Products = _unitOfWork.Products.Get(u => u.Id == id);
                return View(productVM);
            }
            return View(productVM);
        }
        [HttpPost]
        public IActionResult Upsert(ProductVM productVM, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"images\products");

                    if (!string.IsNullOrEmpty(productVM.Products.ImageUrl))
                    {
                        //delete old image
                        var oldImagePath = Path.Combine(wwwRootPath, productVM.Products.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    productVM.Products.ImageUrl = @"\images\products\" + fileName;

                }
                if (productVM.Products.Id == 0)
                {
                    _unitOfWork.Products.Add(productVM.Products);
                }
                else
                {
                    _unitOfWork.Products.Update(productVM.Products);
                }

                _unitOfWork.Save();
                TempData["success"] = "Product created successfully";
                return RedirectToAction("Index");
            }
            else
            {

                productVM.CategoryList = _unitOfWork.Category.GetAll().
            Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });
                return View(productVM);
            }
        }

     

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            List<Products> objProductList = _unitOfWork.Products.GetAll(includeProperties: "Category").ToList();
            return Json(new { data = objProductList });

        }
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var productToBeDeleted = _unitOfWork.Products.Get(u => u.Id== id);
            if(productToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, 
                productToBeDeleted.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }
            _unitOfWork.Products.Remove(productToBeDeleted);
            _unitOfWork.Save();
            
            return Json(new { success = true, message = "Product removed successfully" });

        }
        #endregion


    }
    }
