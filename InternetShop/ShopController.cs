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
        private readonly OnlineShop _shop;
        private User _user;
        private bool _signedIn;
        private bool _adminMode;
        private IAdmin _admin;
        public ShopController(OnlineShop shop)
        {
            _shop = shop;
        }

        private void StartMessageHandler(string message)
        {
            switch (message.Trim())
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

        public void Run()
        {
            _shop?.GetData();
            while(true)
            {
                while (_user == null)
                {
                    PrintStartMenu();
                    string message = Console.ReadLine();
                    if(message?.ToLower().Trim() == "exit")
                    {            
                        _shop?.SaveData();
                        return;
                    }
                    StartMessageHandler(message);
                    while (_signedIn)
                    {
                        if (_adminMode)
                        {
                            PrintAdminMenu();
                            message = Console.ReadLine();
                            if(MessageFormatter(message) == "exit") return;
                            AdminMessageHandler(message);
                            continue;
                        }
                        if (_user == null)
                        {
                            PrintGuestMenu();
                        }
                        else 
                        {
                            PrintDefaultMenu();
                        }
                        message = Console.ReadLine();
                        if(message?.ToLower().Trim() == "exit") return;
                        ShopMessageHandler(message);
                    }
                    if (message?.ToLower().Trim() == "exit")
                    {
                        _shop?.SaveData();
                        return;
                    }
                }
            }
        }

        private User SignUp()
        {
            do
            {
                var userName = GetAnswer("Write your login");
                var password1 = GetAnswer("Write your password");
                var password2 = GetAnswer("Write your password again");
                if (userName.Contains(" ") || password1.Contains(" ") || password2.Contains(" "))
                {
                    PrintMessage("Username and password must not contain spaces", 1500);
                    continue;
                }

                if (password1 == password2)
                {
                    if (_shop.SignUp(userName, password1, password2) == null)
                    {
                        PrintMessage("This username are used already", 1500);
                        if (LeaveQuestion())
                        {
                            PrintMessage("Sign Up form is closed!", 1500);
                            _signedIn = false;
                            return null;
                        }

                        continue;
                    }

                    var user = _shop.SignIn(userName, password1);
                    PrintMessage(user.UserName + " you successfully Signed Up!", 1500);
                    _signedIn = true;
                    return user;
                }

                PrintMessage("Written passwords are not same", 1500);
                if (LeaveQuestion())
                {
                    PrintMessage("Sign Up form is closed!", 1500);
                    _signedIn = false;
                    return null;
                }
            } while (true);
        }

        private User SignIn()
        {
            string userName,password;
            do
            {
                userName = GetAnswer("Write your login");
                password = GetAnswer("Write your password");
                if (userName.Contains(" ") || password.Contains(" "))
                {
                    PrintMessage("Username and password must not contain spaces", 1500);
                    continue;
                }
                if (_shop.SignIn(userName, password) == null)
                {
                    PrintMessage("Username or password is wrong",1500);
                    if (LeaveQuestion())
                    {
                        PrintMessage("Sign In form is closed!",1500);
                        return null;
                    }
                }
                else
                {
                    var user = _shop.SignIn(userName,password);
                    if (user.CheckAccountType == AccountType.Admin)
                    {
                        _adminMode = true;
                        _admin = (IAdmin) user;
                        PrintMessage("Admin " + user.UserName +  " you are successfully Signed In!",1500);
                    }
                    else
                    {
                        _adminMode = false;
                        _admin = null;
                        PrintMessage(user.UserName +  " you are successfully Signed In!",1500);
                    }
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
                switch (message.Trim())
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
                switch (message.Trim())
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

        private void AdminMessageHandler(string message)
        {
            if (!_adminMode) return;
            switch (message.Trim())
            {
                case "1":
                {
                    ShopBalanceMenu();
                    break;
                }
                case "2":
                {
                    ProductListMenu(_shop.ProductList, "Shop product list");
                    break;
                }
                case "3":
                {
                    FindProduct();
                    break;
                }
                case "4":
                {
                    CreateNewProductMenu();
                    break;
                }
                case "5":
                {
                    _user = null;
                    _adminMode = false;
                    _admin = null;
                    _signedIn = false;
                    break;
                }
            }
        }
        
        private void FindProduct()
        {
            while (true)
            {            
                PrintMessage("Print product name");
                var msg = Console.ReadLine();
                if (_shop.GetShopItem(msg) != null)
                {
                    ProductMenu(_shop.GetShopItem(msg));
                    return;
                }
                var findResult = _shop.GetShopItems(msg?.Split(' ')[0]);
                if (findResult.Count == 0)
                {
                    PrintMessage("Failed to find products by the " + '"' + msg + '"' + " keyword" , 1500);
                    return;
                }
                ProductListMenu(findResult, "Find products by a key word " + '"' + msg + '"');
                return;
            }
        }

        private void BalanceMenu()
        {
            do
            {
                PrintMessage("Balance: " + _user.UserBalance + " UAH");
                InfoPrinter.PrintOneRow("1. Replenish the balance");
                InfoPrinter.PrintOneRow("Do you want to come back? (Y)");
                var msg = Console.ReadLine();
                if (MessageFormatter(msg) == "y")
                {
                    return;
                }
                if (msg?.Trim() != "1") continue;
                msg = GetAnswer("Enter the amount you want to recharge");
                try
                {
                    if (int.Parse(msg) > 0)
                    {
                        _user.UserBalance = int.Parse(msg);
                    }
                    else
                    {
                        PrintMessage("Enter a positive non-zero number", 1500);
                    }
                }
                catch (Exception)
                {
                    PrintMessage("The entered value must be an integer");
                }
            } while (true);
        }

        private void ProductListMenu(List<ShopItem> list, string message)
        {
            var pagesCount = list.Count % 6 == 0 ? list.Count / 6 : list.Count / 6 + 1;
            var currentPage = 0;
            do
            {                    
                PrintMessage(message);
                List<ShopItem> currentPageItems = new List<ShopItem>();
                if (pagesCount <= 1)
                {
                    PrintProductList(list);
                    currentPageItems = list;
                } else
                {
                    for (var i = currentPage*6; i < currentPage*6+6; i++)
                    {
                        try
                        {
                            currentPageItems.Add(list[i]);
                        }
                        catch (ArgumentOutOfRangeException)
                        {
                        }
                    }
                    var bottomMenu = "Pages: ";
                    int leftBorder = currentPage, rightBorder = currentPage;
                    for (var i = 0; i < 3; i++)
                    {
                        if (leftBorder >= 0) leftBorder--;
                        else if (rightBorder < pagesCount) rightBorder++;
                        if (rightBorder < pagesCount) rightBorder++;
                        else if (leftBorder >= 0) leftBorder--;
                    }
                    if(leftBorder >= 0) bottomMenu += "... ";
                    for (var i = leftBorder+1; i < rightBorder; i++)
                    {
                        if (i == currentPage) bottomMenu += "(" + (i + 1) + ") ";
                        else bottomMenu += (i + 1) + " ";
                    }
                    if(pagesCount > rightBorder+1) bottomMenu += "... " + pagesCount;
                    else if (pagesCount > rightBorder) bottomMenu += pagesCount;
                    PrintProductList(currentPageItems);
                    InfoPrinter.PrintOneRow(bottomMenu);
                }
                InfoPrinter.PrintOneRow("Type #(number of product) for exact product information");
                InfoPrinter.PrintOneRow("Do you want to come back? (Y)");
                var msg = Console.ReadLine();
                if (msg == null) continue;
                if (MessageFormatter(msg) == "y")
                {
                    return;
                }
                if (msg.StartsWith("#") && msg.Length == 2)
                {
                    var productNumber = msg.Remove(0, 1);
                    try
                    {
                        if (int.Parse(productNumber) - 1 >= 0 && int.Parse(productNumber) - 1 < 6)
                        {
                            try
                            {
                                if (ProductMenu(currentPageItems[int.Parse(productNumber) - 1])) return;
                            }
                            catch (ArgumentOutOfRangeException)
                            {
                                continue;
                            }
                        }
                    }
                    catch (FormatException)
                    {
                        continue;
                    }
                }
                try
                {
                    var number = int.Parse(msg) - 1;
                    if (number >= 0 && number < pagesCount)
                        currentPage = number;
                }
                catch (Exception)
                {
                    // ignored
                }
            } while (true);
        }
        
        private void UserCartMenu()
        {
            do
            {
                _user.CheckCartProducts();
                PrintMessage("Your cart");
                if (_user.Cart.Count != 0)
                {
                    foreach (var item in _user.Cart)
                    {
                        InfoPrinter.PrintOneRow("Product: " + item.ItemName + " Price: " + item.ItemPrice + " UAH");
                    }
                    InfoPrinter.PrintOneRow("1. Buy all products from cart");
                    InfoPrinter.PrintOneRow("2. Remove a product from the cart");
                    InfoPrinter.PrintOneRow("Do you want to come back? (Y)");
                    var msg = Console.ReadLine();
                    if (MessageFormatter(msg) == "y")
                    {
                        return;
                    }
                    if (msg?.Trim() == "1")
                    {
                        PrintMessage("Are you sure you want to buy all products from your cart? (Y) to confirm (any key) to come back");
                        msg = Console.ReadLine();
                        if(MessageFormatter(msg) != "y") continue;
                        if (_user.BuyItemsFromCart())
                        {
                            PrintMessage("All products were successfully purchased",1500);
                        }
                        else
                        {
                            PrintMessage("You don't have enough money on your balance to buy all products from the cart");
                            InfoPrinter.PrintRow("");
                            InfoPrinter.PrintOneRow("Your cart");
                            for (var i = 0; i < _user.Cart.Count; i++)
                            {
                                var item = _user.Cart[i];
                                InfoPrinter.PrintOneRow("#" + (i + 1) + " Product: " + item.ItemName + " Price: " + item.ItemPrice + " UAH");
                            }
                            InfoPrinter.PrintRow("");
                            InfoPrinter.PrintOneRow("Type #(number) to remove a product from the cart");
                            InfoPrinter.PrintOneRow("1. Replenish balance");
                            InfoPrinter.PrintOneRow("Do you want to come back? (Y)");
                            msg = Console.ReadLine();
                            if(msg == null) continue;
                            if (MessageFormatter(msg) == "y")
                            {
                                continue;
                            }
                            if (msg.Trim() == "1")
                            {
                                BalanceMenu();
                                continue;
                            }
                            if (msg.StartsWith("#"))
                            {
                                var listNumber = msg.Remove(0, 1);
                                try
                                {
                                    var number = int.Parse(listNumber);
                                    if (_user.RemoveShopItemFromCart(number-1))
                                    {
                                        PrintMessage("This product was removed from your cart",1500);
                                    }
                                    else
                                    {
                                        PrintMessage("You entered a wrong cart product number",1000);
                                    }
                                }
                                catch (FormatException)
                                {
                                    continue;
                                }
                            }
                        }
                    }
                    if (msg?.Trim() != "2") continue;
                    do
                    {
                        PrintMessage("Your cart");
                        for (var i = 0; i < _user.Cart.Count; i++)
                        {
                            var item = _user.Cart[i];
                            InfoPrinter.PrintOneRow("#" + (i + 1) + " Product: " + item.ItemName + " Price: " +
                                                    item.ItemPrice + " UAH");
                        }

                        InfoPrinter.PrintRow("");
                        InfoPrinter.PrintOneRow("Type product (#number) which you want to remove from the cart");
                        InfoPrinter.PrintOneRow("Do you want to come back? (Y)");
                        msg = Console.ReadLine();
                        if (msg == null) continue;
                        if (MessageFormatter(msg) == "y") break;
                        if (!msg.StartsWith("#")) continue;
                        var listNumber = msg.Remove(0, 1);
                        try
                        {
                            var number = int.Parse(listNumber);
                            if (_user.RemoveShopItemFromCart(number - 1))
                            {
                                PrintMessage("This item was removed from your cart", 1500);
                                break;
                            }
                            PrintMessage("You entered a wrong cart item number", 1500);
                        }
                        catch (FormatException)
                        {
                        }
                    } while (true);
                }
                else
                {
                    InfoPrinter.PrintOneRow("Your cart is empty");
                    InfoPrinter.PrintOneRow("Do you want to come back? (Y)");
                    var msg = Console.ReadLine();
                    if (MessageFormatter(msg) == "y") return;
                }
            } while(true);
        }

        private bool ProductMenu(ShopItem shopItem)
        {
            var isFinished = false;
            do
            {
                var item = (ShopItemExtended) shopItem;
                PrintMessage(item.ItemName);
                InfoPrinter.PrintPriceRow(item.ItemPrice);
                PrintProductDescription(item.ItemDescription);
                if (_user != null && _adminMode)
                {
                    InfoPrinter.PrintOneRow("1. Change a product");
                    InfoPrinter.PrintOneRow("2. Remove a product");
                    InfoPrinter.PrintOneRow("Do you want to come back? (Y)");
                    var msg = Console.ReadLine();
                    if(msg == null) continue;
                    if (MessageFormatter(msg) == "y")
                    { 
                        isFinished = true;
                        continue;
                    }
                    switch (msg.Trim())
                    {
                        case "1":
                            ChangeProductMenu(item);
                            isFinished = true;
                            break;
                        case "2":
                        {
                            PrintMessage("Are you sure you want to remove this product from shop product list?");
                            InfoPrinter.PrintOneRow("(Y) to confirm removing (any key) to come back");
                            msg = Console.ReadLine();
                            if (MessageFormatter(msg) == "y")
                            {
                                if (_admin.RemoveItem(item.ItemName))
                                {
                                    PrintMessage("This product was successfully removed",1500);
                                    return true;
                                }
                                PrintMessage("Unexpected error, removing operation closed",1500);
                                return false;
                            }
                            break;
                        }
                    }
                }
                else if (_user != null)
                {
                    InfoPrinter.PrintOneRow("1. Add a product to a cart");
                    InfoPrinter.PrintOneRow("2. Buy a product");
                    InfoPrinter.PrintOneRow("Do you want to come back? (Y)");
                    var msg = Console.ReadLine();
                    if(msg == null) continue;
                    if (MessageFormatter(msg) == "y")
                    { 
                        isFinished = true;
                        continue;
                    }
                    switch (msg.Trim())
                    {
                        case "1" when _user.AddShopItemToCart(item.ItemName):
                            PrintMessage(item.ItemName + " was successfully added to your cart!",1500);
                            break;
                        case "1":
                            PrintMessage("Unexpected error, form closed",1500);
                            break;
                        case "2" when _user.UserBalance >= item.ItemPrice:
                        {
                            if (GetAnswer("You sure you want to buy a " + item.ItemName + "? (Y) to confirm").ToLower() == "y")
                            {
                                if(_user.BuyItem(item.ItemName)) PrintMessage("You successfully buy a " + item.ItemName,1500);
                                else PrintMessage("Unexpected error, form closed",1500);
                            }
                            break;
                        }
                        case "2":
                            PrintMessage("You don't have enough money on your balance to make a purchase",1500);
                            break;
                    }
                }
                else
                {
                    InfoPrinter.PrintOneRow("Do you want to come back? (Y)");
                    var msg = Console.ReadLine();
                    if (MessageFormatter(msg) == "y") isFinished = true;
                }
            } while (!isFinished);
            return false;
        }

        private void CreateNewProductMenu()
        {
            PrintMessage("You sure you want to create new product? (Y) to continue (any key) to come back");
            var msg = Console.ReadLine();
            if (MessageFormatter(msg) != "y") return;
            string itemName ="", itemDescription = "";
            var itemPrice = 0;
            var isFinished = false;
            do
            {
                PrintMessage("New product");
                InfoPrinter.PrintOneRow("Name: " + itemName, " Price: " + itemPrice + " UAH");
                PrintProductDescription(itemDescription.Trim());
                if (itemName?.Length == 0)
                {
                    InfoPrinter.PrintOneRow("Enter product name");
                    itemName = Console.ReadLine();
                }
                else if (itemPrice == 0)
                {
                    InfoPrinter.PrintOneRow("Enter product price");
                    msg = Console.ReadLine();
                    if(msg == null) continue;
                    try
                    {
                        if (int.Parse(msg) <= 0)
                        {
                            PrintMessage("Price might be a positive not zero integer!", 1500);
                            continue;
                        }
                        itemPrice = int.Parse(msg);
                    }
                    catch (FormatException)
                    {
                        PrintMessage("Price might be a number!", 1500);
                    }
                }
                else if (itemDescription.Length == 0)
                {
                    InfoPrinter.PrintOneRow("Enter product description");
                    do
                    {
                        msg = Console.ReadLine();
                        if(msg != "") itemDescription += msg?.Trim() + " \n ";
                    } while (msg != "");
                }
                else
                {
                    InfoPrinter.PrintOneRow("1. Create new product");
                    InfoPrinter.PrintOneRow("2. Change product name");
                    InfoPrinter.PrintOneRow("3. Change product price");
                    InfoPrinter.PrintOneRow("4. Change product description");
                    InfoPrinter.PrintOneRow("Do you want to come back? (Y)");
                    msg = Console.ReadLine();
                    if(msg == null) continue;
                    if (MessageFormatter(msg) == "y") isFinished = true;
                    switch (msg.Trim())
                    {
                        case "1" when _admin.CreateNewItem(itemName, itemPrice, itemDescription):
                            PrintMessage("New product was successfully created!", 1500);
                            isFinished = true;
                            break;
                        case "1":
                            PrintMessage("A product with this name already exists");
                            break;
                        case "2":
                            itemName = "";
                            continue;
                        case "3":
                            itemPrice = 0;
                            continue;
                        case "4":
                            itemDescription = "";
                            break;
                    }
                }
            } while (!isFinished);
        }

        private void ChangeProductMenu(ShopItem shopItem)
        {
            var item = (ShopItemExtended)shopItem;
            string itemName = item.ItemName, itemDescription = item.ItemDescription;
            var itemPrice = item.ItemPrice;
            var isFinished = false;
            do
            {
                PrintMessage("Product changing menu");
                InfoPrinter.PrintOneRow("Name: " + itemName, " Price: " + itemPrice + " UAH");
                PrintProductDescription(itemDescription);
                string msg;
                if (itemName?.Length == 0)
                {
                    InfoPrinter.PrintOneRow("Enter product name");
                    itemName = Console.ReadLine();
                }
                else if (itemPrice == 0)
                {
                    InfoPrinter.PrintOneRow("Enter the price of the product");
                    msg = Console.ReadLine();
                    if(msg == null) continue;
                    try
                    {
                        if (int.Parse(msg) <= 0)
                        {
                            PrintMessage("The price must be a positive non-zero number!", 1500);
                            continue;
                        }
                        itemPrice = int.Parse(msg);
                    }
                    catch (FormatException)
                    {
                        PrintMessage("The price should be a number!", 1500);
                    }
                }
                else if (itemDescription.Length == 0)
                {
                    InfoPrinter.PrintOneRow("Enter product description");
                    do
                    {
                        msg = Console.ReadLine();
                        itemDescription += msg?.Replace("\n", " ") + " ";
                    } while (msg != "");
                }
                else
                {
                    InfoPrinter.PrintOneRow("1. Change product name");
                    InfoPrinter.PrintOneRow("2. Change the price of the product");
                    InfoPrinter.PrintOneRow("3. Change product description");
                    InfoPrinter.PrintOneRow("4. Save all changes");
                    InfoPrinter.PrintOneRow("Do you want to come back? (Y)");
                    msg = Console.ReadLine();
                    if(msg == null) continue;
                    if (MessageFormatter(msg) == "y") isFinished = true;
                    switch (msg.Trim())
                    {
                        case "1":
                            itemName = "";
                            break;
                        case "2":
                            itemPrice = 0;
                            break;
                        case "3":
                            itemDescription = "";
                            break;
                        case "4" when _admin.ChangeShopItem(item.ItemName, itemName, itemPrice, itemDescription):
                            PrintMessage("This product was successfully changed!",1500);
                            break;
                        case "4":
                            PrintMessage("Unexpected error product change form closed",1500);
                            break;
                    }
                }
            } while (!isFinished);
        }

        private void ShopBalanceMenu()
        {
            var list = _shop.ShopHistory;
            var pagesCount = list.Count % 6 == 0 ? list.Count / 6 : list.Count / 6 + 1;
            var currentPage = 0;
            do
            {
                PrintMessage("Shop balance: " + Math.Round(_shop.ShopProfit,2) + " UAH");
                InfoPrinter.PrintOneRow("");
                var currentPageItems = new List<ShopItemHistory>();
                InfoPrinter.PrintOneRow("Recent purchases");
                if (pagesCount <= 1)
                {
                    foreach (var item in list)
                        InfoPrinter.PrintOneRow("Product: " + item.ItemName + " Price: " + item.ItemPrice + " UAH","Buyer: " + item.UserName);
                } 
                else
                {
                    for (var i = currentPage*6; i < currentPage*6+6; i++)
                    {
                        try
                        {
                            currentPageItems.Add(list[i]);
                        }
                        catch (ArgumentOutOfRangeException)
                        {
                            // ignored
                        }
                    }
                    var bottomMenu = "Pages: ";
                    int leftBorder = currentPage, rightBorder = currentPage;
                    for (var i = 0; i < 3; i++)
                    {
                        if (leftBorder >= 0) leftBorder--;
                        else if (rightBorder < pagesCount) rightBorder++;
                        if (rightBorder < pagesCount) rightBorder++;
                        else if (leftBorder >= 0) leftBorder--;
                    }
                    if(leftBorder >= 0) bottomMenu += "... ";
                    for (var i = leftBorder+1; i < rightBorder; i++)
                    {
                        if (i == currentPage) bottomMenu += "(" + (i + 1) + ") ";
                        else bottomMenu += (i + 1) + " ";
                    }
                    if(pagesCount > rightBorder+1) bottomMenu += "... " + pagesCount;
                    else if (pagesCount > rightBorder) bottomMenu += pagesCount;
                    foreach (var item in currentPageItems)
                    {
                        InfoPrinter.PrintOneRow("Product: " + item.ItemName + " Price: " + item.ItemPrice + " UAH","Buyer: " + item.UserName);
                    }
                    InfoPrinter.PrintOneRow(bottomMenu);
                }
                InfoPrinter.PrintOneRow("Do you want to come back? (Y)");
                var msg = Console.ReadLine();
                if(msg == null) continue;
                if (MessageFormatter(msg) == "y")
                {
                    return;
                }
                try
                {
                    var number = int.Parse(msg) - 1;
                    if (number >= 0 && number < pagesCount)
                        currentPage = number;
                }
                catch (Exception)
                {
                    // ignored
                }
            } while (true);
        }
        
        private string GetAnswer(string question)
        {
            PrintMessage(question);
            return Console.ReadLine()?.Trim();
        }
        
        private bool LeaveQuestion()
        {
            InfoPrinter.PrintOneRow("Do you want to come back (Y) or try again? (Any key)");
            var msg = Console.ReadLine()?.ToLower();
            return msg?.Trim() == "y";
        }

        private string MessageFormatter(string msg)
        {
            return msg.ToLower().Trim();
        }
        private void PrintMessage(string message, int delay)
        {
            PrintMessage(message);
            if (delay < 0)
            {
                return;
            }
            Thread.Sleep(delay);
        }
        
        private void PrintMessage(string message)
        {
            Console.Clear();
            InfoPrinter.PrintLine();
            InfoPrinter.PrintOneRow(message);
            _shop.SaveData();
        }

        private void PrintStartMenu()
        {
            PrintMessage("1. Sign Up");
            InfoPrinter.PrintOneRow("2. Sign In");
            InfoPrinter.PrintOneRow("3. Sign In as guest");
            InfoPrinter.PrintOneRow("Exit the program (Exit)");
        }

        private void PrintDefaultMenu()
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

        private void PrintAdminMenu()
        {
            PrintMessage("1. View shop balance");
            InfoPrinter.PrintOneRow("2. View products");
            InfoPrinter.PrintOneRow("3. Find a product");
            InfoPrinter.PrintOneRow("4. Create a new product");
            InfoPrinter.PrintOneRow("5. Sign out");
        }

        private void PrintUserPurchase()
        {
            do
            {
                PrintMessage("Your purchase history");
                if (_user.PurchaseHistory.Count != 0)
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
                    int[] itemPrices;
                    if(count - i == 2)
                    {
                        itemNames = new[]
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

        private void PrintProductDescription(string itemDescription)
        {
            InfoPrinter.PrintRow("Description");
            InfoPrinter.PrintRow("");
            var i = 0;
            var description = itemDescription.Split(' ');
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
                    if (word.Contains("\n"))
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
            }
            if(line.Trim() != "") InfoPrinter.PrintRow(line);
            InfoPrinter.PrintOneRow("");
        }
    }
}