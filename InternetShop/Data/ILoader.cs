using System;
using System.Collections.Generic;
using InternetShop.Shop;
using InternetShop.Users;

namespace InternetShop.Data
{
    public interface ILoader
    {
        List<ShopItem> GetShopHistory();
        List<User> GetUsers();
        List<ShopItem> GetProductList();
        double GetShopBalance();
        void LoadData(List<ShopItem> shopHistory, double shopBalance, List<User> users, List<ShopItem> productList);
    }
}