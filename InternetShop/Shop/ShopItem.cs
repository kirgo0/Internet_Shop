using System.Runtime.Serialization;

namespace InternetShop.Shop
{
    [DataContract]
    public class ShopItem
    {
        [DataMember]
        public string ItemName { get; set; }
        [DataMember]
        public int ItemPrice { get; set; }

        public ShopItem(string itemName, int itemPrice)
        {
            ItemName = itemName;
            ItemPrice = itemPrice;
        }
    }
}