using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using InternetShop.Shop;
using InternetShop.Users;

namespace InternetShop.DataLoader
{
    public class UserDataLoader
    {
        private List<User> _users;

        public UserDataLoader(List<User> users)
        {
            _users = users;
        }

        public void Load()
        {
            
        }

        public void Save()
        {
            XmlSerializer xml = new XmlSerializer(typeof(User));

            using (FileStream fs = new FileStream("Users.xml", FileMode.OpenOrCreate))
            {
                xml.Serialize(fs, _users);
            }
        }
    }
}