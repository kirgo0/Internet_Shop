
using System.Collections.Generic;

namespace InternetShop.Shop
{
    public interface IShop
    {
        void CreateNewShopItem(string itemName, double itemPrice, string itemDescription);
        void DeleteShopItem(string itemName);
        ShopItem GetShopItem(string itemName);

        List<ShopItem> GetShopItems(string keyword);

        ShopItem BuyShopItem(string itemName);
    }
}