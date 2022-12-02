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
        private int _balance;

        public AccountType CheckAccountType { get; }

        public int UserBalance
        {
            get => _balance;
            set => _balance += value;
        }
        public readonly List<ShopItem> Cart;
        public readonly List<ShopItem> PurchaseHistory;

        protected User(List<ShopItem> basket, List<ShopItem> purchaseHistory, IShop shop, string userName, string password, int userBalance, AccountType checkAccountType)
        {
            Cart = basket;
            PurchaseHistory = purchaseHistory;
            Shop = shop;
            UserName = userName;
            Password = password;
            UserBalance = userBalance;
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
                    Shop.BuyShopItem(basketItem.ItemName);
                    UserBalance = -basketItem.ItemPrice;
                    PurchaseHistory.Add(basketItem);
                    // Successful buy message
                    Cart.Remove(basketItem);
                    i--;
                }
            }                    
            return true;
        }

        public bool BuyItem(string itemName)
        {
            if (GetShopItem(itemName) != null)
            {
                ShopItem item = GetShopItem(itemName);
                if (item.ItemPrice > UserBalance) return false;
                Shop.BuyShopItem(item.ItemName);
                UserBalance = -item.ItemPrice;
                PurchaseHistory.Add(item);
                return true;
            }
            return false;
        }
    }
}