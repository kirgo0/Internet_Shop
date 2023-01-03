using System;
using Newtonsoft.Json;

namespace InternetShop.Users
{
    public class DefaultUser : User, IBuyer
    {
        [JsonConstructor]
        public DefaultUser() : base(AccountType.Default)
        {
        }
        public DefaultUser(string userName, string password) : base(userName, password, AccountType.Default)
        {
        }
        
        public bool RemoveShopItemFromCart(int pos)
        {
            try
            {
                Cart.RemoveAt(pos);
                return true;
            }
            catch (ArgumentOutOfRangeException)
            {
                return false;
            }
        }
        
        public bool AddShopItemToCart(string itemName)
        {
            var item = GetShopItem(itemName);
            if(item != null)
            {
                Cart.Add(item);
            }
            return true;
        }

        public bool BuyItemsFromCart()
        {
            double sum = 0;
            foreach (var item in Cart)
            {
                sum += item.ItemPrice;
            }

            if (sum > UserBalance) return false;
            for(var i = 0; i < Cart.Count; i++)
            {
                var basketItem = Cart[i];
                if (GetShopItem(basketItem.ItemName) != null)
                {
                    Shop.BuyShopItem(basketItem.ItemName,this);
                    UserBalance = -basketItem.ItemPrice;
                    PurchaseHistory.Add(basketItem);
                    // Successful buy message
                    Cart.Remove(basketItem);
                    i--;
                }
            }                    
            return true;
        }

        public void CheckCartProducts()
        {
            for (int i = 0; i < Cart.Count; i++)
            {
                var item = Cart[i];
                if (Shop.GetShopItem(item.ItemName) == null)
                {
                    Cart.Remove(item);
                    i--;
                }
            }
        }

        public bool BuyItem(string itemName) 
        {
            if (GetShopItem(itemName) != null)
            {
                var item = GetShopItem(itemName);
                if (item.ItemPrice > UserBalance) return false;
                Shop.BuyShopItem(item.ItemName,this);
                UserBalance = -item.ItemPrice;
                PurchaseHistory.Add(item);
                return true;
            }
            return false;
        }
    }
}