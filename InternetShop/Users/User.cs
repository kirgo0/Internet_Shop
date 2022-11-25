using System.Collections.Generic;
using InternetShop.Shop;

namespace InternetShop.Users
{
    public abstract class User 
    {
        protected OnlineShop Shop { get; set; }
        private string UserName { get; set; }
        private string Password { get; set; }
        private double UserBalance { get; set; }
        private List<ShopItem> _basket;

        protected User(string userName, string password)
        {
            UserName = userName;
            Password = password;
            UserBalance = 0;
            _basket = new List<ShopItem>();
        }

        public abstract ShopItem SearchShopItem(string itemName);

        public bool Login(string userName, string password)
        {
            return UserName == userName && Password == password;
        }
        public void GetShop(OnlineShop shop)
        {
            Shop = shop;
        }
    }
}