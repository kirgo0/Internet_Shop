﻿using System;
using InternetShop.Shop;

namespace InternetShop.Users
{
    public class DefaultUser : User
    {
        public DefaultUser(string userName, string password) : base(userName, password)
        {
        }

        public override ShopItem SearchShopItem(string itemName)
        {            
            Console.WriteLine(Shop.GetShopItem(itemName) != null ? "Item Founded" : "Item not founded");
            return Shop.GetShopItem(itemName);
        }
    }
}