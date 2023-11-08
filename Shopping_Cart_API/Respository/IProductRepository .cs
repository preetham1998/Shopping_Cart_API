using ShoppingCartAPI.Models;

namespace ShoppingCartAPI.Respository
{
    public interface IProductRepository
    {
        Product GetProductById(int productId);
        List<Product> GetAllProducts();
        void UpdateProduct(Product product);
        void AddProduct(Product product);
    }
}
