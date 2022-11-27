using System;
using System.Collections.Generic;
using InternetShop.Users;

namespace InternetShop.Shop
{
    public class OnlineShop : IShop
    {
        private const int TableWidth = 150;
        private readonly List<User> _users = new List<User>();
        private readonly List<ShopItem> _productList = new List<ShopItem>();

        // Interface methods
        public void CreateNewShopItem(string itemName, double itemPrice)
        {
            if(SearchShopItem(itemName) == null) _productList.Add(new ShopItem(itemName,itemPrice));
        }

        public void DeleteShopItem(string itemName)
        {
            _productList.Remove(SearchShopItem(itemName));
        }

        public ShopItem SearchShopItem(string itemName)
        {
            foreach (var item in _productList)
            {
                if (item.ItemName == itemName) return item;
            }
            return null;
        }

        // Class methods

        public void LoadUsersList()
        {
            
        }

        public User SignUp(string userName, string password, string passwordRepeat)
        {
            if (GetUser(userName) == null)
            {
                if (password == passwordRepeat)
                {
                    User newUser = new DefaultUser(userName, password);
                    _users.Add(newUser);
                    newUser.GetShop(this);
                    return newUser;
                }
            }
            return null;
        }
        public User Login(string userName, string password)
        {
            if(GetUser(userName) != null)
                if (GetUser(userName).Login(userName, password))
                {
                    GetUser(userName).GetShop(this);
                    return GetUser(userName);
                }
            return null;
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
        public void PrintProductList()
        {
            int count = _productList.Count;
            if (count >= 3)
            {
                Console.Clear();
                for (int i = 0; i < count; i+= 3)
                {
                    PrintLine();
                    if (count - i >= 3)
                    {
                        PrintRow(_productList[i].ItemName, _productList[i+1].ItemName, _productList[i+2].ItemName);
                        PrintLine();
                        PrintRow(_productList[i].ItemPrice.ToString(), _productList[i+1].ItemPrice.ToString(), _productList[i+2].ItemPrice.ToString());
                        PrintLine();
                    }
                    else
                    {
                        string[] itemNames, itemPrices;
                        if(count - i == 2)
                        {
                            itemNames = new string[]
                                { _productList[i].ItemName, _productList[i + 1].ItemName, "" };
                            itemPrices = new[]
                                { _productList[i].ItemPrice.ToString(), _productList[i + 1].ItemPrice.ToString(), "" };
                        }
                        else
                        {
                            itemNames = new[]
                                { _productList[i].ItemName, "", "" };
                            itemPrices= new[]
                                { _productList[i].ItemPrice.ToString(), "", "" };
                        }
                        PrintRow(itemNames[0], itemNames[1], itemNames[2]);
                        PrintLine();
                        PrintRow(itemPrices[0], itemPrices[1], itemPrices[2]);
                        PrintLine();
                    }
                }
            }
            
        }
        
        static void PrintLine()
        {
            Console.WriteLine(new string('-', TableWidth));
        }

        static void PrintRow(params string[] columns)
        {
            int width = (TableWidth - columns.Length) / columns.Length;
            string row = "|";

            foreach (string column in columns)
            {
                row += AlignCentre(column, width) + "|";
            }

            Console.WriteLine(row);
        }

        static string AlignCentre(string text, int width)
        {
            text = text.Length > width ? text.Substring(0, width - 3) + "..." : text;

            if (string.IsNullOrEmpty(text))
            {
                return new string(' ', width);
            }
            else
            {
                return text.PadRight(width - (width - text.Length) / 2).PadLeft(width);
            }
        }
    }
}