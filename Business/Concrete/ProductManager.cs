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
        public void Create(Product product)
        {
            _productRepository.Create(product);
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

        public int GetCountByCategory(string category)
        {
            return _productRepository.GetCountByCategory(category);
        }

        public Product GetProductDetails(string productNameUrl)
        {
            return _productRepository.GetProductDetails(productNameUrl);
        }

        public List<Product> GetProductsByCategory(string name, int page, int pageSize)
        {
            return _productRepository.GetProductsByCategory(name, page, pageSize);
        }

        public void Update(Product product)
        {
            _productRepository.Update(product);
        }
    }
}
