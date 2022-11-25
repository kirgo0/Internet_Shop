using System.Collections.Generic;
using InternetShop.Shop;

namespace InternetShop.Users
{
    public abstract class User
    {
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
    }
}