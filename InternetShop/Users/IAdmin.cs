using InternetShop.Shop;

namespace InternetShop.Users
{
    public interface IAdmin
    {        
        void CreateNewItem (string itemName, double itemPrice);
        void DeleteItem (string itemName);
    }
}