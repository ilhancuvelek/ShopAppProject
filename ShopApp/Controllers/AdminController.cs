using Business.Abstract;
using DataAccess.Abstract;
using Entity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ShopApp.Models;

namespace ShopApp.Controllers
{
    public class AdminController : Controller
    {
       private IProductService _productService;
        public AdminController(IProductService IProductService)
        {
            _productService = IProductService;
        }
        public IActionResult ProductList()
        {
            var productListViewModel = new ProductListViewModel()
            {
                Products = _productService.GetAll()
            };
            return View(productListViewModel);
        }
        [HttpGet]
        public IActionResult CreateProduct()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateProduct(ProductModel productModel)
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
        public IActionResult Edit(int? id)
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
        public IActionResult Edit(ProductModel productModel)
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
    }
}

