using InternetShop.Shop;

namespace InternetShop.DataLoader
{
     public abstract class Loader
    {
        protected string Path { get; set; }

        public abstract void Load(OnlineShop shop);
        public abstract void Save();
    }
}