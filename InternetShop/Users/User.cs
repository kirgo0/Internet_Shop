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

        public double UserBalance
        {
            get => _balance;
            set => _balance += value;
        }
        public readonly List<ShopItem> Cart;
        public readonly List<ShopItem> PurchaseHistory;

        protected User(List<ShopItem> basket, List<ShopItem> purchaseHistory, IShop shop, string userName, string password, double userBalance)
        {
            Cart = basket;
            PurchaseHistory = purchaseHistory;
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
            Cart = new List<ShopItem>();
            PurchaseHistory = new List<ShopItem>();
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
        public ShopItem GetShopItem(string itemName)
        {            
            return Shop.GetShopItem(itemName);
        }

        public List<ShopItem> GetShopItems(string keyWord)
        {
            return Shop.GetShopItems(keyWord);
        }

        public void AddShopItemToCart(string itemName)
        {
            ShopItem item = GetShopItem(itemName);
            if(item != null)
            {
                Cart.Add(item);
                // Successful add to basket message
            }
        }

        public void BuyItemsFromCart()
        {
            for(var i = 0; i < Cart.Count; i++)
            {
                ShopItem basketItem = Cart[i];
                if (GetShopItem(basketItem.ItemName) != null)
                {
                    if (basketItem.ItemPrice <= UserBalance)
                    {
                        Shop.BuyShopItem(basketItem.ItemName);
                        UserBalance -= basketItem.ItemPrice;
                        PurchaseHistory.Add(basketItem);
                        // Successful buy message
                        Cart.Remove(basketItem);
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