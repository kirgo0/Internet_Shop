namespace InternetShop.Shop
{
    public class ShopItem
    {
        public string ItemName { get; set; }
        public double ItemPrice { get; set; }

        public ShopItem(string itemName, double itemPrice)
        {
            ItemName = itemName;
            ItemPrice = itemPrice;
        }
    }
}