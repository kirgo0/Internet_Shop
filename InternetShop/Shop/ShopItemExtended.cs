namespace InternetShop.Shop
{
    public class ShopItemExtended : ShopItem
    {
        public string ItemDescription { get; set; }
        public ShopItemExtended(string itemName, double itemPrice, string itemDescription) : base(itemName, itemPrice)
        {
            ItemDescription = itemDescription;
        }
    }
}