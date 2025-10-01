
namespace Grocery.Core.Models
{
    public partial class Client : Model
    {
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public ClientRole Role { get; set; }
        public Client(int id, string name, string emailAddress, string password, ClientRole role = ClientRole.None) : base(id, name)
        {
            EmailAddress=emailAddress;
            Password=password;
            Role = role;
        }
    }

    public enum ClientRole
    {
        Admin,
        None,
    }
}
