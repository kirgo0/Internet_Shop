using Newtonsoft.Json;

namespace InternetShop.Users
{
    public class DefaultUser : User
    {
        [JsonConstructor]
        public DefaultUser() : base(AccountType.Default)
        {
        }
        public DefaultUser(string userName, string password) : base(userName, password, AccountType.Default)
        {
        }
        
    }
}