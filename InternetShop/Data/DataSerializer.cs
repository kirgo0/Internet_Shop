using System.Collections.Generic;
using System.IO;
using InternetShop.Shop;
using InternetShop.Users;
using Newtonsoft.Json;

namespace InternetShop.Data
{
    [JsonObject]
    public class DataSerializer
    {
        [JsonRequired]
        private ShopData _shopData;
        
        [JsonRequired]
        private ItemData _itemData;

        [JsonRequired]
        private UserData _userData;

        public void SaveData(ILoader loader)
        {
            _shopData = new ShopData(loader.GetShopHistory(),loader.GetShopBalance());
            _itemData = new ItemData(loader.GetProductList());
            _userData = new UserData(loader.GetUsers());
            var dataJson = JsonConvert.SerializeObject(this, Formatting.Indented, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            });
            File.WriteAllText("data.json", dataJson);
        }

        public void GetData(ILoader loader)
        {
            try
            {
                var dataJson = File.ReadAllText("data.json");
                var data = JsonConvert.DeserializeObject<DataSerializer>(dataJson, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto
                });
                if (data == null) return;
                _shopData = data._shopData;
                _itemData = data._itemData;
                _userData = data._userData;
                loader.LoadData(_shopData.ShopHistory,_shopData.ShopBalance,_userData.List,_itemData.List);
            }
            catch (IOException e)
            {
                loader.LoadData(new List<ShopItemHistory>(),0.0,new List<User>(),new List<ShopItem>());
            }
            loader.LoadData(_shopData.ShopHistory,_shopData.ShopBalance,_userData.List,_itemData.List);
        }
    }
}