using System.Collections.Generic;
using InternetShop.Shop;
using Newtonsoft.Json;

namespace InternetShop.Data
{
    [JsonObject]
    public class ShopData
    {
        public List<ShopItem> ShopHistory;
        
        public double ShopBalance;

        public ShopData(List<ShopItem> shopHistory, double shopBalance)
        {
            ShopHistory = shopHistory;
            ShopBalance = shopBalance;
        }
    }
}