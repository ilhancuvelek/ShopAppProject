using Business.Abstract;
using Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShopApp.Models;
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
        public IActionResult List(string category,int page=1)
        {
            //sayfalama
            const int pageSize = 3;
            var productViewModel = new ProductListViewModel
            {
                //(sayfalama) ProductListViewModel içinde yazmamızın nedeni list.cshtml de kullanmak
                PageInfo = new PageInfo()
                {
                    TotalItems = _productService.GetCountByCategory(category),
                    CurrentPage=page,
                    ItemsPerPage=pageSize,
                    CurrentCategory=category
                },
                Products = _productService.GetProductsByCategory(category,page,pageSize) 
            };
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
        public IActionResult Search(string q)
        {
            var productViewModel = new ProductListViewModel
            {
                
                Products = _productService.GetSearchResult(q)
            };
            return View(productViewModel);
        }
    }
}
