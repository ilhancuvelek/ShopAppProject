using Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Abstract
{
    public interface ICategoryService:IValidator<Category>
    {
        void Create(Category category);
        void Update(Category category);
        void Delete(Category category);
        Category GetById(int id);
        List<Category> GetAll();

        //category edit product-category birleştirme
        Category GetByIdWithProducts(int id);

        void DeleteFromCategory(int productId, int categoryId);
    }
}
