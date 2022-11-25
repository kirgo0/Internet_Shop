using InternetShop.Shop;

namespace InternetShop.DataLoader
{
    public static class DataLoader
    {
        public static void LoadData(string pathToUserFile, string pathToItemFile, OnlineShop shop)
        {
            UserDataLoader userDataLoader = new UserDataLoader(shop.LoadUsersList());
        }
    }
}