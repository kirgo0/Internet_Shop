using System;
using System.Runtime.Serialization;
using InternetShop.Shop;

namespace InternetShop.Users
{
    [DataContract]
    public class DefaultUser : User
    {
        public DefaultUser(string userName, string password) : base(userName, password, AccountType.Default)
        {
        }
    }
}