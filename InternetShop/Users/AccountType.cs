using System.Runtime.Serialization;

namespace InternetShop.Users
{
    [DataContract]
    public enum AccountType
    {
        Admin,
        Default
    }
}