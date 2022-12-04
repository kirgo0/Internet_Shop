using System.Collections.Generic;
using InternetShop.Shop;
using Newtonsoft.Json;

namespace InternetShop.Data
{
    [JsonObject]
    public class ShopData
    {
        public List<ShopItemHistory> ShopHistory { get; set; }
        
        public double ShopBalance;

        public ShopData(List<ShopItemHistory> shopHistory, double shopBalance)
        {
            ShopHistory = shopHistory;
            ShopBalance = shopBalance;
        }
    }
}