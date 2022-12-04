using System;
using System.Collections.Generic;
using Newtonsoft.Json;

using InternetShop.Shop;

namespace InternetShop.Users
{
    [JsonObject]
    public abstract class User 
    {
        // [DataMember]
        protected IShop Shop { get; set; }
        
        public string UserName { get; set; }
        
        [JsonRequired]
        private string Password { get; set; }

        [JsonRequired]
        private int _balance { get; set; }

        [JsonRequired]
        public AccountType CheckAccountType { get; set; }

        [JsonIgnore]
        public int UserBalance
        {
            get => _balance;
            set => _balance += value;
        }
        // [DataMember]        
        public readonly List<ShopItem> Cart;
        // [DataMember]        
        public readonly List<ShopItem> PurchaseHistory;

        protected User(int balance, List<ShopItem> cart, List<ShopItem> purchaseHistory, IShop shop, string userName, string password, AccountType checkAccountType)
        {
            _balance = balance;
            Cart = cart;
            PurchaseHistory = purchaseHistory;
            Shop = shop;
            UserName = userName;
            Password = password;
            CheckAccountType = checkAccountType;
        }

        protected User(string userName, string password, AccountType checkAccountType)
        {
            UserName = userName;
            Password = password;
            CheckAccountType = checkAccountType;
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

        public bool RemoveShopItemFromCart(int pos)
        {
            try
            {
                Cart.RemoveAt(pos);
                return true;
            }
            catch (ArgumentOutOfRangeException e)
            {
                return false;
            }
        }
        public bool AddShopItemToCart(string itemName)
        {
            ShopItem item = GetShopItem(itemName);
            if(item != null)
            {
                Cart.Add(item);
                // Successful add to basket message
            }
            return true;
        }

        public bool BuyItemsFromCart()
        {
            double sum = 0;
            foreach (var item in Cart)
            {
                sum += item.ItemPrice;
            }

            if (sum > UserBalance) return false;
            for(var i = 0; i < Cart.Count; i++)
            {
                ShopItem basketItem = Cart[i];
                if (GetShopItem(basketItem.ItemName) != null)
                {
                    Shop.BuyShopItem(basketItem.ItemName,this);
                    UserBalance = -basketItem.ItemPrice;
                    PurchaseHistory.Add(basketItem);
                    // Successful buy message
                    Cart.Remove(basketItem);
                    i--;
                }
            }                    
            return true;
        }

        public void CheckCartProducts()
        {
            for (int i = 0; i < Cart.Count; i++)
            {
                var item = Cart[i];
                if (Shop.GetShopItem(item.ItemName) == null)
                {
                    Cart.Remove(item);
                    i--;
                }
            }
        }

        public bool BuyItem(string itemName) 
        {
            if (GetShopItem(itemName) != null)
            {
                ShopItem item = GetShopItem(itemName);
                if (item.ItemPrice > UserBalance) return false;
                Shop.BuyShopItem(item.ItemName,this);
                UserBalance = -item.ItemPrice;
                PurchaseHistory.Add(item);
                return true;
            }
            return false;
        }
    }
}