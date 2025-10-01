using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;

namespace Grocery.Core.Models
{
    public partial class GroceryList : Model
    {
        public DateOnly Date { get; set; }
        public int ClientId { get; set; }

        public Client Client { get; set; }
        public List<Product> Products { get; set; } = new();

        [ObservableProperty]
        private string color;

        public GroceryList(int id, string name, DateOnly date, string color, int clientId) : base(id, name)
        {
            Id = id;
            Name = name;
            Date = date;
            Color = color;
            ClientId = clientId;
        }
    }
}
