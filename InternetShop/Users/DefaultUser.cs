using System;
using InternetShop.Shop;

namespace InternetShop.Users
{
    public class DefaultUser : User
    {
        public DefaultUser(string userName, string password) : base(userName, password, AccountType.Default)
        {
        }
    }
}