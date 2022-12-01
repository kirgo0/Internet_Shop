using System;
using InternetShop.Shop;

namespace InternetShop.Users
{
    public class Admin : User , IAdmin
    {
        public Admin(string userName, string password) : base(userName, password, AccountType.Admin)
        {
        }
        public bool CreateNewItem(string itemName, double itemPrice, string itemDescription)
        {
            return Shop.CreateNewShopItem(itemName,itemPrice,itemDescription);
        }

        public bool RemoveItem(string itemName)
        {
            return Shop.DeleteShopItem(itemName);
        }
        
    }
}