using System;
using InternetShop.Shop;
using InternetShop.Users;

namespace InternetShop
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            OnlineShop shop = new OnlineShop();
            for (int i = 0; i < 28; i++)
            {
                shop.CreateNewShopItem("NVIDIA GTX 1060 6GB" + " (" + i + ")", 15600,"The PNY GeForce GTX 1060 graphics card is loaded with innovative new gaming technologies, making it the perfect choice for the latest high-definition games. Powered by NVIDIA Pascal™ – the most advanced GPU architecture ever created – the GeForce GTX 1060 delivers brilliant performance that opens the door to virtual reality and beyond.");
            }

            for (int i = 0; i < 28; i++)
            {
                shop.CreateNewShopItem("GIGABYTE AMD RADEON RX 6650 8GB" + " (" + i + ")", 18900,"Powered by AMD RDNA 2 Radeon RX 6650XT Integrated with 8GB GDDR6 128-bit memory interface WINDFORCE 3X Cooling System with alternate spinning fans Screen Cooling Graphene nano lubricant RGB Fusion 2.0 Protection metal backplate 2x HDMI, 2x DisplayPort. GIGABYTE Radeon RX 6650 XT GAMING OC 8G graphics card delivers ultra-high frame rates. Get the ultimate gaming experience with powerful new compute units, amazing AMD Infinity Cache, and up to 8GB of dedicated GDDR6 memory. And, when paired with an AMD Ryzen 5000 Series desktop processor, AMD Smart Access Memory technology offers new levels of gaming performance.");
            }
            shop.CreateNewShopItem("NVIDIA GTX 1050 TI 4GB", 8500, "The GeForce GTX 1050 Ti was a mid-range graphics card by NVIDIA, launched on October 25th, 2016. Built on the 14 nm process, and based on the GP107 graphics processor, in its GP107-400-A1 variant, the card supports DirectX 12. This ensures that all modern games will run on GeForce GTX 1050 Ti. The GP107 graphics processor is an average sized chip with a die area of 132 mm² and 3,300 million transistors. It features 768 shading units, 48 texture mapping units, and 32 ROPs. NVIDIA has paired 4 GB GDDR5 memory with the GeForce GTX 1050 Ti, which are connected using a 128-bit memory interface. The GPU is operating at a frequency of 1291 MHz, which can be boosted up to 1392 MHz, memory is running at 1752 MHz (7 Gbps effective).");
            shop.CreateNewShopItem("NVIDIA RTX 4090 24GB", 103999,"The NVIDIA GeForce RTX 4090 is the ultimate GeForce GPU. It brings an enormous leap in performance, efficiency, and AI-powered graphics. Experience ultra-high performance gaming, incredibly detailed virtual worlds with ray tracing, unprecedented productivity, and new ways to create. It’s powered by the NVIDIA Ada Lovelace architecture and comes with 24 GB of G6X memory to deliver the ultimate experience for gamers and creators.");
            shop.CreateNewShopItem("MSI GeForce RTX 3060 12GB", 17050,"The GeForce RTX 3060 lets you take on the latest games using the power of Ampere, NVIDIA's 2nd generation RTX architecture. Get incredible performance with enhanced Ray Tracing Cores and Tensor Cores, new streaming multiprocessors, and high-speed G6 memory.");
            // shop.CreateNewShopItem("MSI GeForce RTX 3060 13GB", 17050,"The GeForce RTX 3060 lets you take on the latest games using the power of Ampere, NVIDIA's 2nd generation RTX architecture. Get incredible performance with enhanced Ray Tracing Cores and Tensor Cores, new streaming multiprocessors, and high-speed G6 memory.");
            shop.SignUpAdmin("Kirgo", "kirgotop", "kirgotop");
            shop.SignUp("Solify", "1234", "1234");
            ShopController controller = new ShopController(shop);
            controller.Run();
        }
    }
}