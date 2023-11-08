using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using ShoppingCartAPI.Models;
using ShoppingCartAPI.Services;
using System.Text.Json;

namespace ShoppingCartAPI.Controllers
{
    [Route("api/shopping")]
    [ApiController]
    public class ShoppingCartController : Controller
    {
        private IMemoryCache _cache;
        private readonly IProductService productService;

        public ShoppingCartController(IMemoryCache cache, IProductService productService)
        {
            _cache = cache;
            this.productService = productService;
        }


        //[HttpGet("products/{productId}")]
        //public IActionResult GetProductById(int productId)
        //{
        //    var product = productService.GetProductById(productId);

        //    if (product == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(product);
        //}

        //[HttpGet("AllProducts")]
        //public IActionResult GetAllProducts()
        //{
        //    var products = productService.GetAllProducts();
        //    return Ok(products);
        //}

        [HttpPost("addToCart")]
        public IActionResult AddItemToCart([FromBody] AddCartItemRequest request)
        {
            var product = productService.GetProductById(request.ProductId);

            if (product == null)
            {
                return NotFound();
            }

            if (product.Quantity < request.Quantity)
            {
                return BadRequest("Insufficient product quantity");
            }

            List<CartItem> shoppingCart;

            // Try to get the existing cart data from the cache
            if (_cache.TryGetValue("shoppingCart", out shoppingCart))
            {
                // Check if the product with the same ID already exists in the cart
                var existingItem = shoppingCart.FirstOrDefault(item => item.ProductId == request.ProductId);

                if (existingItem != null)
                {
                    // Update the quantity of the existing item
                    existingItem.Quantity += request.Quantity;
                }
                else
                {
                    // Create a new cart item if it doesn't exist in the cart
                    var item = new CartItem
                    {
                        ProductId = product.Id,
                        ProductName = product.Name,
                        Price = product.Price,
                        Quantity = request.Quantity
                    };

                    shoppingCart.Add(item);
                }
            }
            else
            {
                // If the cart doesn't exist in the cache, create a new cart and add the item
                shoppingCart = new List<CartItem>
        {
            new CartItem
            {
                ProductId = product.Id,
                ProductName = product.Name,
                Price = product.Price,
                Quantity = request.Quantity
            }
        };
            }

            // Store or update the cart data in the cache
            _cache.Set("shoppingCart", shoppingCart);

            product.Quantity -= request.Quantity;

            return Ok("Item added to the cart");
        }


        [HttpGet("cart")]
        public IActionResult GetCartItems()
        {
            if (!_cache.TryGetValue("shoppingCart", out List<CartItem> shoppingCart))
            {
                shoppingCart = new List<CartItem>();
            }

            return Ok(shoppingCart);
        }

        [HttpDelete("removeFromCart/{productId}")]
        public IActionResult RemoveItemFromCart(int productId)
        {
            // Try to get the existing cart data from the cache
            if (_cache.TryGetValue("shoppingCart", out List<CartItem> shoppingCart))
            {
                var existingItem = shoppingCart.FirstOrDefault(item => item.ProductId == productId);
                if (existingItem != null)
                {
                    // Remove the item from the cart
                    shoppingCart.Remove(existingItem);
                    // Update or remove the cart data in the cache
                    if (shoppingCart.Count > 0)
                    {
                        _cache.Set("shoppingCart", shoppingCart);
                    }
                    else
                    {
                        // If the cart is empty, remove it from the cache
                        _cache.Remove("shoppingCart");
                    }
                    return Ok("Item removed from the cart");
                }
            }

            return NotFound("Item not found in the cart");
        }

        [HttpPut("update-inventory/{productId}")]
        public IActionResult UpdateInventory(int productId, [FromBody] int newQuantity)
        {
            // Find the product in your inventory
            var product = productService.GetAllProducts().FirstOrDefault(p => p.Id == productId);

            if (product == null)
            {
                return NotFound("Product not found");
            }

            if (newQuantity < 0)
            {
                return BadRequest("Invalid inventory quantity");
            }

            // Update the inventory quantity
            product.Quantity = newQuantity;

            // If the product is in the cart, update the cart quantity
            if (_cache.TryGetValue("shoppingCart", out List<CartItem> shoppingCart))
            {
                var cartItem = shoppingCart.FirstOrDefault(item => item.ProductId == productId);
                if (cartItem != null)
                {
                    cartItem.Quantity = Math.Min(cartItem.Quantity, newQuantity);
                }
            }

            return Ok("Inventory updated");
        }

        [HttpGet("inventory")]
        public IActionResult GetInventoryWithUpdatedQuantities()
        {
            // If there's no cart data, return the original inventory
            if (!_cache.TryGetValue("shoppingCart", out List<CartItem> shoppingCart))
            {
                return Ok(productService.GetAllProducts());
            }

            // Clone the original inventory to avoid modifying it
            var updatedInventory = productService.GetAllProducts().Select(product => new Product
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Quantity = product.Quantity
            }).ToList();

            // Update the inventory quantities based on the items in the cart
            foreach (var cartItem in shoppingCart)
            {
                var product = updatedInventory.FirstOrDefault(p => p.Id == cartItem.ProductId);
                if (product != null)
                {
                    // Update the inventory quantity by subtracting the cart quantity
                    product.Quantity -= cartItem.Quantity;
                }
            }

            return Ok(updatedInventory);
        }



    }
}
