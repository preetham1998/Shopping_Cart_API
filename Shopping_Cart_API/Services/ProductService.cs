using Microsoft.AspNetCore.Mvc;
using ShoppingCartAPI.Models;
using ShoppingCartAPI.Respository;
using ShoppingCartAPI.Services;

namespace ShoppingCartAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository productRepository;

        public ProductService(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        public Product GetProductById(int productId)
        {
            return productRepository.GetProductById(productId);
        }

        public List<Product> GetAllProducts()
        {
            return productRepository.GetAllProducts();
        }

        public void UpdateProduct(Product product)
        {
            productRepository.UpdateProduct(product);
        }

        public void CreateProduct(Product product)
        {
            productRepository.AddProduct(product);
        }



    }
}

