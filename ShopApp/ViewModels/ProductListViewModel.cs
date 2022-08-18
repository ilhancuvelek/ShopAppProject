using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApp.ViewModels
{
    public class PageInfo
    {
        public int TotalItems { get; set; }//Veri tabanında kaç ürün var (filtrlenmemiş)
        public int ItemsPerPage { get; set; }//sayfa başı kaç ürün
        public int CurrentPage { get; set; }
        public string CurrentCategory { get; set; }

        public int TotalPages()
        {
            return (int)Math.Ceiling((decimal)TotalItems/ItemsPerPage);
        }
    }
    public class ProductListViewModel
    {
        public PageInfo PageInfo { get; set; }
        public List<Product> Products { get; set; }
  
    }
}
