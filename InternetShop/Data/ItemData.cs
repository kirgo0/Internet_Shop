using System.Collections.Generic;
using InternetShop.Shop;
using Newtonsoft.Json;

namespace InternetShop.Data
{
    [JsonObject]
    public class ItemData
    {
        public List<ShopItem> List { get; set; }

        public ItemData(List<ShopItem> shopItems)
        {
            List = shopItems;
        }
    }
    
    
}