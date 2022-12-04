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
        private readonly OnlineShop _shop = new OnlineShop();
        private User _user;
        private bool _signedIn;
        private bool _adminMode;
        private IAdmin _admin;
        public ShopController(OnlineShop shop)
        {
            _shop = shop;
        }

        public void Run()
        {
            // for (int i = 0; i < 28; i++)
            // {
            //     _shop.CreateNewShopItem("NVIDIA GTX 1060 6GB" + " (" + i + ")", 15600,"The PNY GeForce GTX 1060 graphics card is loaded with innovative new gaming technologies, making it the perfect choice for the latest high-definition games. Powered by NVIDIA Pascal™ – the most advanced GPU architecture ever created – the GeForce GTX 1060 delivers brilliant performance that opens the door to virtual reality and beyond.");
            // }
            _shop.GetData();
            while(true)
            {
                while (_user == null)
                {
                    PrintStartMenu();
                    string message = Console.ReadLine();
                    if(message.ToLower().Trim() == "exit")
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
                            if(message.ToLower().Trim() == "exit") return;
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
                        if(message.ToLower().Trim() == "exit") return;
                        ShopMessageHandler(message);
                    }
                    if (message.ToLower().Trim() == "exit")
                    {
                        _shop?.SaveData();
                        return;
                    }
                }
            }
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
                }if (password1 == password2)
                {
                    if (_shop.SignUp(userName, password1, password2) == null)
                    {
                        PrintMessage("This username are used already",1500);
                        if (LeaveQuestion())
                        {
                            PrintMessage("Sign Up form is closed!",1500);
                            _signedIn = false;
                            return null;
                        }
                        continue;
                    }
                    var user = _shop.SignIn(userName,password1);
                    PrintMessage(user.UserName +  " you successfully Signed Up!",1500);
                    _signedIn = true;
                    return user;
                }
                PrintMessage("Written passwords are not same",1500);
                if (LeaveQuestion())
                {
                    PrintMessage("Sign Up form is closed!",1500);
                    _signedIn = false;
                    return null;
                }
            } while (true);
            return null;
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
                    PrintShopBalance();
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
            var isFinished = false;
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
                    var findResult = _shop.GetShopItems(msg.Split(' ')[0]);
                    if (findResult.Count == 0)
                    {
                        PrintMessage("Failed to find products by the " + '"' + msg + '"' + " keyword" , 1500);
                        isFinished = true;
                        continue;
                    }
                    ProductListMenu(findResult, "Find products by a key word " + '"' + msg + '"');
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
                InfoPrinter.PrintOneRow("Do you want to come back? (Y)");
                var msg = Console.ReadLine();
                if (msg.ToLower().Trim() == "y")
                {
                    isFinished = true;
                    continue;
                }
                if (msg.Trim() == "1")
                {
                    msg = GetAnswer("Enter the amount you want to recharge");
                    try
                    {
                        Int32.Parse(msg);
                    }
                    catch (Exception e)
                    {
                        PrintMessage("The entered value must be a number");
                        continue;
                    }
                    if (Int32.Parse(msg) > 0)
                    {
                        _user.UserBalance = Int32.Parse(msg);
                    }
                    else
                    {
                        PrintMessage("Enter a positive non-zero number", 1500);
                    }
                }
            } while (!isFinished);
        }

        private void ProductListMenu(List<ShopItem> list, string message)
        {
            bool isFinished = false;
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
                        else if (rightBorder < pagesCount) rightBorder++;
                        if (rightBorder < pagesCount) rightBorder++;
                        else if (leftBorder >= 0) leftBorder--;
                    }
                    if(leftBorder >= 0) bottomMenu += "... ";
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
                InfoPrinter.PrintOneRow("Type #(number of product) for exact product information");
                InfoPrinter.PrintOneRow("Do you want to come back? (Y)");
                var msg = Console.ReadLine();
                if (msg.ToLower().Trim() == "y")
                {
                    isFinished = true;
                    continue;
                }
                if (msg.StartsWith("#") && msg.Length == 2)
                {
                    var productNumber = msg.Remove(0, 1);
                    try
                    {
                        Int32.Parse(productNumber);
                    }
                    catch (FormatException e)
                    {
                        continue;
                        throw;
                    }
                    if (Int32.Parse(productNumber) - 1 >= 0 && Int32.Parse(productNumber) - 1 < 6)
                    {
                        try
                        {
                            if (ProductMenu(currentPageItems[Int32.Parse(productNumber) - 1])) isFinished = true;
                        }
                        catch (ArgumentOutOfRangeException e)
                        {
                            Console.WriteLine("out of range");
                            continue;
                        }
                    }
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
        
        private void UserCartMenu()
        {
            bool isFinished = false;
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
                    if (msg.ToLower().Trim() == "y")
                    {
                        isFinished = true;
                        continue;
                    }
                    if (msg.Trim() == "1")
                    {
                        PrintMessage("Are you sure you want to buy all products from your cart? (Y) to confirm (any key) to come back");
                        msg = Console.ReadLine();
                        if(msg.ToLower().Trim() != "y") continue;
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
                            if (msg.ToLower().Trim() == "y")
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
                                    Int32.Parse(listNumber);
                                }
                                catch (FormatException e)
                                {
                                    continue;
                                }
                                var number = Int32.Parse(listNumber);
                                if (_user.RemoveShopItemFromCart(number-1))
                                {
                                    PrintMessage("This product was removed from your cart",1500);
                                }
                                else
                                {
                                    PrintMessage("You entered a wrong cart product number",1000);
                                }
                            }
                        }
                    }
                    if (msg.Trim() == "2")
                    {
                        PrintMessage("Your cart");
                        for (var i = 0; i < _user.Cart.Count; i++)
                        {
                            var item = _user.Cart[i];
                            InfoPrinter.PrintOneRow("#" + (i + 1) + " Product: " + item.ItemName + " Price: " + item.ItemPrice + " UAH");
                        }
                        InfoPrinter.PrintRow("");
                        InfoPrinter.PrintOneRow("Type product (#number) which you want to remove from the cart");
                        InfoPrinter.PrintOneRow("Do you want to come back? (Y)");
                        msg = Console.ReadLine();
                        if (msg.ToLower().Trim() == "y")
                        {
                            continue;
                        }
                        if (msg.StartsWith("#"))
                        {
                            var listNumber = msg.Remove(0, 1);
                            try
                            {
                                Int32.Parse(listNumber);
                            }
                            catch (FormatException e)
                            {
                                continue;
                            }
                            var number = Int32.Parse(listNumber);
                            if (_user.RemoveShopItemFromCart(number-1))
                            {
                                PrintMessage("This item was removed from your cart",1500);
                            }
                            else
                            {
                                PrintMessage("You entered a wrong cart item number",1000);
                            }
                        }
                    }
                }
                else
                {
                    InfoPrinter.PrintOneRow("Your cart is empty");
                    InfoPrinter.PrintOneRow("Do you want to come back? (Y)");
                    var msg = Console.ReadLine();
                    if (msg.ToLower().Trim() == "y")
                    {
                        isFinished = true;
                        continue;
                    }
                }
            } while(!isFinished);
        }

        private bool ProductMenu(ShopItem shopItem)
        {
            bool isFinished = false;
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
                    if (msg.ToLower().Trim() == "y")
                    { 
                        isFinished = true;
                        continue;
                    }
                    if (msg.Trim() == "1")
                    {
                        ChangeProductMenu(item);
                        isFinished = true;
                    } else if (msg.Trim() == "2")
                    {
                        PrintMessage("Are you sure you want to remove this product from shop product list?");
                        InfoPrinter.PrintOneRow("(Y) to confirm removing (any key) to come back");
                        msg = Console.ReadLine();
                        if (msg.ToLower().Trim() == "y")
                        {
                            if (_admin.RemoveItem(item.ItemName))
                            {
                                PrintMessage("This product was successfully removed",1500);
                                return true;
                            }
                            else
                            {
                                PrintMessage("Unexpected error, removing operation closed",1500);
                                return false;
                            }
                        }
                    }
                }
                else if (_user != null)
                {
                    InfoPrinter.PrintOneRow("1. Add a product to a cart");
                    InfoPrinter.PrintOneRow("2. Buy a product");
                    InfoPrinter.PrintOneRow("Do you want to come back? (Y)");
                    var msg = Console.ReadLine();
                    if (msg.ToLower().Trim() == "y")
                    { 
                        isFinished = true;
                        continue;
                    }
                    if (msg.Trim() == "1")
                    {
                        if ( _user.AddShopItemToCart(item.ItemName))
                        {
                            PrintMessage(item.ItemName + " was successfully added to your cart!",1500);
                        }
                        else
                        {
                            PrintMessage("Unexpected error, form closed",1500);
                        }
                    } else if (msg.Trim() == "2")
                    {
                        if (_user.UserBalance >= item.ItemPrice)
                        {
                            if (GetAnswer("You sure you want to buy a " + item.ItemName + "? (Y) to confirm").ToLower() == "y")
                            {
                                if(_user.BuyItem(item.ItemName)) PrintMessage("You successfully buy a " + item.ItemName,1500);
                                else PrintMessage("Unexpected error, form closed",1500);
                            }
                        }
                        else
                        {
                            PrintMessage("You don't have enough money on your balance to make a purchase",1500);
                        }
                    }
                }
                else
                {
                    InfoPrinter.PrintOneRow("Do you want to come back? (Y)");
                    var msg = Console.ReadLine();
                    if (msg.ToLower().Trim() == "y")
                    { 
                        isFinished = true;
                    }
                }
            } while (!isFinished);
            return false;
        }

        private void CreateNewProductMenu()
        {
            var isFinished = false;
            do
            {
                PrintMessage("You sure you want to create new product? (Y) to continue (any key) to come back");
                var msg = Console.ReadLine();
                if (msg.ToLower().Trim() == "y")
                {
                    string itemName ="", itemDescription = "";
                    int itemPrice = 0;
                    do
                    {
                        PrintMessage("New product");
                        InfoPrinter.PrintOneRow("Name: " + itemName, " Price: " + itemPrice + " UAH");
                        PrintProductDescription(itemDescription);
                        if (itemName.Length == 0)
                        {
                            InfoPrinter.PrintOneRow("Enter product name");
                            itemName = Console.ReadLine();
                        }
                        else if (itemPrice == 0)
                        {
                            InfoPrinter.PrintOneRow("Enter product price");
                            msg = Console.ReadLine();
                            try
                            {
                                Int32.Parse(msg);
                            }
                            catch (FormatException e)
                            {
                                PrintMessage("Price might be a number!", 1500);
                                continue;
                            }
                            if (Int32.Parse(msg) <= 0)
                            {
                                PrintMessage("Price might be a positive not zero number!", 1500);
                                continue;
                            }
                            itemPrice = Int32.Parse(msg);
                        }
                        else if (itemDescription.Length == 0)
                        {
                            InfoPrinter.PrintOneRow("Enter product description");
                            do
                            {
                                msg = Console.ReadLine();
                                itemDescription += msg.Replace("\n"," ") + " ";
                            } while (msg != "");
                        }
                        else
                        {
                            do
                            {
                                InfoPrinter.PrintOneRow("1. Create new product");
                                InfoPrinter.PrintOneRow("2. Change product name");
                                InfoPrinter.PrintOneRow("3. Change product price");
                                InfoPrinter.PrintOneRow("4. Change product description");
                                InfoPrinter.PrintOneRow("Do you want to come back? (Y)");
                                msg = Console.ReadLine();
                                if (msg.ToLower().Trim() == "y") isFinished = true;
                                if (msg.Trim() == "1")
                                {
                                    if (_admin.CreateNewItem(itemName, itemPrice, itemDescription))
                                    {
                                        PrintMessage("New product was successfully created!", 1500);
                                        isFinished = true;
                                    }
                                    else
                                    {
                                        PrintMessage("A product with this name already exists");
                                    }
                                }
                                if (msg.Trim() == "2")
                                {
                                    itemName = "";
                                    break;
                                }

                                if (msg.Trim() == "3")
                                {
                                    itemPrice = 0;
                                    break;
                                }

                                if (msg.Trim() == "4")
                                {
                                    itemDescription = "";
                                    break;
                                }
                            } while (!isFinished);
                        }
                    } while (!isFinished);
                }
                else
                {
                    isFinished = true;
                }
            } while (!isFinished);
        }

        private void ChangeProductMenu(ShopItem shopItem)
        {
            var item = (ShopItemExtended)shopItem;
            string itemName = item.ItemName, itemDescription = item.ItemDescription;
            int itemPrice = item.ItemPrice;
            bool isFinished = false;
            do
            {
                PrintMessage("Product changing menu");
                InfoPrinter.PrintOneRow("Name: " + itemName, " Price: " + itemPrice + " UAH");
                PrintProductDescription(itemDescription);
                string msg;
                if (itemName.Length == 0)
                {
                    InfoPrinter.PrintOneRow("Enter product name");
                    itemName = Console.ReadLine();
                }
                else if (itemPrice == 0)
                {
                    InfoPrinter.PrintOneRow("Enter the price of the product");
                    msg = Console.ReadLine();
                    try
                    {
                        Int32.Parse(msg);
                    }
                    catch (FormatException e)
                    {
                        PrintMessage("The price should be a number!", 1500);
                        continue;
                    }

                    if (Int32.Parse(msg) <= 0)
                    {
                        PrintMessage("The price must be a positive non-zero number!", 1500);
                        continue;
                    }

                    itemPrice = Int32.Parse(msg);
                }
                else if (itemDescription.Length == 0)
                {
                    InfoPrinter.PrintOneRow("Enter product description");
                    do
                    {
                        msg = Console.ReadLine();
                        itemDescription += msg.Replace("\n", " ") + " ";
                    } while (msg != "");
                }
                else
                {
                    do
                    {
                        InfoPrinter.PrintOneRow("1. Change product name");
                        InfoPrinter.PrintOneRow("2. Change the price of the product");
                        InfoPrinter.PrintOneRow("3. Change product description");
                        InfoPrinter.PrintOneRow("4. Save all changes");
                        InfoPrinter.PrintOneRow("Do you want to come back? (Y)");
                        msg = Console.ReadLine();
                        if (msg.ToLower().Trim() == "y") isFinished = true;
                        if (msg.Trim() == "1")
                        {
                            itemName = "";
                            break;
                        }
                        if (msg.Trim() == "2")
                        {
                            itemPrice = 0;
                            break;
                        }
                        if (msg.Trim() == "3")
                        {
                            itemDescription = "";
                            break;
                        }

                        if (msg.Trim() == "4")
                        {
                            if(_admin.ChangeShopItem(item.ItemName, itemName, itemPrice, itemDescription))
                            {
                                PrintMessage("This product was successfully changed!",1500);
                                isFinished = true;
                            }
                            else
                            {
                                PrintMessage("Unexpected error product change form closed",1500);
                            }
                        }
                    } while (!isFinished);
                }
            } while (!isFinished);
        } 
       
        private string GetAnswer(string question)
        {
            PrintMessage(question);
            return Console.ReadLine().Trim();
        }
        
        private bool LeaveQuestion()
        {
            InfoPrinter.PrintOneRow("Do you want to come back (Y) or try again? (Any key)");
            var msg = Console.ReadLine().ToLower();
            return msg.Trim() == "y";
        }

        private void PrintMessage(string message, int delay)
        {
            if (delay < 0)
            {
                PrintMessage(message);
                return;
            }
            Console.Clear();
            InfoPrinter.PrintLine();
            InfoPrinter.PrintOneRow(message);
            Thread.Sleep(delay);
        }
        
        private void PrintMessage(string message)
        {
            Console.Clear();
            InfoPrinter.PrintLine();
            InfoPrinter.PrintOneRow(message);
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

        private void PrintShopBalance()
        {
            do
            {
                PrintMessage("Shop balance");
                InfoPrinter.PrintOneRow(_shop.ShopBalance + " UAH");
            } while (!LeaveQuestion());
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
                    line += word + " ";
                    i++;
                }
            }
            InfoPrinter.PrintRow(line);
            InfoPrinter.PrintOneRow("");
        }
        
    }
}