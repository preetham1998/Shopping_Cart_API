using ShoppingCartAPI.Models;

namespace ShoppingCartAPI.Respository
{
    public class ProductRepository : IProductRepository
    {
        private static List<Product> _products = new List<Product>()
    {
        new Product { Id = 1, Name = "Product 1", Description = "A description of product 1", Price = 19.99M, Quantity = 100, Category = "Electronics", ImageUrl = "https://example.com/product1.jpg" },
        new Product { Id = 2, Name = "Product 2", Description = "A description of product 2", Price = 14.99M, Quantity = 50, Category = "Books", ImageUrl = "https://example.com/product2.jpg" },
        new Product { Id = 3, Name = "Product 3", Description = "A description of product 3", Price = 29.99M, Quantity = 25, Category = "Clothing", ImageUrl = "https://example.com/product3.jpg" }
    };

        public Product GetProductById(int productId)
        {
            return _products.FirstOrDefault(p => p.Id == productId);
        }

        public List<Product> GetAllProducts()
        {
            return _products;
        }

        public void UpdateProduct(Product product)
        {
            var index = _products.FindIndex(p => p.Id == product.Id);
            if (index != -1)
            {
                _products[index] = product;
            }
        }

        public void AddProduct(Product product)
        {
            _products.Add(product);
        }


    
    }
}
