using InternetShop.Shop;

namespace InternetShop
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            OnlineShop shop = new OnlineShop();
            ShopController controller = new ShopController(shop);
            controller.Run();
        }
    }
}