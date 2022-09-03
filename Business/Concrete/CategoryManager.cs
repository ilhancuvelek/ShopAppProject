using Business.Abstract;
using DataAccess.Abstract;
using Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Concrete
{
    public class CategoryManager : ICategoryService
    {
        private ICategoryRepository _categoryRepository;
        public CategoryManager(ICategoryRepository categoryRepository)
        {
            _categoryRepository= categoryRepository;
        }
        public void Create(Category category)
        {
            _categoryRepository.Create(category);
        }

        public void Delete(Category category)
        {
            _categoryRepository.Delete(category);
        }

        public void DeleteFromCategory(int productId, int categoryId)
        {
            _categoryRepository.DeleteFromCategory(productId, categoryId);  
        }

        public List<Category> GetAll()
        {
            return _categoryRepository.GetAll();
        }

        public Category GetById(int id)
        {
            return _categoryRepository.GetById(id);
        }

        public Category GetByIdWithProducts(int id)
        {
            return _categoryRepository.GetByIdWithProducts(id);
        }

        public void Update(Category category)
        {
            _categoryRepository.Update(category);
        }

        // İŞ KURALI
        public string ErrorMessage { get; set; }
        public bool Validation(Category entity)
        {
            throw new NotImplementedException();
        }
    }
}
