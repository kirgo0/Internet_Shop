using System;
using System.Collections.Generic;
using System.Threading;
using InternetShop.Users;

namespace InternetShop.Shop
{
    public class OnlineShop : IShop
    {
        private const int TableWidth = 150;
        private readonly List<User> _users = new List<User>();
        private readonly List<ShopItem> _productList = new List<ShopItem>();
        private double _shopBalance = 0;

        private double ShopProfit
        {
            get => _shopBalance;
            set => _shopBalance += value * 0.1;
        }

        // Interface methods
        public void CreateNewShopItem(string itemName, double itemPrice, string itemDescription)
        {
            if(GetShopItem(itemName) == null) _productList.Add(new ShopItemExtended(itemName,itemPrice,itemDescription));
        }

        public void DeleteShopItem(string itemName)
        {
            _productList.Remove(GetShopItem(itemName));
        }

        public ShopItem GetShopItem(string itemName)
        {
            foreach (var item in _productList)
            {
                if (item.ItemName == itemName) return item;
            }
            return null;
        }

        public ShopItem BuyShopItem(string itemName)
        {
            if (GetShopItem(itemName) != null)
            {
                ShopProfit = GetShopItem(itemName).ItemPrice;
                return GetShopItem(itemName);
            }
            return null;
        }

        // Class methods

        public void LoadUsersList()
        {
            
        }

        public Admin SignUpAdmin(string userName, string password, string passwordRepeat)
        {
            if (GetUser(userName) == null)
            {
                if (password == passwordRepeat)
                {
                    Admin newUser = new Admin(userName, password);
                    _users.Add(newUser);
                    newUser.GetShop(this);     
                    Console.WriteLine("Admin successfully signUp");
                    return newUser;
                }
            }
            return null;
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
                    Console.WriteLine("You successfully signUp");
                    return newUser;
                }
            }
            return null;
        }
        public User SignIn(string userName, string password)
        {
            if(GetUser(userName) != null)
                if (GetUser(userName).SignIn(userName, password))
                {
                    GetUser(userName).GetShop(this);                   
                    Console.WriteLine("You successfully login");
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
            Thread.Sleep(1000);
            int count = _productList.Count;
            if (count >= 3)
            {
                Console.Clear();
                for (int i = 0; i < count; i+= 3)
                {
                    PrintLine();
                    if (count - i >= 3)
                    {
                        PrintRow("#" + i,"#" + (i+1),"#" + (i+2));
                        PrintLine();
                        PrintRow(_productList[i].ItemName, _productList[i+1].ItemName, _productList[i+2].ItemName);
                        PrintLine();
                        PrintPriceRow(_productList[i].ItemPrice.ToString(), _productList[i+1].ItemPrice.ToString(), _productList[i+2].ItemPrice.ToString());
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
                        PrintRow("#" + i,"#" + (i+1),"#" + (i+2));
                        PrintLine();
                        PrintRow(itemNames[0], itemNames[1], itemNames[2]);
                        PrintLine();
                        PrintPriceRow(itemPrices[0], itemPrices[1], itemPrices[2]);
                        PrintLine();
                    }
                }
            }
            
        }
        
        private void PrintLine()
        {
            Console.WriteLine(new string('-', TableWidth));
        }

        private void PrintRow(params string[] columns)
        {
            int width = (TableWidth - columns.Length) / columns.Length;
            string row = "|";

            foreach (string column in columns)
            {
                row += AlignCentre(column, width) + "|";
            }

            Console.WriteLine(row);
        }
        
        private void PrintPriceRow(params string[] columns)
        {
            int width = (TableWidth - columns.Length) / columns.Length;
            string row = "|";

            foreach (string column in columns)
            {
                row += AlignCentre((column.Length > 0 ? " Price:" : "") + column + (column.Length > 0 ? " UAH" : ""), width) + "|";
            }

            Console.WriteLine(row);
        }

        private string AlignCentre(string text, int width)
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