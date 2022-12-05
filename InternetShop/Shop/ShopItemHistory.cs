using Newtonsoft.Json;

namespace InternetShop.Shop
{
    public class ShopItemHistory : ShopItem
    {

        [JsonRequired]
        public string UserName { get; set; }

        public ShopItemHistory(string itemName, int itemPrice, string userName) : base(itemName, itemPrice)
        {
            UserName = userName;
        }
    }
}