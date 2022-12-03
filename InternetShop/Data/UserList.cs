using System.Collections.Generic;
using System.Runtime.Serialization;
using InternetShop.Users;


namespace InternetShop.Data
{
    [DataContract]
    [KnownType(typeof(Admin))]
    [KnownType(typeof(DefaultUser))]
    public class UserList
    {
        [DataMember]
        public List<User> List { get; set; }

        public UserList(List<User> list)
        {
            List = list;
        }
    }
}