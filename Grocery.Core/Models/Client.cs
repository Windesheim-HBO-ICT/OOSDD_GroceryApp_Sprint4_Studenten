namespace Grocery.Core.Models
{
    /// <summary>
    /// Representeert een geregistreerde klant in het systeem.
    /// Standaardrol is None; alleen  toegewezen gebruikers krijgen Admin rechten.
    /// </summary>
    public partial class Client : Model
    {
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; } = Role.None; // Default waarde conform UC13

        public Client(int id, string name, string emailAddress, string password, Role role = Role.None)
            : base(id, name)
        {
            EmailAddress = emailAddress;
            Password = password;
            Role = role;
        }
    }
}
