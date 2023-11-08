public class ShoppingCartItem
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }

    public ShoppingCartItem(int productId, int quantity)
    {
        ProductId = productId;
        Quantity = quantity;
    }
}
