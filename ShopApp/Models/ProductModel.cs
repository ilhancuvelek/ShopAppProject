namespace ShopApp.Models
{

    //entity deki lerin hepsini kullanmak istemediğimizde veya
    //değişiklik yapacağımız zaman entity de yapmayalım diye oluşturduk.
    public class ProductModel
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public string Url { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
    }
}
