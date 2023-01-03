namespace InternetShop.Users
{
    public interface IBuyer
    {
        bool RemoveShopItemFromCart(int pos);
        bool AddShopItemToCart(string itemName);
        bool BuyItemsFromCart();
        void CheckCartProducts();
        bool BuyItem(string itemName);
    }
}