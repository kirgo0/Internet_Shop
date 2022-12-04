using System.Collections.Generic;
using System.Runtime.Serialization;
using InternetShop.Shop;
using InternetShop.Users;

namespace InternetShop.Data
{
    [DataContract]
    [KnownType(typeof(ShopItemExtended))]
    public class ItemList
    {
        [DataMember]
        public List<ShopItem> List { get; set; }

        public ItemList(List<ShopItem> shopItems)
        {
            List = shopItems;
        }
    }
    
    
}