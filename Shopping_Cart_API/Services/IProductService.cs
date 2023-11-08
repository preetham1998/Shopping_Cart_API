using ShoppingCartAPI.Models;

namespace ShoppingCartAPI.Services
{
    public interface IProductService
    {
        Product GetProductById(int productId);
        List<Product> GetAllProducts();
        void UpdateProduct(Product product);
        void CreateProduct(Product product);
    }
}
