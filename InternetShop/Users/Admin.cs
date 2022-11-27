using System;
using InternetShop.Shop;

namespace InternetShop.Users
{
    public class Admin : User , IAdmin
    {
        public Admin(string userName, string password) : base(userName, password)
        {
        }
        public void CreateNewItem(string itemName, double itemPrice)
        {
            Shop.CreateNewShopItem(itemName,itemPrice);
        }

        public void DeleteItem(string itemName)
        {
            Shop.DeleteShopItem(itemName);
        }

        public override ShopItem SearchShopItem(string itemName)
        {
            Console.WriteLine(Shop.SearchShopItem(itemName) != null ? "Item Founded" : "Item not founded");
            return Shop.SearchShopItem(itemName);
        }
    }
}