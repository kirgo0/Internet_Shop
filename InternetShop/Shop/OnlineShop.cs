using System.Collections.Generic;
using System.IO;
using InternetShop.Data;
using InternetShop.Users;
using Newtonsoft.Json;

namespace InternetShop.Shop
{
    public class OnlineShop : IShop, ILoader
    {
        private List<User> _users = new List<User>();
        public List<ShopItem> ProductList = new List<ShopItem>();
        public List<ShopItemHistory> ShopHistory = new List<ShopItemHistory>();
        public double ShopBalance = 0;
        private double ShopProfit
        {
            get => ShopBalance;
            set => ShopBalance += value < 10000 ? value * 0.05 : value * 0.1;
        }

        // Interface methods
        public bool CreateNewShopItem(string itemName, int itemPrice, string itemDescription)
        {
            if (GetShopItem(itemName) == null)
            {
                ProductList.Add(new ShopItemExtended(itemName,itemPrice,itemDescription));
                return true;
            }
            return false;
        }

        public bool ChangeShopItem(string itemName, string newItemName, int newItemPrice, string newItemDescription)
        {
            if (GetShopItem(itemName) != null)
            {
                var item = (ShopItemExtended) GetShopItem(itemName);
                item.ItemName = newItemName;
                item.ItemPrice = newItemPrice;
                item.ItemDescription = newItemDescription;
                return true;
            }
            return false;
        }

        public bool DeleteShopItem(string itemName)
        {
            return ProductList.Remove(GetShopItem(itemName));
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

        public ShopItem BuyShopItem(string itemName, User user)
        {
            if (GetShopItem(itemName) == null) return null;
            ShopProfit = GetShopItem(itemName).ItemPrice;
            var itemHistory = new ShopItemHistory(GetShopItem(itemName).ItemName, GetShopItem(itemName).ItemPrice, user.UserName);
            // ShopHistory.Add(itemHistory);
            ShopHistory.Insert(0,itemHistory);
            return GetShopItem(itemName);
        }

        // Class methods
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
        
        // Save Data Methods
        
        public void SaveData()
        {
            DataSerializer dataSerializer = new DataSerializer();
            dataSerializer.SaveData(this);
        }
        
        public void GetData()
        {
            DataSerializer dataSerializer = new DataSerializer();
            dataSerializer.GetData(this);
            if(_users.Count != 0)
                foreach (var user in _users)
                {
                    user.GetShop(this);
                }
        }

        // ILoader interface methods
        public List<ShopItemHistory> GetShopHistory()
        {
            return ShopHistory;
        }

        public List<User> GetUsers()
        {
            return _users;
        }

        public List<ShopItem> GetProductList()
        {
            return ProductList;
        }

        public double GetShopBalance()
        {
            return ShopBalance;
        }

        public void LoadData(List<ShopItemHistory> shopHistory, double shopBalance, List<User> users, List<ShopItem> productList)
        {
            if(shopHistory != null) ShopHistory = shopHistory;
            ShopBalance = shopBalance;
            if(users != null) _users = users;
            if(productList != null) ProductList = productList;
        }
    }
}