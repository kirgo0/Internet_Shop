using System;
using InternetShop.Shop;

namespace InternetShop.Users
{
    public class Admin : User , IAdmin
    {
        public Admin(string userName, string password) : base(userName, password)
        {
        }
        public void CreateNewItem(string itemName, double itemPrice, string itemDescription)
        {
            Shop.CreateNewShopItem(itemName,itemPrice,itemDescription);
        }

        public void DeleteItem(string itemName)
        {
            Shop.DeleteShopItem(itemName);
        }

        public override ShopItem SearchShopItem(string itemName)
        {
            Console.WriteLine(Shop.GetShopItem(itemName) != null ? "Item Founded" : "Item not founded");
            return Shop.GetShopItem(itemName);
        }
    }
}