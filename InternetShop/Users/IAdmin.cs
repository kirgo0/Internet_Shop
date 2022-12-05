namespace InternetShop.Users
{
    public interface IAdmin
    {        
        bool CreateNewItem (string itemName, int itemPrice, string itemDescription);

        bool ChangeShopItem(string itemName, string newItemName, int newItemPrice, string newItemDescription);

        bool RemoveItem (string itemName);
    }
}