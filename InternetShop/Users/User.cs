using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using InternetShop.Shop;

namespace InternetShop.Users
{
    public abstract class User 
    {
        protected IShop Shop { get; set; }
        public string UserName { get; set; }
        private string Password { get; set; }
        private double _balance;

        private double UserBalance
        {
            get => _balance;
            set => _balance += value;
        }
        private readonly List<ShopItem> _basket;
        private List<ShopItem> _purchaseHistory;

        protected User(List<ShopItem> basket, List<ShopItem> purchaseHistory, IShop shop, string userName, string password, double userBalance)
        {
            _basket = basket;
            _purchaseHistory = purchaseHistory;
            Shop = shop;
            UserName = userName;
            Password = password;
            UserBalance = userBalance;
        }

        protected User(string userName, string password)
        {
            UserName = userName;
            Password = password;
            UserBalance = 100000;
            _basket = new List<ShopItem>();
            _purchaseHistory = new List<ShopItem>();
        }
        
        public bool SignIn(string userName, string password)
        {
            // Sign in message
            return UserName == userName && Password == password;
        }
        public void GetShop(IShop shop)
        {
            Shop = shop;
        }
        public abstract ShopItem SearchShopItem(string itemName);

        public void AddShopItemToBasket(string itemName)
        {
            ShopItem item = Shop.GetShopItem(itemName);
            if(item != null)
            {
                _basket.Add(item);
                // Successful add to basket message
            }
        }

        public void BuyItemsFromBasket()
        {
            for(var i = 0; i < _basket.Count; i++)
            {
                ShopItem basketItem = _basket[i];
                if (Shop.GetShopItem(basketItem.ItemName) != null)
                {
                    if (basketItem.ItemPrice <= UserBalance)
                    {
                        Shop.BuyShopItem(basketItem.ItemName);
                        UserBalance -= basketItem.ItemPrice;
                        _purchaseHistory.Add(basketItem);
                        // Successful buy message
                        _basket.Remove(basketItem);
                        i--;
                    }
                    else
                    {
                        // No balance message
                    }
                }
            }
        }
    }
}