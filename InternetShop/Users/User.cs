using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Newtonsoft.Json;

using InternetShop.Shop;

namespace InternetShop.Users
{
    [JsonObject]
    public abstract class User 
    {
        protected IShop Shop { get; set; }
        
        public string UserName { get; set; }
        
        [JsonRequired]
        private string Password { get; set; }
        
        [JsonIgnore]
        public string UserPassword {
            get => Password;
            set => Password = value;
        }

        [JsonRequired]
        private int Balance { get; set; }

        [JsonRequired]
        public AccountType CheckAccountType { get; set; }

        [JsonIgnore]
        public int UserBalance
        {
            get => Balance;
            set => Balance += value;
        }
        
        [JsonRequired]
        public readonly List<ShopItem> Cart;
        
        [JsonRequired]
        public readonly List<ShopItem> PurchaseHistory;
        
        [JsonConstructor]
        protected User(AccountType checkAccountType) {
            CheckAccountType = checkAccountType;
        }
        protected User(string userName, string password, AccountType checkAccountType)
        {
            UserName = userName;
            Password = GetHashedPassword(password);
            CheckAccountType = checkAccountType;
            UserBalance = 0;
            Cart = new List<ShopItem>();
            PurchaseHistory = new List<ShopItem>();
        }
        
        public bool SignIn(string userName, string password)
        {
            return UserName == userName && CheckHashedPassword(Password, password);
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

        private string GetHashedPassword(string password)
        {
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000);
            byte[] hash = pbkdf2.GetBytes(20);
            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);
            string passwordHash = Convert.ToBase64String(hashBytes);
            return passwordHash;
        }

        private bool CheckHashedPassword(string savedPasswordHash, string password)
        {
            byte[] hashBytes = Convert.FromBase64String(savedPasswordHash);
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000);
            byte[] hash = pbkdf2.GetBytes(20);
            for (int i=0; i < 20; i++)
                if (hashBytes[i + 16] != hash[i])
                    return false;
            return true;
        }
    }
}