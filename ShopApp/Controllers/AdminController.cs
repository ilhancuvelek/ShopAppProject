using Business.Abstract;
using DataAccess.Abstract;
using Entity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ShopApp.Models;
using System.Linq;

namespace ShopApp.Controllers
{
    public class AdminController : Controller
    {
        private IProductService _productService;
        private ICategoryService _categoryService;
        public AdminController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }
        //  --- Product ---
        public IActionResult ProductList()
        {
            var productListViewModel = new ProductListViewModel()
            {
                Products = _productService.GetAll()
            };
            return View(productListViewModel);
        }
        [HttpGet]
        public IActionResult ProductCreate()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ProductCreate(ProductModel productModel)
        {
            if (ModelState.IsValid)
            {
                var entity = new Product()
                {
                    Name = productModel.Name,
                    Url = productModel.Url,
                    Price = productModel.Price,
                    Description = productModel.Description,
                    ImageUrl = productModel.ImageUrl
                };

                if (_productService.Create(entity)) // iş kuralı için create yi bool a çevirdik
                {
                    //bilgilendirme mesajı
                    CreateMessage("kayıt eklendi", "success");
                    //bilgilendirme mesajı -son-
                    return RedirectToAction("ProductList");
                }
                CreateMessage(_productService.ErrorMessage, "danger");
                return View(productModel);

            }
            return View(productModel);
        }
        [HttpGet]
        public IActionResult ProductEdit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entity = _productService.GetByIdWithCategories((int)id);

            if (entity == null)
            {
                return NotFound();
            }

            var model = new ProductModel()
            {
                ProductId = entity.ProductId,
                Name = entity.Name,
                Url = entity.Url,
                Price = entity.Price,
                ImageUrl = entity.ImageUrl,
                Description = entity.Description,
                SelectedCategories = entity.ProductsCategories.Select(p => p.Category).ToList()//bu seçilen ürünün kategorileri
            };
            ViewBag.Categories = _categoryService.GetAll();//bu tüm kategoriler
            return View(model);

        }
        [HttpPost]
        public IActionResult ProductEdit(ProductModel productModel, int[] categoryIds)
        {
            if (ModelState.IsValid)
            {
                var entity = _productService.GetById(productModel.ProductId);
                if (entity == null)
                {
                    return NotFound();
                }
                entity.Name = productModel.Name;
                entity.Price = productModel.Price;
                entity.Url = productModel.Url;
                entity.ImageUrl = productModel.ImageUrl;
                entity.Description = productModel.Description;

                _productService.Update(entity, categoryIds);

                //bilgilendirme mesajı
                CreateMessage("kayıt güncellendi", "success");
                //bilgilendirme mesajı -son-

                return RedirectToAction("ProductList");
            }
            ViewBag.Categories = _categoryService.GetAll();
            return View(productModel);


        }
        public IActionResult DeleteProduct(int productId)
        {
            var product = _productService.GetById(productId);
            if (product!=null)
            {
                _productService.Delete(product);
            }

            //bilgilendirme mesajı
            CreateMessage("kayıt silindi", "danger");
            //bilgilendirme mesajı -son-
            return RedirectToAction("ProductList");
        }

        //  --- Product SON ---

        // --- Category ---
        public IActionResult CategoryList()
        {
            var categoryListViewModel = new CategoryListViewModel()
            {
                Categories = _categoryService.GetAll()
            };
            return View(categoryListViewModel);
        }
        public IActionResult CategoryCreate()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CategoryCreate(CategoryModel categoryModel)
        {
            if (ModelState.IsValid)
            {
                var entity = new Category()
                {
                    Name = categoryModel.Name,
                    Url = categoryModel.Url
                };

                _categoryService.Create(entity);

                //bilgilendirme mesajı
                CreateMessage("kayıt eklendi", "success");
                //bilgilendirme mesajı -son-


                return RedirectToAction("CategoryList");
            }
            return View(categoryModel);
        }
        [HttpGet]
        public IActionResult CategoryEdit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entity = _categoryService.GetByIdWithProducts((int)id);

            if (entity == null)
            {
                return NotFound();
            }

            var model = new CategoryModel()
            {
                CategoryId = entity.CategoryId,
                Name = entity.Name,
                Url = entity.Url,
                Products = entity.ProductsCategories.Select(p => p.Product).ToList()
            };
            return View(model);

        }
        [HttpPost]
        public IActionResult CategoryEdit(CategoryModel categoryModel)
        {
            if (ModelState.IsValid)
            {
                var entity = _categoryService.GetById(categoryModel.CategoryId);
                if (entity == null)
                {
                    return NotFound();
                }
                entity.Name = categoryModel.Name;
                entity.Url = categoryModel.Url;

                _categoryService.Update(entity);

                //bilgilendirme mesajı
                CreateMessage("kayıt güncellendi", "success");
                //bilgilendirme mesajı -son-

                return RedirectToAction("CategoryList");
            }
            return View(categoryModel);
        }
        public IActionResult DeleteCategory(int categoryId)
        {
            var category = _categoryService.GetById(categoryId);
            if (category != null)
            {
                _categoryService.Delete(category);
            }

            //bilgilendirme mesajı
            CreateMessage("kayıt silindi", "danger");
            //bilgilendirme mesajı -son-
            return RedirectToAction("CategoryList");
        }
        [HttpPost]
        public IActionResult DeleteFromCategory(int productId,int categoryId)
        {
            _categoryService.DeleteFromCategory(productId, categoryId);
            return Redirect("/admin/categories/" + categoryId);
        }

        // --- Category SON ---

        //bilgilendirme mesajı
        private void CreateMessage(string message,string alerttype)
        {
            var msg = new AlertMessage
            {
                Message = message,
                AlertType = alerttype
            };
            TempData["message"] = JsonConvert.SerializeObject(msg);
        }
        //bilgilendirme mesajı -son-
    }
}

