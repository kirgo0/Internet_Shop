using System.Collections.Generic;
using InternetShop.Users;
using Newtonsoft.Json;


namespace InternetShop.Data
{
    [JsonObject]
    public class UserData
    {
        public List<User> List { get; set; }

        public UserData(List<User> list)
        {
            List = list;
        }
    }
}