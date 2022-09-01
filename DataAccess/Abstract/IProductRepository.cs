using Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Abstract
{
    public interface IProductRepository:IRepository<Product>
    {
        //Kategori inculude
        Product GetProductDetails(string productNameUrl);

        //Kategori adına göre ürün filtreleme
        List<Product> GetProductsByCategory(string name, int page, int pageSize);

        //sayfalama
        int GetCountByCategory(string category);

        //anasayfa ürünleri
        List<Product> GetHomePageProducts();

        //Aranan ürünler
        List<Product> GetSearchResult(string searchString);

        //product edit sayfası kategori checkbox
        Product GetByIdWithCategories(int id);

        //aşırı yükleme
        void Update(Product product, int[] categoryIds);
    }
}
