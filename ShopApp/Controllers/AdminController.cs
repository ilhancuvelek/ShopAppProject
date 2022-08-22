using DataAccess.Abstract;
using Entity;
using Microsoft.AspNetCore.Mvc;
using ShopApp.Models;

namespace ShopApp.Controllers
{
    public class AdminController : Controller
    {
        private IProductRepository _productRepository;
        public AdminController(IProductRepository productRepository)
        {
            _productRepository= productRepository;
        }
        public IActionResult ProductList()
        {
            var productListViewModel = new ProductListViewModel()
            {
                Products = _productRepository.GetAll()
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
            _productRepository.Create(entity);
            return RedirectToAction("ProductList");
        }
    }
}
