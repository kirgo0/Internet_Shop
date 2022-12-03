using System;
using System.Runtime.Serialization;
using InternetShop.Shop;

namespace InternetShop.Users
{
    [DataContract]
    public class Admin : User , IAdmin
    {
        public Admin(string userName, string password) : base(userName, password, AccountType.Admin)
        {
        }
        public bool CreateNewItem(string itemName, int itemPrice, string itemDescription)
        {
            return Shop.CreateNewShopItem(itemName,itemPrice,itemDescription);
        }

        public bool ChangeShopItem(string itemName, string newItemName, int newItemPrice, string newItemDescription)
        {
            return Shop.ChangeShopItem(itemName,newItemName,newItemPrice,newItemDescription);
        }
        public bool RemoveItem(string itemName)
        {
            return Shop.DeleteShopItem(itemName);
        }
        
    }
}