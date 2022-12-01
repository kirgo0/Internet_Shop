using System;
using System.Collections.Generic;
using System.Threading;
using InternetShop.Users;

namespace InternetShop.Shop
{
    public class OnlineShop : IShop
    {
        private readonly List<User> _users = new List<User>();
        public readonly List<ShopItem> ProductList = new List<ShopItem>();
        private double _shopBalance = 0;
        private double ShopProfit
        {
            get => _shopBalance;
            set => _shopBalance += value < 10000 ? value * 0.05 : value * 0.1;
        }

        // Interface methods
        public void CreateNewShopItem(string itemName, double itemPrice, string itemDescription)
        {
            if(GetShopItem(itemName) == null) ProductList.Add(new ShopItemExtended(itemName,itemPrice,itemDescription));
        }

        public void DeleteShopItem(string itemName)
        {
            ProductList.Remove(GetShopItem(itemName));
        }

        public ShopItem GetShopItem(string itemName)
        {
            foreach (var item in ProductList)
            {
                if (item.ItemName.ToLower() == itemName.ToLower()) return item;
            }
            return null;
        }

        public List<ShopItem> GetShopItems(string keyWord)
        {
            List<ShopItem> shopItems = new List<ShopItem>();
            foreach (var item in ProductList)
            {
                if (item.ItemName.ToLower().Contains(keyWord.ToLower())) shopItems.Add(item);
            }
            return shopItems;
        }

        public ShopItem BuyShopItem(string itemName)
        {
            if (GetShopItem(itemName) == null) return null;
            ShopProfit = GetShopItem(itemName).ItemPrice;
            return GetShopItem(itemName);
        }

        // Class methods

        public void LoadUsersList()
        {
            
        }

        public User SignUpAdmin(string userName, string password, string passwordRepeat)
        {
            if (GetUser(userName) != null) return null;
            if (password != passwordRepeat) return null;
            User newUser = new Admin(userName, password);
            _users.Add(newUser);
            newUser.GetShop(this);     
            return newUser;
        }
        public User SignUp(string userName, string password, string passwordRepeat)
        {
            if (GetUser(userName) != null) return null;
            if (password != passwordRepeat) return null;
            User newUser = new DefaultUser(userName, password);
            _users.Add(newUser);
            newUser.GetShop(this);     
            return newUser;
        }
        public User SignIn(string userName, string password)
        {
            if (GetUser(userName) == null) return null;
            if (!GetUser(userName).SignIn(userName, password)) return null;
            GetUser(userName).GetShop(this);
            return GetUser(userName);
        }

        private User GetUser(string userName)
        {
            foreach (var user in _users)
            {
                if (user.UserName == userName) return user;
            }
            return null;
        }
        
        // Print methods

    }
}