
namespace Grocery.Core.Models
{
    public class BoughtProduct
    {
        public Product Product { get; set; }
        public Client Client { get; set; }
        public GroceryList GroceryList { get; set; }
        public BoughtProduct(Client client, GroceryList groceryList, Product product)
        {
            Client = client;
            GroceryList = groceryList;
            Product = product;
        }
    }
}
