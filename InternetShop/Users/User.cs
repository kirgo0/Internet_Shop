using System.Collections.Generic;
using System.Runtime.Serialization;
using InternetShop.Shop;

namespace InternetShop.Users
{
    [DataContract]
    public abstract class User 
    {
        [DataMember]
        protected OnlineShop Shop { get; set; }
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        private string Password { get; set; }
        [DataMember]
        private double UserBalance { get; set; }
        [DataMember]
        private List<ShopItem> _basket;

        public User(List<ShopItem> basket, OnlineShop shop, string userName, string password, double userBalance)
        {
            _basket = basket;
            Shop = shop;
            UserName = userName;
            Password = password;
            UserBalance = userBalance;
        }

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