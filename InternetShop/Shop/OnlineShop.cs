using System.Collections.Generic;
using System.IO;
using InternetShop.Data;
using InternetShop.Users;
using Newtonsoft.Json;

namespace InternetShop.Shop
{
    public class OnlineShop : IShop
    {
        private List<User> _users = new List<User>();
        public List<ShopItem> ProductList = new List<ShopItem>();
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

        public ShopItem BuyShopItem(string itemName)
        {
            if (GetShopItem(itemName) == null) return null;
            ShopProfit = GetShopItem(itemName).ItemPrice;
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
            var userList = new UserData(_users);
            var itemList = new ItemData(ProductList);
            var dataJson = JsonConvert.SerializeObject(userList, Formatting.Indented, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            });
            File.WriteAllText("users.json", dataJson);
            dataJson = JsonConvert.SerializeObject(itemList, Formatting.Indented, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            });
            File.WriteAllText("items.json", dataJson);
        }
        
        public void GetData()
        {
            try
            {
                var data = File.ReadAllText("users.json");
                var userList = JsonConvert.DeserializeObject<UserData>(data, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto
                });
                if (userList == null) return;
                _users = userList.List;
                foreach (var user in _users)
                {
                    user.GetShop(this);
                }
                // product loader
                data = File.ReadAllText("items.json");
                var itemList = JsonConvert.DeserializeObject<ItemData>(data, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto
                });
                if(itemList == null) return;
                ProductList = itemList.List;
            }
            catch (IOException e)
            {
                _users = new List<User>();
            }
        }


    }
}