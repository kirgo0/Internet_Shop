using InternetShop.Shop;

namespace InternetShop.Users
{
    public interface IAdmin
    {        
        void CreateNewItem (string itemName, double itemPrice, string itemDescription);
        void DeleteItem (string itemName);
    }
}