namespace InternetShop.Shop
{
    public class ShopItem
    {
        public string ItemName { get; set; }
        public int ItemPrice { get; set; }

        public ShopItem(string itemName, int itemPrice)
        {
            ItemName = itemName;
            ItemPrice = itemPrice;
        }
    }
}