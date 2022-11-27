using InternetShop.Shop;
using InternetShop.Users;

namespace InternetShop
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            OnlineShop shop = new OnlineShop();
            shop.CreateNewShopItem("NVIDIA GTX 1060 6GB", 15600);
            shop.CreateNewShopItem("NVIDIA GTX 1050 TI 4GB", 8500);
            shop.CreateNewShopItem("GIGABYTE AMD RADEON RX 6650 8GB", 18900);
            shop.CreateNewShopItem("NVIDIA RTX 4090 24GB", 103999);
            shop.CreateNewShopItem("MSI GeForce RTX 2060 12GB", 17050);
            User kirgo = shop.SignUp("Kirgo", "kirgotop", "kirgotop");
            shop.PrintProductList();
            kirgo.SearchShopItem("NVIDIA GTX 1060 6GB");
            
        }
    }
}