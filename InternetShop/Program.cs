using InternetShop.Shop;

namespace InternetShop
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            OnlineShop shop = new OnlineShop();
            shop.CreateNewShopItem("Aboba1",150.5);
            shop.CreateNewShopItem("Aboba2",150.0);
            shop.CreateNewShopItem("Aboba3", 1504564);
            shop.CreateNewShopItem("Aboba4", 1504564);
            shop.CreateNewShopItem("Aboba5", 1504564);
            shop.CreateNewShopItem("Aboba6", 1504564);
            shop.CreateNewShopItem("Aboba7", 1504564);
            shop.CreateNewShopItem("Aboba7", 1504564);
            shop.CreateNewShopItem("Aboba8", 1504564);
            shop.PrintProductList();
        }
    }
}