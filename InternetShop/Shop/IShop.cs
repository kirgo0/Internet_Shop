namespace InternetShop.Shop
{
    public interface IShop
    {
        void CreateNewShopItem(string itemName, double itemPrice);
        void DeleteShopItem(string itemName);
        ShopItem SearchShopItem(string itemName);

    }
}