public class ShoppingCart
{
    private List<ShoppingCartItem> items;

    public ShoppingCart()
    {
        items = new List<ShoppingCartItem>();
    }

    public void AddItem(int productId, int quantity)
    {
        var existingItem = items.FirstOrDefault(item => item.ProductId == productId);

        if (existingItem != null)
        {
            existingItem.Quantity += quantity;
        }
        else
        {
            items.Add(new ShoppingCartItem(productId, quantity));
        }
    }

    public void RemoveItem(int productId)
    {
        items.RemoveAll(item => item.ProductId == productId);
    }

    public List<ShoppingCartItem> GetItems()
    {
        return items;
    }

    public int GetQuantity(int productId)
    {
        var item = items.FirstOrDefault(item => item.ProductId == productId);

        if (item != null)
        {
            return item.Quantity;
        }
        else
        {
            return 0;
        }
    }


   

}
