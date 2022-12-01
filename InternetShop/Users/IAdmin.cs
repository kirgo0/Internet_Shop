using InternetShop.Shop;

namespace InternetShop.Users
{
    public interface IAdmin
    {        
        bool CreateNewItem (string itemName, double itemPrice, string itemDescription);
        bool RemoveItem (string itemName);
    }
}