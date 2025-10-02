namespace Grocery.Core.Models
{
    public class BoughtProductItem
    {
        public Client Client { get; set; } = null!;
        public GroceryList GroceryList { get; set; } = null!;
        public Product Product { get; set; } = null!;
    }
}
