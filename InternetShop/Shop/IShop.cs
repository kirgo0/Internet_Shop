
namespace InternetShop.Shop
{
    public interface IShop
    {
        void CreateNewShopItem(string itemName, double itemPrice, string itemDescription);
        void DeleteShopItem(string itemName);
        ShopItem GetShopItem(string itemName);

        ShopItem BuyShopItem(string itemName);
    }
}