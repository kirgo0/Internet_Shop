using System;
using System.Collections.Generic;
using System.Threading;
using InternetShop.Data;
using InternetShop.Shop;
using InternetShop.Users;

namespace InternetShop
{
    public class ShopController
    {
        private OnlineShop _shop = new OnlineShop();
        private UserList _users;
        private ItemList _items;
        private User _user;
        private bool _signedIn = false;
        public ShopController(OnlineShop shop)
        {
            _shop = shop;
        }
        public ShopController(UserList users, ItemList items)
        {
            _users = users;
            _items = items;
        }

        public void Run()
        {
            while(true)
            {
                while (_user == null)
                {
                    PrintStartMenu();
                    string message = Console.ReadLine();
                    if(message == "Exit") return;
                    StartMessageHandler(message);
                    while (_signedIn)
                    {
                        if (_user == null)
                        {
                            PrintGuestMenu();
                        }
                        else
                        {
                            PrintShopMenu();
                        }
                        message = Console.ReadLine();
                        if(message == "Exit") return;
                        ShopMessageHandler(message);
                    }
                    if(message == "Exit") return;
                }
            }
        }
        private void StartMessageHandler(string message)
        {
            switch (message)
            {
                case "1" :
                {
                    _user = SignUp();
                    break;
                }
                case "2":
                {
                    _user = SignIn();
                    break;
                }
                case "3":
                {
                    _signedIn = true;
                    break;
                }
            }
        }

        private User SignUp()
        {
            string userName,password1;
            do
            {
                userName = GetAnswer("Write your login");
                password1 = GetAnswer("Write your password");
                var password2 = GetAnswer("Write your password again");
                if (password1 == password2)
                {
                    if (_shop.SignUp(userName, password1, password2) == null)
                    {
                        PrintMessage("This username are used already");
                        continue;
                    }
                    var user = _shop.SignIn(userName,password1);
                    PrintMessage(user.UserName +  " You successfully Signed Up!");
                    _signedIn = true;
                    return user;
                }
                PrintMessage("Written passwords are not same");
                if (LeaveQuestion())
                {
                    PrintMessage("Sign Up form canceled!");
                    _signedIn = false;
                    return null;
                }
            } while (_shop.SignIn(userName, password1) == null);
            return null;
        }

        private User SignIn()
        {
            string userName,password;
            do
            {
                userName = GetAnswer("Write your login");
                password = GetAnswer("Write your password");
                if (_shop.SignIn(userName, password) == null)
                {
                    PrintMessage("Username or password is wrong");
                    if (LeaveQuestion())
                    {
                        PrintMessage("Sign Ip form canceled!");
                        return null;
                    }
                }
                else
                {
                    var user = _shop.SignIn(userName,password);
                    PrintMessage(user.UserName +  " You successfully Signed In!");
                    _signedIn = true;
                    return user;
                }
            } while (_shop.SignIn(userName,password) == null);
            return null;
        }
        
        private void ShopMessageHandler(string message)
        {
            if (_user == null)
            {
                switch (message)
                {
                    case "1":
                    {
                        ProductListMenu(_shop.ProductList,"Shop product list");
                        break;
                    }
                    case "2":
                    {
                        FindProduct();
                        break;
                    }
                    case "3":
                    {
                        _signedIn = false;
                        break;
                    }
                }
            }
            else
            {
                switch (message)
                {
                    case "1":
                    {
                        BalanceMenu();
                        break;
                    }
                    case "2":
                    {
                        UserCartMenu();
                        break;
                    }
                    case "3":
                    {
                        PrintUserPurchase();
                        break;
                    }
                    case "4":
                    {
                        ProductListMenu(_shop.ProductList, "Shop product list");
                        break;
                    }
                    case "5":
                    {
                        FindProduct();
                        break;
                    }
                    case "6":
                    {
                        _user = null;
                        _signedIn = false;
                        break;
                    }
                }
            }
        }

        private void FindProduct()
        {
            List<ShopItem> findResult = new List<ShopItem>();
            bool isFinished = false;
            do
            {            
                PrintMessage("Print product name");
                var msg = Console.ReadLine();
                if (_shop.GetShopItem(msg) != null)
                {
                    ProductMenu(_shop.GetShopItem(msg));
                    isFinished = true;
                }
                else
                {
                    findResult = _shop.GetShopItems(msg.Split(' ')[0]);
                    if (findResult == null) continue;
                    ProductListMenu(findResult, "Find products by a key word " + '"' + msg.Split(' ')[0] + '"');
                    isFinished = true;
                }
            } while (!isFinished);
        }

        private void BalanceMenu()
        {
            bool isFinished = false;
            do
            {
                PrintMessage("Balance: " + _user.UserBalance + " UAH");
                InfoPrinter.PrintOneRow("1. Replenish the balance");
                InfoPrinter.PrintOneRow("You want to come back? (Y)");
                var msg = Console.ReadLine();
                if (msg.ToLower() == "y")
                {
                    isFinished = true;
                    continue;
                }
                if (msg == "1")
                {
                    msg = GetAnswer("Enter the amount you want to recharge");
                    try
                    {
                        Int32.Parse(msg);
                    }
                    catch (Exception e)
                    {
                        continue;
                    }
                    _user.UserBalance = Int32.Parse(msg);
                }
            } while (!isFinished);
        }

        private void ProductListMenu(List<ShopItem> list, string message)
        {
            bool isFinished = false;
            var pagesCount = list.Count / 6 + 1;
            var currentPage = 0;
            do
            {                    
                PrintMessage(message);
                if (pagesCount <= 1)
                {
                    PrintProductList(list);
                } else
                {
                    List<ShopItem> currentPageItems = new List<ShopItem>();
                    for (int i = currentPage*6; i < currentPage*6+6; i++)
                    {
                        try
                        {
                            currentPageItems.Add(list[i]);
                        }
                        catch (ArgumentOutOfRangeException e)
                        {
                            continue;
                            throw;
                        }
                    }
                    var bottomMenu = "Pages: ";
                    int leftBorder = currentPage, rightBorder = currentPage;
                    for (int i = 0; i < 3; i++)
                    {
                        if (leftBorder >= 0) leftBorder--;
                        else rightBorder++;
                        if (rightBorder < pagesCount) rightBorder++;
                        else leftBorder--;
                    }
                    if(leftBorder > 0) bottomMenu += "... ";
                    for (int i = leftBorder+1; i < rightBorder; i++)
                    {
                        if (i == currentPage) bottomMenu += "(" + (i + 1) + ") ";
                        else bottomMenu += (i + 1) + " ";
                    }
                    if(pagesCount > rightBorder+1) bottomMenu += "... " + pagesCount;
                    else if (pagesCount > rightBorder) bottomMenu += pagesCount;
                    PrintProductList(currentPageItems);
                    InfoPrinter.PrintOneRow(bottomMenu);
                }
                InfoPrinter.PrintOneRow("You want to come back? (Y)");
                var msg = Console.ReadLine();
                if (msg.ToLower() == "y")
                {
                    isFinished = true;
                    continue;
                }
                try
                {
                    Int32.Parse(msg);
                }
                catch (Exception e)
                {
                    continue;
                }
                var number = Int32.Parse(msg) - 1;
                if (number >= 0 && number < pagesCount)
                    currentPage = number;
            } while (!isFinished);
        }
        
        private string GetAnswer(string question)
        {
            PrintMessage(question);
            return Console.ReadLine();
        }
        
        private bool LeaveQuestion()
        {
            Thread.Sleep(500);
            InfoPrinter.PrintOneRow("You want to come back (Y) or try again? (Any key)");
            var msg = Console.ReadLine().ToLower();
            return msg == "y";
        }

        private void PrintMessage(string message)
        {
            Thread.Sleep(500);
            Console.Clear();
            InfoPrinter.PrintLine();
            InfoPrinter.PrintOneRow(message);
        }

        private void PrintStartMenu()
        {
            PrintMessage("1. Sign Up");
            InfoPrinter.PrintOneRow("2. Sign In");
            InfoPrinter.PrintOneRow("3. Sign In as guest");
        }

        private void PrintShopMenu()
        {
            PrintMessage("1. View Balance");
            InfoPrinter.PrintOneRow("2. View your cart");
            InfoPrinter.PrintOneRow("3. View your purchase history");
            InfoPrinter.PrintOneRow("4. View products");
            InfoPrinter.PrintOneRow("5. Find a product");
            InfoPrinter.PrintOneRow("6. Sign out");
        }
        
        private void PrintGuestMenu()
        {
            PrintMessage("1. View products");
            InfoPrinter.PrintOneRow("2. Find a product");
            InfoPrinter.PrintOneRow("3. Sign out");
        }

        private void UserCartMenu()
        {
            do
            {
                PrintMessage("Your cart");
                if (_user.Cart.Count != 0)
                {
                    foreach (var item in _user.Cart)
                    {
                        InfoPrinter.PrintOneRow("Product: " + item.ItemName + " Price: " + item.ItemPrice + " UAH");
                    }
                }
                else
                {
                    InfoPrinter.PrintOneRow("Your cart as empty");
                }
            } while(!LeaveQuestion());
        }

        private void ProductMenu(ShopItem shopItem)
        {
            bool isFinished = false;
            do
            {
                var item = (ShopItemExtended) shopItem;
                PrintMessage(item.ItemName);
                InfoPrinter.PrintPriceRow(item.ItemPrice);
                InfoPrinter.PrintRow("Description");
                InfoPrinter.PrintRow("");
                var i = 0;
                var description = item.ItemDescription.Split(' ');
                var line = "";
                foreach (var word in description)
                {
                    i += word.Length;
                    if (i > InfoPrinter.TableWidth - InfoPrinter.TableWidth / 4)
                    {
                        i = 0;
                        InfoPrinter.PrintRow(line);
                        line = "";
                    }
                    else
                    {
                        line += word + " ";
                        i++;
                    }
                }
                InfoPrinter.PrintOneRow("");
                if (_user != null)
                {
                    InfoPrinter.PrintOneRow("1. Add item to a cart");
                    InfoPrinter.PrintOneRow("2. Buy item");
                    InfoPrinter.PrintOneRow("You want to come back? (Y)");
                    var msg = Console.ReadLine();
                    if (msg.ToLower() == "y")
                    { 
                        isFinished = true;
                        continue;
                    }
                    if (msg == "1")
                    {
                        var result = _user.AddShopItemToCart(item.ItemName);
                        if (result)
                        {
                            PrintMessage(item.ItemName + " successfully added to your cart!");
                        }
                    }
                }
                else
                {
                    InfoPrinter.PrintOneRow("You want to come back? (Y)");
                    var msg = Console.ReadLine();
                    if (msg.ToLower() == "y")
                    { 
                        isFinished = true;
                    }
                }
            } while (!isFinished);
        }
        
        private void PrintUserPurchase()
        {
            do
            {
                PrintMessage("Your purchase history");
                if (_user.Cart.Count != 0)
                {
                    foreach (var item in _user.PurchaseHistory)
                    {
                        InfoPrinter.PrintOneRow("Product: " + item.ItemName + " Price: " + item.ItemPrice + " UAH");
                    }
                }
                else
                {
                    InfoPrinter.PrintOneRow("You haven't bought anything yet(");
                }
            } while(!LeaveQuestion());
        }

        private void PrintProductList(List<ShopItem> list)
        {
            var count = list.Count;
            if (count == 0) return;
            for (var i = 0; i < count; i+= 3)
            {
                if (count - i >= 3)
                {
                    InfoPrinter.PrintOneRow("#" + (i+1),"#" + (i+2),"#" + (i+3));
                    InfoPrinter.PrintOneRow(list[i].ItemName, list[i+1].ItemName, list[i+2].ItemName);
                    InfoPrinter.PrintPriceRow(list[i].ItemPrice, list[i+1].ItemPrice, list[i+2].ItemPrice);
                }
                else
                {
                    string[] itemNames;
                    double[] itemPrices;
                    if(count - i == 2)
                    {
                        itemNames = new string[]
                            { list[i].ItemName, list[i + 1].ItemName, "" };
                        itemPrices = new[]
                            { list[i].ItemPrice, list[i + 1].ItemPrice, 0 };
                    }
                    else
                    {
                        itemNames = new[]
                            { list[i].ItemName, "", "" };
                        itemPrices= new[]
                            { list[i].ItemPrice, 0, 0 };
                    }
                    InfoPrinter.PrintOneRow(i+1 <= count ? "#" + (i+1): "",
                        i+2 <= count ? "#" + (i+2): "",
                        i+3 <= count ? "#" + (i+3): "");
                    InfoPrinter.PrintOneRow(itemNames[0], itemNames[1], itemNames[2]);
                    InfoPrinter.PrintPriceRow(itemPrices[0], itemPrices[1], itemPrices[2]);
                }
            }
        }
        
    }
}