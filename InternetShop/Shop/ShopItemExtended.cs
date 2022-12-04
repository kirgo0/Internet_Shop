using System.Runtime.Serialization;

namespace InternetShop.Shop
{
    [DataContract]
    public class ShopItemExtended : ShopItem
    {
        [DataMember]
        public string ItemDescription { get; set; }
        public ShopItemExtended(string itemName, int itemPrice, string itemDescription) : base(itemName, itemPrice)
        {
            ItemDescription = itemDescription;
        }
    }
}