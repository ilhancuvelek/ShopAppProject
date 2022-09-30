using Business.Abstract;
using DataAccess.Abstract;
using Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ShopApp.Extensions;
using ShopApp.Identity;
using ShopApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApp.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AdminController : Controller
    {
        private IProductService _productService;
        private ICategoryService _categoryService;
        private RoleManager<IdentityRole> _roleManager;
        private UserManager<User> _userManager;
        public AdminController(IProductService productService, ICategoryService categoryService, RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            _productService = productService;
            _categoryService = categoryService;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        //Users
        public IActionResult UserList()
        {
            return View(_userManager.Users);
        }
        
        //Users --SON--

        //Üyelik Yönetimi
        public IActionResult RoleList()
        {
            return View(_roleManager.Roles);
        }
        public IActionResult RoleCreate()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> RoleCreate(RoleModel roleModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _roleManager.CreateAsync(new IdentityRole(roleModel.Name));
                if (result.Succeeded)
                {
                    return RedirectToAction("RoleList","Admin");
                }
            }
            return View(roleModel);
        }
        public async Task<IActionResult> RoleEdit(string id)
        {
            var role=await _roleManager.FindByIdAsync(id);
            var members =new List<User>();
            var nonmembers = new List<User>();
            foreach (var user in _userManager.Users)
            {
                var list=await _userManager.IsInRoleAsync(user, role.Name)?members:nonmembers;
                list.Add(user);
            }
            var model = new RoleDetails()
            {
                Role=role,
                Members=members,
                NonMembers=nonmembers
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> RoleEdit(RoleEditModel roleEditModel)
        {
            if (ModelState.IsValid)
            {
                foreach (var userId in roleEditModel.IdsToAdd ?? new string[] {})
                {
                    var user = await _userManager.FindByIdAsync(userId);
                    if (user!=null)
                    {
                        var result = await _userManager.AddToRoleAsync(user, roleEditModel.RoleName);
                        if (!result.Succeeded)
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError("", error.Description);
                            }
                        }
                    }
                }
                foreach (var userId in roleEditModel.IdsToDelete ?? new string[] { })
                {
                    var user = await _userManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        var result = await _userManager.RemoveFromRoleAsync(user, roleEditModel.RoleName);
                        if (!result.Succeeded)
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError("", error.Description);
                            }
                        }
                    }
                }
            }
            return Redirect("/admin/role/"+roleEditModel.RoleId);
        }
        //Üyelik Yönetimi --SON--

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
                    TempData.Put("message", new AlertMessage()
                    {
                        Title = "kayıt eklendi",
                        Message = "kayıt eklendi.",
                        AlertType = "success"
                    });
                    //bilgilendirme mesajı -son-
                    return RedirectToAction("ProductList");
                }
                TempData.Put("message", new AlertMessage()
                {
                    Title = "Hata",
                    Message = _productService.ErrorMessage,
                    AlertType = "success"
                });
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
                IsApproved=entity.IsApproved,
                IsHome=entity.IsHome,
                SelectedCategories = entity.ProductsCategories.Select(p => p.Category).ToList()//bu seçilen ürünün kategorileri
            };
            ViewBag.Categories = _categoryService.GetAll();//bu tüm kategoriler
            return View(model);

        }
        [HttpPost]
        public async Task<IActionResult> ProductEdit(ProductModel productModel, int[] categoryIds,IFormFile file)
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
                entity.Description = productModel.Description;
                entity.IsHome = productModel.IsHome;
                entity.IsApproved = productModel.IsApproved;

                //resim ekleme
                if (file!=null)
                {
                    var extention = Path.GetExtension(file.FileName);
                    var randomName = string.Format($"{Guid.NewGuid()}{extention}");
                    entity.ImageUrl = randomName;
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img", randomName);

                    using (var stream=new FileStream(path,FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }
                //resim ekleme --SON--

                if (_productService.Update(entity, categoryIds)) // iş kuralı için update i bool a çevirdik
                {
                    //bilgilendirme mesajı
                    TempData.Put("message", new AlertMessage()
                    {
                        Title = "kayıt güncellendi",
                        Message = "kayıt güncellendi.",
                        AlertType = "success"
                    });
                    //bilgilendirme mesajı -son-
                    return RedirectToAction("ProductList");
                }
                TempData.Put("message", new AlertMessage()
                {
                    Title = "Hata",
                    Message = _productService.ErrorMessage,
                    AlertType = "danger"
                });
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
            TempData.Put("message", new AlertMessage()
            {
                Title = "kayıt silindi",
                Message = "kayıt silindi.",
                AlertType = "danger"
            });
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
                TempData.Put("message", new AlertMessage()
                {
                    Title = "kayıt eklendi",
                    Message = $"{entity.Name} isimli kayıt eklendi.",
                    AlertType = "success"
                });
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
                TempData.Put("message", new AlertMessage()
                {
                    Title = "kayıt güncellendi",
                    Message = $"{entity.Name} isimli kayıt güncellendi.",
                    AlertType = "success"
                });
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
            TempData.Put("message", new AlertMessage()
            {
                Title = "kayıt silindi",
                Message = "kayıt silindi.",
                AlertType = "danger"
            });
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

