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

        public DataSerializer(ShopData shopData, ItemData itemData, UserData userData)
        {
            _shopData = shopData;
            _itemData = itemData;
            _userData = userData;
            
        }

        public void SaveData(ILoader loader)
        {
            _shopData = new ShopData(loader.GetShopHistory(),loader.GetShopBalance());
            _itemData = new ItemData(loader.GetProductList());
            _userData = new UserData(loader.GetUsers());
        }

        public void GetData(ILoader loader)
        {
            loader.LoadData(_shopData.ShopHistory,_shopData.ShopBalance,_userData.List,_itemData.List);
        }
    }
}