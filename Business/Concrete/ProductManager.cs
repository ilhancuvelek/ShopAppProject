using Business.Abstract;
using DataAccess.Abstract;
using Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Concrete
{
    public class ProductManager : IProductService
    {
        private IProductRepository _productRepository;
        public ProductManager(IProductRepository productRepository)
        {
            _productRepository=productRepository;
        }
        public bool Create(Product product)
        {
            if (Validation(product))
            {
                _productRepository.Create(product);
                return true;
            }
            return false;
        }

        public void Delete(Product product)
        {
            _productRepository.Delete(product); 
        }

        public List<Product> GetAll()
        {
            return _productRepository.GetAll(); 
        }

        public Product GetById(int id)
        {
            return _productRepository.GetById(id);
        }

        public Product GetByIdWithCategories(int id)
        {
            return _productRepository.GetByIdWithCategories(id);
        }

        public int GetCountByCategory(string category)
        {
            return _productRepository.GetCountByCategory(category);
        }

        public List<Product> GetHomePageProducts()
        {
            return _productRepository.GetHomePageProducts();
        }

        public Product GetProductDetails(string productNameUrl)
        {
            return _productRepository.GetProductDetails(productNameUrl);
        }

        public List<Product> GetProductsByCategory(string name, int page, int pageSize)
        {
            return _productRepository.GetProductsByCategory(name, page, pageSize);
        }

        public List<Product> GetSearchResult(string searchString)
        {
            return _productRepository.GetSearchResult(searchString);
        }

        public void Update(Product product)
        {
            _productRepository.Update(product);
        }

        public void Update(Product product, int[] categoryIds)
        {
            _productRepository.Update(product, categoryIds);
        }

        // İŞ KURALI
        public string ErrorMessage { get; set; }
        public bool Validation(Product entity)
        {
            var isValid = true;
            if (string.IsNullOrEmpty(entity.Name)) 
            {
                ErrorMessage += "ürün ismi girmelisiniz \n";
                isValid = false;
            }
            if (entity.Price<0)
            {
                ErrorMessage += "ürün fiyatı negatif olamaz \n";
                isValid = false;
            }
            return isValid;
        }
    }
}
