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
            var entity = new Product()
            {
                Price = productModel.Price,
                Name=productModel.Name,
                Description=productModel.Description,
                ImageUrl=productModel.ImageUrl,
                Url=productModel.Url    
            };
            _productService.Create(entity);

            //bilgilendirme mesajı
             var msg= new AlertMessage
            {
                Message=$"{entity.Name} isimli ürün eklendi.",
                AlertType="success"
            };
            TempData["message"] = JsonConvert.SerializeObject(msg);
            //bilgilendirme mesajı -son-
            return RedirectToAction("ProductList");
        }
        [HttpGet]
        public IActionResult ProductEdit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entity = _productService.GetById((int)id);

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
                Description = entity.Description
            };
            return View(model);

        }
        [HttpPost]
        public IActionResult ProductEdit(ProductModel productModel)
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

            _productService.Update(entity);

            //bilgilendirme mesajı
            var msg = new AlertMessage
            {
                Message = $"{entity.Name} isimli ürün güncellendi.",
                AlertType = "success"
            };
            TempData["message"] = JsonConvert.SerializeObject(msg);
            //bilgilendirme mesajı -son-
            return RedirectToAction("ProductList");

        }
        public IActionResult DeleteProduct(int productId)
        {
            var product = _productService.GetById(productId);
            if (product!=null)
            {
                _productService.Delete(product);
            }

            //bilgilendirme mesajı
            var msg = new AlertMessage
            {
                Message = $"{product.Name} isimli ürün silindi.",
                AlertType = "danger"
            };
            TempData["message"] = JsonConvert.SerializeObject(msg);
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
            var entity = new Category()
            {
                Name = categoryModel.Name,
                Url=categoryModel.Url
            };
            _categoryService.Create(entity);

            //bilgilendirme mesajı
            var msg = new AlertMessage
            {
                Message = $"{entity.Name} isimli category eklendi.",
                AlertType = "success"
            };
            TempData["message"] = JsonConvert.SerializeObject(msg);
            //bilgilendirme mesajı -son-
            return RedirectToAction("CategoryList");
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
            var entity = _categoryService.GetById(categoryModel.CategoryId);
            if (entity == null)
            {
                return NotFound();
            }
            entity.Name = categoryModel.Name;
            entity.Url = categoryModel.Url;

            _categoryService.Update(entity);

            //bilgilendirme mesajı
            var msg = new AlertMessage
            {
                Message = $"{entity.Name} isimli kategory güncellendi.",
                AlertType = "success"
            };
            TempData["message"] = JsonConvert.SerializeObject(msg);
            //bilgilendirme mesajı -son-
            return RedirectToAction("CategoryList");

        }
        public IActionResult DeleteCategory(int categoryId)
        {
            var category = _categoryService.GetById(categoryId);
            if (category != null)
            {
                _categoryService.Delete(category);
            }

            //bilgilendirme mesajı
            var msg = new AlertMessage
            {
                Message = $"{category.Name} isimli kategory silindi.",
                AlertType = "danger"
            };
            TempData["message"] = JsonConvert.SerializeObject(msg);
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
    }
}

