using DataAccess.Abstract;
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
    }
}
