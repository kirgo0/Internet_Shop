using System.Collections.Generic;
using InternetShop.Users;

namespace InternetShop.Shop
{
    public interface IShop
    {
        bool CreateNewShopItem(string itemName, int itemPrice, string itemDescription);
        bool ChangeShopItem(string itemName, string newItemName, int newItemPrice, string newItemDescription);
        bool DeleteShopItem(string itemName);
        ShopItem GetShopItem(string itemName);

        List<ShopItem> GetShopItems(string keyword);

        ShopItem BuyShopItem(string itemName, User user);
    }
}