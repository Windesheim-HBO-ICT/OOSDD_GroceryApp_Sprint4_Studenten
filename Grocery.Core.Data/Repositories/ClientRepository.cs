using Grocery.Core.Interfaces.Repositories;
using Grocery.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace Grocery.Core.Data.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly List<Client> clientList;

        public ClientRepository()
        {
            clientList = new List<Client>
            {
                new Client(1, "User One", "user1@mail.com", "password1") { Role = Role.None },
                new Client(2, "User Two", "user2@mail.com", "password2") { Role = Role.None },
                new Client(3, "Admin User", "admin@mail.com", "admin123") { Role = Role.Admin } // admin
            };
        }

        public Client? Get(string email)
        {
            return clientList.FirstOrDefault(c => c.EmailAddress.Equals(email));
        }

        public Client? Get(int id)
        {
            return clientList.FirstOrDefault(c => c.Id == id);
        }

        public List<Client> GetAll()
        {
            return clientList;
        }
    }
}
