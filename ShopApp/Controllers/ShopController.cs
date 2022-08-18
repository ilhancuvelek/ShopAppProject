using Business.Abstract;
using Microsoft.AspNetCore.Mvc;
using ShopApp.Models;
using ShopApp.ViewModels;
using System.Linq;

namespace ShopApp.Controllers
{
    public class ShopController : Controller
    {
        private IProductService _productService;
        public ShopController(IProductService productService)
        {
            this._productService = productService;
        }
        public IActionResult List(string category)
        {
            var productViewModel = new ProductListViewModel { Products = _productService.GetProductsByCategory(category) };
            return View(productViewModel);
        }
        public IActionResult Details(string productNameUrl)
        {
            if (productNameUrl == null)
            {
                return NotFound();
            }
            var product=_productService.GetProductDetails(productNameUrl);
            if (product==null)
            {
                return NotFound();
            }
            
            return View(new ProductDetailModel
            {
                Product=product,
                Categories=product.ProductsCategories.Select(i=>i.Category).ToList()
            });
        }
    }
}
